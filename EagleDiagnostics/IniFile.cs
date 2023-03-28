using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

// Change this to match your program's normal namespace
namespace MyProg
{
    class IniFile   // revision 11
    {
        readonly string Path;
#pragma warning disable CS8601 // Může jít o přiřazení s odkazem null.
        readonly string EXE = Assembly.GetExecutingAssembly().GetName().Name;
#pragma warning restore CS8601 // Může jít o přiřazení s odkazem null.

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern long WritePrivateProfileString(string Section, string? Key, string? Value, string FilePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);

#pragma warning disable CA2101 // Zadání zařazení pro argumenty řetězce P/Invoke
        [DllImport("kernel32")]

        static extern int GetPrivateProfileString(string Section, int Key,
               string Value, [MarshalAs(UnmanagedType.LPArray)] byte[] Result,
               int Size, string FileName);

        [DllImport("kernel32")]
        static extern int GetPrivateProfileString(int Section, string Key,
               string Value, [MarshalAs(UnmanagedType.LPArray)] byte[] Result,
               int Size, string FileName);
#pragma warning restore CA2101 // Zadání zařazení pro argumenty řetězce P/Invoke
        public IniFile(string? IniPath = null)
        {
            Path = new FileInfo(IniPath ?? EXE + ".ini").FullName;
        }

        public string Read(string Key, string? Section = null)
        {
            var RetVal = new StringBuilder(255);
            _ = GetPrivateProfileString(Section ?? EXE, Key, "", RetVal, 255, Path);
            return RetVal.ToString();
        }

        public void Write(string? Key, string? Value, string? Section = null)
        {
            WritePrivateProfileString(Section ?? EXE, Key, Value, Path);
        }

        public void DeleteKey(string Key, string? Section = null)
        {
            Write(Key, null, Section ?? EXE);
        }

        public void DeleteSection(string? Section = null)
        {
            Write(null, null, Section ?? EXE);
        }

        public bool KeyExists(string Key, string? Section = null)
        {
            return Read(Key, Section).Length > 0;
        }

        public string[] GetEntryNames(string section)
        {
            //    Sets the maxsize buffer to 500, if the more
            //    is required then doubles the size each time. 
            for (int maxsize = 500; true; maxsize *= 2)
            {
                //    Obtains the EntryKey information in bytes
                //    and stores them in the maxsize buffer (Bytes array).
                //    Note that the SectionHeader value has been passed.
                byte[] bytes = new byte[maxsize];
                int size = GetPrivateProfileString(section, 0, "", bytes, maxsize, Path);

                // Check the information obtained is not bigger
                // than the allocated maxsize buffer - 2 bytes.
                // if it is, then skip over the next section
                // so that the maxsize buffer can be doubled.
                if (size < maxsize - 2)
                {
                    // Converts the bytes value into an ASCII char.
                    // This is one long string.
                    string entries = Encoding.ASCII.GetString(bytes, 0,
                                              size - (size > 0 ? 1 : 0));
                    // Splits the Long string into an array based on the "\0"
                    // or null (Newline) value and returns the value(s) in an array
                    return entries.Split(new char[] { '\0' });
                }
            }
        }
    }
}

#region howToUse
/*
Open the INI file in one of the 3 following ways:
    // Creates or loads an INI file in the same directory as your executable
    // named EXE.ini (where EXE is the name of your executable)
    var MyIni = new IniFile();

    // Or specify a specific name in the current dir
    var MyIni = new IniFile("Settings.ini");

    // Or specify a specific name in a specific dir
    var MyIni = new IniFile(@"C:\Settings.ini");

You can write some values like so:
    MyIni.Write("DefaultVolume", "100");
    MyIni.Write("HomePage", "http://www.google.com");

To create a file like this:
    [MyProg]
    DefaultVolume = 100
    HomePage = http://www.google.com

To read the values out of the INI file:
    var DefaultVolume = MyIni.Read("DefaultVolume");
    var HomePage = MyIni.Read("HomePage");

Optionally, you can set [Section] 's:
    MyIni.Write("DefaultVolume", "100", "Audio");
    MyIni.Write("HomePage", "http://www.google.com", "Web");

To create a file like this:
    [Audio]
    DefaultVolume = 100

    [Web]
    HomePage = http://www.google.com

You can also check for the existence of a key like so:
    if (!MyIni.KeyExists("DefaultVolume", "Audio"))
    {
        MyIni.Write("DefaultVolume", "100", "Audio");
    }

You can delete a key like so:
    MyIni.DeleteKey("DefaultVolume", "Audio");

You can also delete a whole section (including all keys) like so:
    MyIni.DeleteSection("Web");
*/
#endregion