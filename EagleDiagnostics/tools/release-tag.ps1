param(
  [string]$RepoRoot = ".",
  [string]$Remote = "origin",
  [string]$TagPrefix = "v0",
  [string]$ProjectPath = "",              # optional explicit csproj/props path relative to RepoRoot
  [switch]$CommitAndTag,
  [string]$CommitMessage = ""
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

# Run native commands safely in CLM:
# - Use Start-Process so native stderr does NOT become a PowerShell error record (NativeCommandError) in WinPS 5.1
# - Quote arguments correctly so git sees the same argv you intended (e.g., commit message with spaces)
function Exec {
  param(
    [Parameter(Mandatory = $true)]
    [string[]]$Command
  )

  $exe = $Command[0]
  $args = @()
  if ($Command.Count -gt 1) { $args = $Command[1..($Command.Count - 1)] }

  Write-Host (">> " + ($Command -join " "))

  $tmpOut = Join-Path $env:TEMP ("rt_" + $PID + "_" + (Get-Random) + ".out.txt")
  $tmpErr = Join-Path $env:TEMP ("rt_" + $PID + "_" + (Get-Random) + ".err.txt")

  # Quote/escape args for Windows CreateProcess command-line parsing
  function Quote-Arg([string]$a) {
    if ($null -eq $a) { return '""' }
    if ($a -match '[\s"]') {
      $a = $a -replace '"','\"'
      return '"' + $a + '"'
    }
    return $a
  }

  try {
    $argLine = ($args | ForEach-Object { Quote-Arg $_ }) -join ' '

    $p = Start-Process -FilePath $exe `
                       -ArgumentList $argLine `
                       -NoNewWindow `
                       -Wait `
                       -PassThru `
                       -RedirectStandardOutput $tmpOut `
                       -RedirectStandardError  $tmpErr

    $exit = $p.ExitCode

    $out = ""
    if (Test-Path -LiteralPath $tmpOut) { $out = Get-Content -LiteralPath $tmpOut -Raw }

    $err = ""
    if (Test-Path -LiteralPath $tmpErr) { $err = Get-Content -LiteralPath $tmpErr -Raw }

    if ($exit -ne 0) {
      throw "Command failed ($exit): $($Command -join ' ')`n$out`n$err"
    }

    # If there are warnings, show them but don't fail the script
    if (-not [string]::IsNullOrWhiteSpace($err)) {
      Write-Host ($err.TrimEnd())
    }

    return $out
  }
  finally {
    Remove-Item -LiteralPath $tmpOut -ErrorAction SilentlyContinue
    Remove-Item -LiteralPath $tmpErr -ErrorAction SilentlyContinue
  }
}

# CLM-safe write (UTF8; BOM may be present in Windows PowerShell 5.1, usually OK for MSBuild files)
function Save-Text([string]$path, [string]$content) {
  Set-Content -LiteralPath $path -Value $content -Encoding UTF8
}

# CLM-safe "upsert" for <Tag>Value</Tag> inside the first PropertyGroup
function Upsert-TagText([string]$text, [string]$name, [string]$value) {
  $open  = "<$name>"
  $close = "</$name>"

  # Replace existing tag value (single line or multi-line)
  if ($text -match [regex]::Escape($open)) {
    $pattern = "<$name>\s*.*?\s*</$name>"
    return [regex]::Replace($text, $pattern, "$open$value$close", "Singleline")
  }

  # Insert into the first <PropertyGroup ...>
  $pgPattern = "<PropertyGroup\b[^>]*>"
  $m = [regex]::Match($text, $pgPattern)
  if (-not $m.Success) {
    throw "No <PropertyGroup> found in file. Add one, or create Directory.Build.props."
  }

  $insertPos = $m.Index + $m.Length
  $insertion = "`r`n    $open$value$close"
  return $text.Insert($insertPos, $insertion)
}

function Update-VersionInMsbuildFile(
  [string]$msbuildFile,
  [string]$version,
  [string]$assemblyVersion,
  [string]$fileVersion,
  [string]$informationalVersion
) {
  Write-Host ">> Updating versions in: $msbuildFile"

  $text = Get-Content -LiteralPath $msbuildFile -Raw

  $text = Upsert-TagText $text "Version" $version
  $text = Upsert-TagText $text "AssemblyVersion" $assemblyVersion
  $text = Upsert-TagText $text "FileVersion" $fileVersion
  $text = Upsert-TagText $text "InformationalVersion" $informationalVersion

  Save-Text $msbuildFile $text
}

function Resolve-ProjectFile([string]$root, [string]$explicitPath) {
  if (-not [string]::IsNullOrWhiteSpace($explicitPath)) {
    $p = Join-Path $root $explicitPath
    if (-not (Test-Path -LiteralPath $p)) { throw "ProjectPath not found: $p" }
    return (Resolve-Path -LiteralPath $p).Path
  }

  # Prefer Directory.Build.props if present (best place to version everything)
  $dbp = Join-Path $root "Directory.Build.props"
  if (Test-Path -LiteralPath $dbp) {
    return (Resolve-Path -LiteralPath $dbp).Path
  }

  # Your known layout
  $known = Join-Path $root "EagleDiagnostics\EagleDiagnostics.csproj"
  if (Test-Path -LiteralPath $known) {
    return (Resolve-Path -LiteralPath $known).Path
  }

  # Fallback: first csproj excluding bin/obj
  $csproj = Get-ChildItem -Path $root -Recurse -Filter *.csproj -File |
    Where-Object { $_.FullName -notmatch '\\(bin|obj)\\' } |
    Select-Object -First 1

  if (-not $csproj) { throw "No .csproj found under $root" }
  return $csproj.FullName
}

Push-Location $RepoRoot
try {
  Exec @("git","--version") | Out-Null
  Exec @("git","rev-parse","--is-inside-work-tree") | Out-Null
  Exec @("git","fetch",$Remote,"--tags","--prune") | Out-Null

  $status = Exec @("git","status","--porcelain")
  $dirty = ($status | Out-String).Trim().Length -gt 0
  if (-not $CommitAndTag -and $dirty) {
    throw "Working tree is not clean. Commit/stash changes first (or use -CommitAndTag)."
  }

  # Next tag: v0.YY.M.D.BUILD
  $now = Get-Date
  $yy = $now.Year % 100
  $m  = $now.Month
  $d  = $now.Day

  $base = "$TagPrefix.$yy.$m.$d"
  $pattern = "$base.*"

  $tagsRaw = Exec @("git","tag","--list",$pattern)
  $tags = ($tagsRaw | Out-String).Split("`n", [System.StringSplitOptions]::RemoveEmptyEntries) |
          ForEach-Object { $_.Trim() } |
          Where-Object { $_ -ne "" }

  $maxBuild = -1
  foreach ($t in $tags) {
    $parts = $t.Split(".")
    if ($parts.Count -ge 5) {
      $last = $parts[$parts.Count - 1]
      $n = 0
      if ([int]::TryParse($last, [ref]$n)) {
        if ($n -gt $maxBuild) { $maxBuild = $n }
      }
    }
  }

  $nextBuild = $maxBuild + 1
  if ($nextBuild -lt 0) { $nextBuild = 0 }

  $tag = "$base.$nextBuild"                 # v0.26.1.28.0
  $numericVersion = "$yy.$m.$d.$nextBuild"  # 26.1.28.0

  # Update project versions
  $msbuildFile = Resolve-ProjectFile (Get-Location).Path $ProjectPath
  Update-VersionInMsbuildFile $msbuildFile $numericVersion $numericVersion $numericVersion $tag

  # Commit if requested (includes version changes)
  $status2 = Exec @("git","status","--porcelain")
  $dirty2 = ($status2 | Out-String).Trim().Length -gt 0

  if ($CommitAndTag) {
    if (-not $dirty2) { throw "Nothing to commit, but -CommitAndTag was specified." }

    $msg = $CommitMessage
    if ([string]::IsNullOrWhiteSpace($msg)) { $msg = "Release $tag" }

    Exec @("git","add","-A") | Out-Null
    Exec @("git","commit","-m",$msg) | Out-Null
  }
  else {
    if ($dirty2) {
      throw "Version fields were updated in '$msbuildFile' but are not committed. Re-run with -CommitAndTag (recommended) or commit manually, then tag."
    }
  }

  # Push current branch
  $branch = (Exec @("git","branch","--show-current") | Out-String).Trim()
  if ([string]::IsNullOrWhiteSpace($branch)) { throw "Detached HEAD; cannot push branch." }

  Exec @("git","push",$Remote,$branch) | Out-Null

  # Tag HEAD and push tag (triggers GitHub Action)
  Exec @("git","tag","-a",$tag,"-m","Release $tag") | Out-Null
  Exec @("git","push",$Remote,$tag) | Out-Null

  Write-Host ""
  Write-Host "✅ Updated versions in: $msbuildFile"
  Write-Host "✅ Created and pushed tag: $tag"
  Write-Host "GitHub Action should start from this tag push."
}
finally {
  Pop-Location
}
