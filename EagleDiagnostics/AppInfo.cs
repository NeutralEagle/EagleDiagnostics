using System.Reflection;

namespace EagleDiagnostics;

public static class AppInfo
{
    public static string Version =>
        Assembly.GetExecutingAssembly()
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
            ?.InformationalVersion
        ?? "Unknown";
}
