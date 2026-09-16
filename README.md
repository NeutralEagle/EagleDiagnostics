# Eagle Diagnostics – Loxone Config Launcher & Diagnostics Tool

Eagle Diagnostics is a Windows **Loxone Config launcher and diagnostics utility** for working with Loxone installations.

Its main purpose is to make multiple versions of **Loxone Config** easy to find, launch, update, and manage. It also includes several practical tools for diagnostics, monitoring, and working with Loxone devices.

## What it does

### Loxone Config launcher and version management

* Finds installed Loxone Config versions automatically.
* Lists all detected versions so you can launch the one you need.
* Starts Loxone Config with a selected language.
* Checks the Loxone update feed for Test, Beta, or Release versions.
* Downloads, extracts, and installs newer Loxone Config versions when available.
* Supports optional one-click/silent Loxone Config installation.
* Rescans installed versions after Config installations or removals.

### Loxone utilities

* **EagleLoxMonitor** - receive, view, filter, save, and load Loxone monitor traffic.
* **WSSender** - connect to a Loxone device through its WebSocket endpoint and send supported device commands such as get, set, store, erase, and reboot.
* **DeflogParser** - open or paste Deflog output, detect known issues, and navigate matching log entries.

## Why Eagle Diagnostics?

Working with multiple Loxone installations often means working with different versions of Loxone Config.

Eagle Diagnostics provides a single place to find installed Config versions and launch the correct one without manually searching through installation folders.

The included diagnostic tools also provide quick access to commonly needed Loxone troubleshooting, monitoring, device communication, and log analysis functions.

## Getting started

1. Download the latest version from the [Releases page](https://github.com/NeutralEagle/EagleDiagnostics/releases/latest).
2. Run `EagleDiagnostics.exe` on Windows.
3. Eagle Diagnostics searches for installed Loxone Config versions automatically.
4. Select the required Config version and language, then choose **Start**.
5. Use **Rescan** after installing or removing a Loxone Config version.
6. Choose a release channel and select **UpdateCheck** to check for a newer Loxone Config version.

## Loxone Config updates

Eagle Diagnostics can check Loxone's update feed for available Config versions.

Supported update channels include:

* Test
* Beta
* Release

When a newer version is available, Eagle Diagnostics can download, extract, and start the installation process.

Optional silent installation can also be used where appropriate.

## Requirements

* Windows
* One or more installed versions of Loxone Config for the launcher features
* Network access for update checks, downloads, and network utilities
* Administrator approval when installing Loxone Config updates

## Build from source

Eagle Diagnostics targets **.NET 10 for Windows** and uses **Windows Forms**.

```powershell
dotnet build EagleDiagnostics.sln
```

## Notes

* Eagle Diagnostics is an independent utility and is not affiliated with Loxone.
* The update workflow installs software obtained from Loxone's update feed.
* Review the selected Loxone Config version and installer prompt before continuing with an installation.

## Contributing

Issues and pull requests are welcome.

When reporting a bug, please include:

* Eagle Diagnostics version
* Windows version
* Relevant Loxone Config version
* Steps to reproduce the issue
* Screenshots or logs where applicable
