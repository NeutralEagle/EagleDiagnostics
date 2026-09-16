using System.Diagnostics;
using System.IO.Compression;
using System.Text;
using System.Xml;

namespace EagleDiagnostics;

public static class LoxoneConfigHelpers
{
    private const string DefaultRootDirectory = @"C:\Program Files (x86)\Loxone";
    private const string UpdateCheckUrl = "http://update.loxone.com/updatecheck.xml";

    public sealed record InstalledVersionScan(int LatestVersion, bool HadError);
    public sealed record Release(int Version, string VersionString, string DownloadUrl);

    public static string FindRootDirectory()
    {
        string installLocation = ExternalHelpers.RegistryRead(
            @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\LoxoneConfig_is1",
            "InstallLocation");

        if (string.IsNullOrWhiteSpace(installLocation))
            return DefaultRootDirectory;

        string? directory = installLocation.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        while (!string.IsNullOrEmpty(directory) && !string.Equals(Path.GetFileName(directory), "Loxone", StringComparison.OrdinalIgnoreCase))
            directory = Path.GetDirectoryName(directory);

        return string.IsNullOrEmpty(directory) ? DefaultRootDirectory : directory;
    }

    public static IEnumerable<string> FindInstallations(string rootDirectory)
    {
        foreach (DirectoryInfo directory in EnumerateDirectories(new DirectoryInfo(rootDirectory)))
        {
            if (File.Exists(Path.Combine(directory.FullName, "LoxoneConfig.exe")))
                yield return Path.GetRelativePath(rootDirectory, directory.FullName);
        }
    }

    public static InstalledVersionScan ScanInstalledVersions(
        string configRootDirectory,
        IEnumerable<string> installations,
        IProgress<(int Current, int Total)> progress)
    {
        var versions = new List<int>();
        bool hadError = false;
        string[] installationList = installations.ToArray();

        for (int index = 0; index < installationList.Length; index++)
        {
            progress.Report((index + 1, installationList.Length));

            try
            {
                string executablePath = Path.Combine(configRootDirectory, installationList[index], "LoxoneConfig.exe");
                string? fileVersion = FileVersionInfo.GetVersionInfo(executablePath).FileVersion;
                if (string.IsNullOrWhiteSpace(fileVersion))
                {
                    hadError = true;
                    continue;
                }

                versions.Add(ParseVersion(fileVersion));
            }
            catch
            {
                hadError = true;
            }
        }

        return new InstalledVersionScan(versions.Count > 0 ? versions.Max() : 0, hadError);
    }

    public static async Task<Release> GetLatestReleaseAsync(string releaseType)
    {
        using HttpClient client = new();
        string xmlString = await client.GetStringAsync(UpdateCheckUrl);
        XmlDocument xmlDocument = new();
        xmlDocument.LoadXml(xmlString);

        XmlAttributeCollection? attributes = xmlDocument
            .SelectSingleNode($"/Miniserversoftware/{releaseType}")
            ?.Attributes;
        if (attributes is null)
            throw new InvalidOperationException("The update feed does not contain the selected release type.");

        string? versionString = attributes["Version"]?.Value;
        string? downloadUrl = attributes["Path"]?.Value;
        if (string.IsNullOrWhiteSpace(versionString) || string.IsNullOrWhiteSpace(downloadUrl))
            throw new InvalidOperationException("The update feed is missing version or download information.");

        return new Release(ParseVersion(versionString), versionString, downloadUrl);
    }

    public static async Task DownloadAndInstallAsync(
        string downloadUrl,
        string version,
        string releaseType,
        bool oneClickInstall,
        Action<string> reportProgress)
    {
        string destinationFilePath = Path.GetFullPath("Config.zip");
        using (var client = new HttpClientDownloadWithProgress(downloadUrl, destinationFilePath))
        {
            client.ProgressChanged += (totalFileSize, downloaded, percentage) =>
            {
                long downloadedMegabytes = downloaded / 1_000_000;
                long totalMegabytes = (totalFileSize ?? 0) / 1_000_000;
                reportProgress($"{percentage}% ({downloadedMegabytes}/{totalMegabytes}MB)");
            };
            await client.StartDownload();
        }

        reportProgress("Extracting download...");
        string destinationDirectory = Path.GetDirectoryName(destinationFilePath)!;
        await Task.Run(() => ZipFile.ExtractToDirectory(destinationFilePath, destinationDirectory, true));
        File.Delete(destinationFilePath);

        reportProgress("Download Complete");
        await InstallAsync(
            Path.Combine(destinationDirectory, "LoxoneConfigSetup.exe"), version, releaseType, oneClickInstall);
    }

    private static IEnumerable<DirectoryInfo> EnumerateDirectories(DirectoryInfo directory)
    {
        foreach (DirectoryInfo child in directory.EnumerateDirectories().OrderBy(d => d.CreationTime))
        {
            yield return child;

            foreach (DirectoryInfo descendant in EnumerateDirectories(child))
                yield return descendant;
        }
    }

    private static async Task InstallAsync(string installerPath, string version, string releaseType, bool oneClickInstall)
    {
        string folderName = $"{version} {GetReleaseSuffix(releaseType)}";
        string installPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Loxone", folderName);
        string startMenuName = $"Loxone Config {folderName}";
        var arguments = new List<string>();

        if (oneClickInstall)
            arguments.AddRange(["/VERYSILENT", "/SUPPRESSMSGBOXES", "/NORESTART"]);

        arguments.Add($"/DIR=\"{installPath}\"");
        arguments.Add($"/GROUP=\"{startMenuName}\"");

        using Process? process = Process.Start(new ProcessStartInfo
        {
            FileName = installerPath,
            Arguments = string.Join(" ", arguments),
            UseShellExecute = true,
            Verb = "runas"
        });

        if (process is null)
            throw new InvalidOperationException("Could not start Loxone Config installer.");

        await process.WaitForExitAsync();
        if (process.ExitCode != 0)
            throw new InvalidOperationException($"Loxone Config installer exited with code {process.ExitCode}.");
    }

    private static string GetReleaseSuffix(string releaseType) => releaseType.ToLowerInvariant() switch
    {
        "test" => "A",
        "beta" => "B",
        "release" => "R",
        _ => throw new ArgumentException($"Unknown release type: {releaseType}")
    };

    private static int ParseVersion(string version)
    {
        StringBuilder builder = new();
        foreach (string part in version.Split('.'))
            builder.Append(part.PadLeft(2, '0'));

        return Convert.ToInt32(builder.ToString());
    }
}
