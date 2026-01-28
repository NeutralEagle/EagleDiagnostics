using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace EagleDiagnostics
{
    class IniFile   // revision 12 (NET10-safe P/Invoke)
    {
        private readonly string Path;
        private readonly string EXE = Assembly.GetExecutingAssembly().GetName().Name ?? "";

        // NOTE:
        // - Use the explicit W (Unicode) entry points.
        // - nSize is in CHARACTERS (UTF-16), not bytes.
        // - Keep separate overloads for single-value reads (StringBuilder) and multi-strings (char[]).

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true,
            EntryPoint = "WritePrivateProfileStringW")]
        private static extern bool WritePrivateProfileString(
            string lpAppName,
            string? lpKeyName,
            string? lpString,
            string lpFileName);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true,
            EntryPoint = "GetPrivateProfileStringW")]
        private static extern int GetPrivateProfileString(
            string lpAppName,
            string lpKeyName,
            string lpDefault,
            StringBuilder lpReturnedString,
            int nSize,
            string lpFileName);

        // Passing lpKeyName = null returns all key names in the section (NUL-separated list)
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true,
            EntryPoint = "GetPrivateProfileStringW")]
        private static extern int GetPrivateProfileString(
            string lpAppName,
            string? lpKeyName,
            string lpDefault,
            [Out] char[] lpReturnedString,
            int nSize,
            string lpFileName);

        public IniFile(string? iniPath = null)
        {
            // Optional: guard if you might run on non-Windows
            if (!OperatingSystem.IsWindows())
                throw new PlatformNotSupportedException("IniFile uses Win32 INI APIs (kernel32) and requires Windows.");

            Path = new FileInfo(iniPath ?? (EXE + ".ini")).FullName;
        }

        public string Read(string key, string? section = null)
        {
            // If you expect long values, you can grow this buffer similarly to GetEntryNames.
            var retVal = new StringBuilder(255);
            _ = GetPrivateProfileString(section ?? EXE, key, "", retVal, retVal.Capacity, Path);
            return retVal.ToString();
        }

        public void Write(string? key, string? value, string? section = null)
        {
            _ = WritePrivateProfileString(section ?? EXE, key, value, Path);
        }

        public void DeleteKey(string key, string? section = null)
        {
            Write(key, null, section ?? EXE);
        }

        public void DeleteSection(string? section = null)
        {
            Write(null, null, section ?? EXE);
        }

        public bool KeyExists(string key, string? section = null)
        {
            return Read(key, section).Length > 0;
        }

        /// <summary>
        /// Returns all key names in a section (does not return values).
        /// </summary>
        public string[] GetEntryNames(string section)
        {
            for (int maxChars = 512; ; maxChars *= 2)
            {
                var buffer = new char[maxChars];

                // lpKeyName = null => return all key names in the section as NUL-separated strings
                int copied = GetPrivateProfileString(section, null, "", buffer, buffer.Length, Path);

                // If buffer was too small, Win32 typically returns maxChars - 2 for this multi-string form.
                if (copied < maxChars - 2)
                {
                    string entries = new string(buffer, 0, copied);
                    return entries.Split('\0', StringSplitOptions.RemoveEmptyEntries);
                }
            }
        }
    }
}