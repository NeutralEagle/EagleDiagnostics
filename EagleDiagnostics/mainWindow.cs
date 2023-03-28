namespace EagleDiagnostics
{
    using System.IO;
    using System.Diagnostics;
    using System.Xml;
    using System.Text;
    using MyProg;
    using System.Net;
    using System.Threading.Tasks;


    //TODO
    //Major
    //Statistics editing
    //Loxone Monitor
    //deflog parser
    //možnost zadat si èísla bugù a sledovat, jejich stav, zda už byly opravené, tedy dám tam nìjaké, které chci sledovat.
    //
    //
    //Minor




    public partial class MainWindow : Form
    {
        static bool errorflag;
        readonly string appDataLocal = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        readonly string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string configDirectoryPath = "";
        private readonly List<int> configVersionList = new() { };
        int subdirlevel = 0;
        readonly List<string> languageList = new() { "CAT", "CHS", "CSY", "DEU", "ENG", "ENU", "ESN", "FRA", "ITA", "HUN", "NLD", "NOR", "PLK", "ROM", "RUS", "SKY", "TRK" };
        public MainWindow()
        {

            
            InitializeComponent();
            configDirectoryPath = FindConfig();
            
            OnLoadChecks(languageList);
            



        }

        private void ButtonConfigRescan_Click(object sender, EventArgs e)
        {
            RescanConfigInstallations();
        }

        private void RescanConfigInstallations()
        {
            comboConfigVersion.Items.Clear();
            HandleDirectory(configDirectoryPath);

        }

        private void HandleDirectory(string dir)
        {
            string path = "";
            var di = new DirectoryInfo(dir);
            var directories = di.EnumerateDirectories()
                                .OrderBy(d => d.CreationTime)
                                .Select(d => d.Name)
                                .ToList();

            foreach (string subdirectory in directories)
            {
                path = $"{dir}\\{subdirectory}";
                if (File.Exists($"{path}\\LoxoneConfig.exe"))
                {
                    string[] configPathSub = path.Split('\\')[^(subdirlevel + 1)..];
                    string comboItem = "";
                    foreach (var x in configPathSub)
                    {
                        comboItem += $"{x}\\";
                    }
                    comboItem = comboItem.TrimEnd('\\');
                    comboConfigVersion.Items.Add(comboItem);
                    continue;
                }
                else
                {
                    subdirlevel++;
                    HandleDirectory(path);
                }
                
                if (subdirlevel > 0) subdirlevel--;
            }
            comboConfigVersion.SelectedIndex = comboConfigVersion.Items.Count - 1;
        }


        private void ButtonConfigLaunch_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start($"{configDirectoryPath}\\{comboConfigVersion.Text}\\LoxoneConfig.exe", $"/Language={comboConfigLanguage.Text}");
            }
            catch (Exception ex)
            { 
            MessageBox.Show(ex.Message);
            }
            
        }

        private void ButtonStartApp_Click(object sender, EventArgs e)
        {
            string debugAttr = "";

            if (checkBoxAppDebug.Checked)
            {
                debugAttr = "--debug";
            }
            Process.Start($"{appDataLocal}\\Programs\\kerberos\\Loxone.exe", debugAttr);
        }

        private void MainWindow_FormClosing(Object sender, FormClosingEventArgs e)
        {
            if (Directory.Exists($"{appData}\\EagleDiagnostics"))
            {
                if (File.Exists($"{appData}\\EagleDiagnostics\\config.ini"))
                    File.Delete($"{appData}\\EagleDiagnostics\\config.ini");
            }
            else
                Directory.CreateDirectory($"{appData}\\EagleDiagnostics\\config.ini");
            
            
            var MyIni = new IniFile($"{appData}\\EagleDiagnostics\\config.ini");
            

            if(catalanToolStripMenuItem.Checked)                MyIni.Write("CAT", "1", "LanguageList");
            if (chineseToolStripMenuItem.Checked)                MyIni.Write("CHS", "1", "LanguageList");
            if (czechToolStripMenuItem.Checked)                MyIni.Write("CSY", "1", "LanguageList");
            if (englishToolStripMenuItem.Checked)                MyIni.Write("ENG", "1", "LanguageList");
            if (englishUSToolStripMenuItem.Checked)                MyIni.Write("ENU", "1", "LanguageList");
            if (spanishToolStripMenuItem.Checked)                MyIni.Write("ESN", "1", "LanguageList");
            if (frenchToolStripMenuItem.Checked)                MyIni.Write("FRA", "1", "LanguageList");
            if (italianToolStripMenuItem.Checked)                MyIni.Write("ITA", "1", "LanguageList");
            if (hungarianToolStripMenuItem.Checked)                MyIni.Write("HUN", "1", "LanguageList");
            if (dutchToolStripMenuItem.Checked)                MyIni.Write("NLD", "1", "LanguageList");
            if (norwegianToolStripMenuItem.Checked)                MyIni.Write("NOR", "1", "LanguageList");
            if (polishToolStripMenuItem.Checked)                MyIni.Write("PLK", "1", "LanguageList");
            if (romanianToolStripMenuItem.Checked)                MyIni.Write("ROM", "1", "LanguageList");
            if (russianToolStripMenuItem.Checked)                MyIni.Write("RUS", "1", "LanguageList");
            if (russianToolStripMenuItem.Checked)                MyIni.Write("SKY", "1", "LanguageList");
            if (turkishToolStripMenuItem.Checked)                MyIni.Write("TRK", "1", "LanguageList");



            foreach (string item in comboConfigVersion.Items)
            {
                MyIni.Write(item, "1", "Versions");
            }
            if (comboConfigLanguage.SelectedItem != null)

                MyIni.Write("DefaultLang", comboConfigLanguage.SelectedItem.ToString(), "Misc");

            else
                MyIni.Write("DefaultLang", "ENG", "Misc");
            if(comboConfigVersion.SelectedItem!=null)
            MyIni.Write("DefaultVer", comboConfigVersion.SelectedItem.ToString(), "Misc");
            if (checkBoxAppDebug.Checked)
                MyIni.Write("DefaultDebugCheck", "1", "Misc");
            else
                MyIni.Write("DefaultDebugCheck", "0", "Misc");
            if (comboReleaseType.SelectedItem != null)
                MyIni.Write("ReleaseType", comboReleaseType.SelectedItem.ToString(), "Misc");


        }
        private void OnLoadChecks(List<string> languageList)
        {
            var MyIni = new IniFile($"{appData}\\EagleDiagnostics\\config.ini");
            List<string> newList = new(languageList);
            if (IniExists()) 
            {
                foreach (var lang in languageList)
                    if (MyIni.KeyExists(lang, "LanguageList"))
                    {
                        if (MyIni.Read(lang, "LanguageList") != "1")
                            newList.Remove(lang);
                    }
                    else
                        newList.Remove(lang);
                FillLangCombo(newList);
                var entries = MyIni.GetEntryNames("Versions");
                foreach (var entry in entries)
                {
                    if (MyIni.Read(entry, "Versions") == "1")
                    {
                        comboConfigVersion.Items.Add(entry);
                    }
                }
                if (MyIni.KeyExists("DefaultLang","Misc"))
                    comboConfigLanguage.SelectedItem = MyIni.Read("DefaultLang", "Misc");
                else
                    comboConfigLanguage.SelectedItem = comboConfigLanguage.Items.Count - 1;

                if (MyIni.Read("CAT", "LanguageList") == "1") catalanToolStripMenuItem.Checked = true;
                if (MyIni.Read("CHS", "LanguageList") == "1") chineseToolStripMenuItem.Checked = true;
                if (MyIni.Read("CSY", "LanguageList") == "1") czechToolStripMenuItem.Checked = true;
                if (MyIni.Read("ENG", "LanguageList") == "1") englishToolStripMenuItem.Checked = true;
                if (MyIni.Read("ENU", "LanguageList") == "1") englishUSToolStripMenuItem.Checked = true;
                if (MyIni.Read("ENS", "LanguageList") == "1") spanishToolStripMenuItem.Checked = true;
                if (MyIni.Read("FRA", "LanguageList") == "1") frenchToolStripMenuItem.Checked = true;
                if (MyIni.Read("ITA", "LanguageList") == "1") italianToolStripMenuItem.Checked = true;
                if (MyIni.Read("HUN", "LanguageList") == "1") hungarianToolStripMenuItem.Checked = true;
                if (MyIni.Read("NLD", "LanguageList") == "1") dutchToolStripMenuItem.Checked = true;
                if (MyIni.Read("NOR", "LanguageList") == "1") norwegianToolStripMenuItem.Checked = true;
                if (MyIni.Read("PLK", "LanguageList") == "1") polishToolStripMenuItem.Checked = true;
                if (MyIni.Read("ROM", "LanguageList") == "1") romanianToolStripMenuItem.Checked = true;
                if (MyIni.Read("RUS", "LanguageList") == "1") russianToolStripMenuItem.Checked = true;
                if (MyIni.Read("SKY", "LanguageList") == "1") slovakianToolStripMenuItem.Checked = true;
                if (MyIni.Read("TRK", "LanguageList") == "1") turkishToolStripMenuItem.Checked = true;

                if (MyIni.KeyExists("DefaultVer","Misc"))
                    comboConfigVersion.SelectedItem = MyIni.Read("DefaultVer", "Misc");
                else
                    comboConfigVersion.SelectedItem = comboConfigVersion.Items.Count - 1;

                if (MyIni.KeyExists("ReleaseType", "Misc"))
                    comboReleaseType.SelectedItem = MyIni.Read("ReleaseType", "Misc");
                else
                    comboReleaseType.SelectedItem = comboReleaseType.Items.Count - 1;


                if (MyIni.KeyExists("DefaultDebugCheck", "Misc"))
                    if (MyIni.Read("DefaultDebugCheck", "Misc") == "1")
                        checkBoxAppDebug.Checked = true;
                    else checkBoxAppDebug.Checked = false;
                else checkBoxAppDebug.Checked = false;


                    
                
            }
            else
            {
                FirstStart();
            }

        }
        private bool IniExists()
        {
            if (File.Exists($"{appData}\\EagleDiagnostics\\config.ini")) return true;
            else return false;
        }


        private void FillLangCombo(List<string> languageList)
        {
            comboConfigLanguage.Items.AddRange(languageList.ToArray());
        }


        private void CreateIni(List<string> languagelist)
        {

            Directory.CreateDirectory($"{appData}\\EagleDiagnostics");
            var MyIni = new IniFile($"{appData}\\EagleDiagnostics\\config.ini");
            foreach (string lang in languagelist)
                {
                MyIni.Write(lang, "1", "LanguageList");
                }
                
                foreach (string a in comboConfigVersion.Items)
                {
                    MyIni.Write(a, "1", "Versions");
                }
                
        }

        private void FirstStart()
        {
            RescanConfigInstallations();
            CreateIni(languageList);
            FillLangCombo(languageList);
        }

        private string FindConfig()
        {
            string configPath = ExternalHelpers.RegistryRead("SOFTWARE\\WOW6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\LoxoneConfig_is1", "InstallLocation");
            
            string[] configDirectoryPathArr = configPath.Split("\\");
            int i = 0;

            foreach (var x in configDirectoryPathArr)
            {
                configDirectoryPath += $"{configDirectoryPathArr[i]}\\";
                if (x == "Loxone")
                {
                    break;
                }
                else
                {
                    i++;
                }

            }
            return configDirectoryPath;
        }

        private static string ConfigVersion(string filePath)
        {
            try
            {
                var versionInfo = FileVersionInfo.GetVersionInfo(filePath);
                
                string? version = versionInfo.FileVersion;
                if (version is null)
                    return "N/A";
                else
                return version;
            }
            catch
            {
                errorflag = true;
            }
            return "";
        }

        #region Toolstrip Menu item click methods
        private void LanguageSelectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (languageSelectToolStripMenuItem.Checked)
            {
                settingsToolStripMenuItem.DropDown.AutoClose = false;
                languageSelectToolStripMenuItem.DropDown.AutoClose = false;
            }
            else
            {
                settingsToolStripMenuItem.DropDown.AutoClose = true;
                languageSelectToolStripMenuItem.DropDown.AutoClose = true;

                settingsToolStripMenuItem.DropDown.Close();
                languageSelectToolStripMenuItem.DropDown.Close();

            }



        }
        private void CatalanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (catalanToolStripMenuItem.Checked && !comboConfigLanguage.Items.Contains("CAT"))
            {
                comboConfigLanguage.Items.Add("CAT");
                comboConfigLanguage.SelectedIndex = comboConfigLanguage.Items.IndexOf("CAT");
            }
            else if (!catalanToolStripMenuItem.Checked)
            {  
                comboConfigLanguage.Items.Remove("CAT");
                comboConfigLanguage.SelectedIndex = comboConfigLanguage.Items.Count - 1;
            }
        }

        private void ChineseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (chineseToolStripMenuItem.Checked && !comboConfigLanguage.Items.Contains("CHS"))
            {
                comboConfigLanguage.Items.Add("CHS");
                comboConfigLanguage.SelectedIndex = comboConfigLanguage.Items.IndexOf("CHS");
            }
            else if (!chineseToolStripMenuItem.Checked)
            {
                comboConfigLanguage.Items.Remove("CHS");
                comboConfigLanguage.SelectedIndex = comboConfigLanguage.Items.Count - 1;
            }
        }

        private void CzechToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (czechToolStripMenuItem.Checked && !comboConfigLanguage.Items.Contains("CSY"))
            {
                comboConfigLanguage.Items.Add("CSY");
                comboConfigLanguage.SelectedIndex = comboConfigLanguage.Items.IndexOf("CSY");
            }
            else if (!czechToolStripMenuItem.Checked)
            {
                comboConfigLanguage.Items.Remove("CSY");
                comboConfigLanguage.SelectedIndex = comboConfigLanguage.Items.Count - 1;
            }
        }

        private void GermanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (germanToolStripMenuItem.Checked && !comboConfigLanguage.Items.Contains("DEU"))
            {
                comboConfigLanguage.Items.Add("DEU");
                comboConfigLanguage.SelectedIndex = comboConfigLanguage.Items.IndexOf("DEU");
            }
            else if (!germanToolStripMenuItem.Checked)
            {
                comboConfigLanguage.Items.Remove("DEU");
                comboConfigLanguage.SelectedIndex = comboConfigLanguage.Items.Count - 1;
            }
        }

        private void EnglishToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (englishToolStripMenuItem.Checked && !comboConfigLanguage.Items.Contains("ENG"))
            {
                comboConfigLanguage.Items.Add("ENG");
                comboConfigLanguage.SelectedIndex = comboConfigLanguage.Items.IndexOf("ENG");
            }
            else if (!englishToolStripMenuItem.Checked)
            {
                comboConfigLanguage.Items.Remove("ENG");
                comboConfigLanguage.SelectedIndex = comboConfigLanguage.Items.Count - 1;
            }
        }

        private void EnglishUSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (englishUSToolStripMenuItem.Checked && !comboConfigLanguage.Items.Contains("ENU"))
            {
                comboConfigLanguage.Items.Add("ENU");
                comboConfigLanguage.SelectedIndex = comboConfigLanguage.Items.IndexOf("ENU");
            }
            else if (!englishUSToolStripMenuItem.Checked)
            {
                comboConfigLanguage.Items.Remove("ENU");
                comboConfigLanguage.SelectedIndex = comboConfigLanguage.Items.Count - 1;
            }
        }

        private void SpanishToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (spanishToolStripMenuItem.Checked && !comboConfigLanguage.Items.Contains("ESN"))
            {
                comboConfigLanguage.Items.Add("ESN");
                comboConfigLanguage.SelectedIndex = comboConfigLanguage.Items.IndexOf("ESN");
            }
            else if (!spanishToolStripMenuItem.Checked)
            {
                comboConfigLanguage.Items.Remove("ESN");
                comboConfigLanguage.SelectedIndex = comboConfigLanguage.Items.Count - 1;
            }
        }

        private void FrenchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frenchToolStripMenuItem.Checked && !comboConfigLanguage.Items.Contains("FRA"))
            {
                comboConfigLanguage.Items.Add("FRA");
                comboConfigLanguage.SelectedIndex = comboConfigLanguage.Items.IndexOf("FRA");
            }
            else if (!frenchToolStripMenuItem.Checked)
            {
                comboConfigLanguage.Items.Remove("FRA");
                comboConfigLanguage.SelectedIndex = comboConfigLanguage.Items.Count - 1;
            }
        }

        private void ItalianToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (italianToolStripMenuItem.Checked && !comboConfigLanguage.Items.Contains("ITA"))
            {
                comboConfigLanguage.Items.Add("ITA");
                comboConfigLanguage.SelectedIndex = comboConfigLanguage.Items.IndexOf("ITA");
            }
            else if (!italianToolStripMenuItem.Checked)
            {
                comboConfigLanguage.Items.Remove("ITA");
                comboConfigLanguage.SelectedIndex = comboConfigLanguage.Items.Count - 1;
            }
        }

        private void HungarianToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (hungarianToolStripMenuItem.Checked && !comboConfigLanguage.Items.Contains("HUN"))
            {
                comboConfigLanguage.Items.Add("HUN");
                comboConfigLanguage.SelectedIndex = comboConfigLanguage.Items.IndexOf("HUN");
            }
            else if (!hungarianToolStripMenuItem.Checked)
            {
                comboConfigLanguage.Items.Remove("HUN");
                comboConfigLanguage.SelectedIndex = comboConfigLanguage.Items.Count - 1;
            }
        }

        private void DutchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dutchToolStripMenuItem.Checked && !comboConfigLanguage.Items.Contains("NLD"))
            {
                comboConfigLanguage.Items.Add("NLD");
                comboConfigLanguage.SelectedIndex = comboConfigLanguage.Items.IndexOf("NLD");
            }
            else if (!dutchToolStripMenuItem.Checked)
            {
                comboConfigLanguage.Items.Remove("NLD");
                comboConfigLanguage.SelectedIndex = comboConfigLanguage.Items.Count - 1;
            }
        }

        private void NorwegianToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (norwegianToolStripMenuItem.Checked && !comboConfigLanguage.Items.Contains("NOR"))
            {
                comboConfigLanguage.Items.Add("NOR");
                comboConfigLanguage.SelectedIndex = comboConfigLanguage.Items.IndexOf("NOR");
            }
            else if (!norwegianToolStripMenuItem.Checked)
            {
                comboConfigLanguage.Items.Remove("NOR");
                comboConfigLanguage.SelectedIndex = comboConfigLanguage.Items.Count - 1;
            }
        }

        private void PolishToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (polishToolStripMenuItem.Checked && !comboConfigLanguage.Items.Contains("PLK"))
            {
                comboConfigLanguage.Items.Add("PLK");
                comboConfigLanguage.SelectedIndex = comboConfigLanguage.Items.IndexOf("PLK");
            }
            else if (!polishToolStripMenuItem.Checked)
            {
                comboConfigLanguage.Items.Remove("PLK");
                comboConfigLanguage.SelectedIndex = comboConfigLanguage.Items.Count - 1;
            }

        }

        private void RomanianToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (romanianToolStripMenuItem.Checked && !comboConfigLanguage.Items.Contains("ROM"))
            {
                comboConfigLanguage.Items.Add("ROM");
                comboConfigLanguage.SelectedIndex = comboConfigLanguage.Items.IndexOf("ROM");
            }
            else if (!romanianToolStripMenuItem.Checked)
            {
                comboConfigLanguage.Items.Remove("ROM");
                comboConfigLanguage.SelectedIndex = comboConfigLanguage.Items.Count - 1;
            }
        }

        private void RussianToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (russianToolStripMenuItem.Checked && !comboConfigLanguage.Items.Contains("RUS"))
            {
                comboConfigLanguage.Items.Add("RUS");
                comboConfigLanguage.SelectedIndex = comboConfigLanguage.Items.IndexOf("RUS");
            }
            else if (!russianToolStripMenuItem.Checked)
            {
                comboConfigLanguage.Items.Remove("RUS");
                comboConfigLanguage.SelectedIndex = comboConfigLanguage.Items.Count - 1;
            }
        }

        private void SlovakianToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (russianToolStripMenuItem.Checked && !comboConfigLanguage.Items.Contains("SKY"))
            {
                comboConfigLanguage.Items.Add("SKY");
                comboConfigLanguage.SelectedIndex = comboConfigLanguage.Items.IndexOf("SKY");
            }
            else if (!russianToolStripMenuItem.Checked)
            {
                comboConfigLanguage.Items.Remove("SKY");
                comboConfigLanguage.SelectedIndex = comboConfigLanguage.Items.Count - 1;
            }
        }

        private void TurkishToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (turkishToolStripMenuItem.Checked && !comboConfigLanguage.Items.Contains("TRK"))
            {
                comboConfigLanguage.Items.Add("TRK");
                comboConfigLanguage.SelectedIndex = comboConfigLanguage.Items.IndexOf("TRK");
            }
            else if (!turkishToolStripMenuItem.Checked)
            {
                comboConfigLanguage.Items.Remove("TRK");
                comboConfigLanguage.SelectedIndex = comboConfigLanguage.Items.Count - 1;
            }
        }
        #endregion

        private void ButtonUpdateCheck_Click(object sender, EventArgs e)
        {
            configVersionList.Clear();
            errorflag = false;
            foreach (var a in comboConfigVersion.Items)
            {
                string version = ConfigVersion($"{configDirectoryPath}\\{a}\\LoxoneConfig.exe");
                string[] versionArr = version.Split('.');
                int n = 0;
                StringBuilder stringBuilder = new();
                foreach (var b in versionArr)
                {
                    versionArr[n] = b.PadLeft(2, '0');
                    stringBuilder.Append(versionArr[n]);
                    n++;
                }
                configVersionList.Add(Convert.ToInt32(stringBuilder.ToString()));
            }
            if(errorflag) MessageBox.Show("Missing one or more Loxone Config.exe from list. Please rescan and re-run UpdateCheck");
            int max = configVersionList.Max();
            labelLastVersion.Text = $"Last installed version: {max}";
            XmlNode xml = HttpGetXML("http://update.loxone.com/updatecheck.xml");
            int ver = 0;
            string? downloadUrl = "";
            if (xml is null)
            {
                MessageBox.Show("Updatecheck XML request returned null");
            }
            else
            {
                XmlAttributeCollection? attributes = null;
                XmlNode? attrNode =
                    xml.SelectSingleNode($"/Miniserversoftware/{comboReleaseType.Text}");
                if (attrNode is null) { }
                else
                {
                    attributes = attrNode.Attributes;
                }
                
                
                if (attributes is null)
                {
                    MessageBox.Show("AttributeXML is null");
                }
                else
                {
                    foreach (XmlNode x in attributes)
                    {
                        if (x.Name == "Version")
                        {
                            if (x.Value is null)
                            { }
                            else
                            {
                                string[] text = x.Value.Split('.');
                                StringBuilder stringBuilder = new();
                                foreach (string c in text) stringBuilder.Append(c.PadLeft(2, '0'));
                                ver = Convert.ToInt32(stringBuilder.ToString());
                            }
                        }
                        if (x.Name == "Path")
                        {
                            downloadUrl = x.Value;
                        }
                    }
                
                    if (ver > max)
                    {
                        DialogResult result = MessageBox.Show($"New {comboReleaseType.Text} version is available, press OK to download.", "New version!", MessageBoxButtons.OKCancel);
                        if (result == DialogResult.OK) _ = DownloadAsync(downloadUrl);
                    }
                    else
                        MessageBox.Show($"You already have the latest {comboReleaseType.Text} version or newer: Need:{ver}/Have:{max}");
                }
            }

        }

        private static XmlNode HttpGetXML(string url)
        {
#pragma warning disable SYSLIB0014 // Typ nebo èlen je zastaralý.
            HttpWebRequest? request = (HttpWebRequest)WebRequest.Create(url);
#pragma warning restore SYSLIB0014 // Typ nebo èlen je zastaralý.
            XmlDocument xmlDoc = new();
            if (request.GetResponse() is not HttpWebResponse response) { MessageBox.Show("XML request returned null"); }
            else
            {
                xmlDoc.Load(response.GetResponseStream());
            }

            return xmlDoc;
            
        }
        private static async Task DownloadAsync(string url)
        {
            MessageBox.Show("Downloading.");
            using var client = new HttpClient();
            using var s = await client.GetStreamAsync(url);
            
            using var fs = new FileStream(url.Split('/').Last(), FileMode.OpenOrCreate);
            await s.CopyToAsync(fs);
            MessageBox.Show("Download finished. Opening download directory.");
            Process.Start("explorer.exe",Environment.CurrentDirectory);
        }
    }
}