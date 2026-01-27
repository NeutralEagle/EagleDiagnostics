using Microsoft.Win32;

namespace EagleDiagnostics
{
    internal class ExternalHelpers
    {
        public static string RegistryRead(string path, string value)
        {
            try
            {
                using RegistryKey? key = Registry.LocalMachine.OpenSubKey(path);
                if (key is null) return "";

                object? o = key.GetValue(value);
                return o?.ToString() ?? "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return "";
            }
        }
    }
}
