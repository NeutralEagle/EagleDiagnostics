using Microsoft.Win32;

namespace EagleDiagnostics
{
    internal class ExternalHelpers
    {
        public static string RegistryRead(string path,string value)
        {
            try
            {
                using (RegistryKey? key = Registry.LocalMachine.OpenSubKey(path))
                {
                    if (key != null)
                    {
                        Object? o = key.GetValue(value);
                        if (o != null)
                        {
#pragma warning disable CS8603 // Může jít o vrácený odkaz null.
                            return o.ToString();
#pragma warning restore CS8603 // Může jít o vrácený odkaz null.
                        }
                        else
                        {
                            return "";
                        }
                    }
                }
            }
            catch (Exception ex)  //just for demonstration...it's always best to handle specific exceptions
            {
                MessageBox.Show(ex.Message);
            }
            return "";
        }
    }
}
