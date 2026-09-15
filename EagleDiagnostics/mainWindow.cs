namespace EagleDiagnostics
{
    using EagleDiagnostics.DeflogParser;
    using EagleDiagnostics.Properties;
    using EagleDiagnostics.NFCPlot;
    using EagleDiagnostics.WSSender;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using System.Xml;


    

    public partial class MainWindow : Form
    {

        static string downloadLabelString = "";
        string backgroundPath = "";
        static bool errorflag;
        readonly string appDataLocal = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        readonly string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string configDirectoryPath = "";
        private readonly List<int> configVersionList = [];
        int subdirlevel = 0;
        readonly List<string> languageList = ["BGR", "CAT", "CHS", "CSY", "DEU", "ENG", "ENU", "ESN", "FRA", "ITA", "HUN", "NLD", "NOR", "PLK", "PTG", "ROM", "RUS", "SKY", "THA", "TRK", "UKR", "VNM"];

        public MainWindow()
        {


            InitializeComponent();
            configDirectoryPath = FindConfig();

            OnLoadChecks(languageList);




        }
        private void OpenLatestReleasePage()
        {
            Process.Start(new ProcessStartInfo
            {
                FileName =
                    "https://github.com/NeutralEagle/EagleDiagnostics/releases/latest",
                UseShellExecute = true
            });
        }

        private async Task CheckForApplicationUpdateAsync(bool showUpToDateMessage)
        {
            string? latestVersion =
                await UpdateChecker.GetLatestVersionAsync();

            // Could not check: offline, timeout, GitHub unavailable, etc.
            if (latestVersion == null)
            {
                if (showUpToDateMessage)
                {
                    MessageBox.Show(
                        "Could not check for updates.",
                        "Eagle Diagnostics",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }

                return;
            }

            if (UpdateChecker.IsNewerVersion(
                latestVersion,
                AppInfo.Version))
            {
                DialogResult result = MessageBox.Show(
                    $"A new version of Eagle Diagnostics is available.\n\n" +
                    $"Current version: {AppInfo.Version}\n" +
                    $"Latest version:  {latestVersion}\n\n" +
                    $"Open the download page?",
                    "Update available",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information);

                if (result == DialogResult.Yes)
                {
                    OpenLatestReleasePage();
                }
            }
            else if (showUpToDateMessage)
            {
                MessageBox.Show(
                    $"Eagle Diagnostics is up to date.\n\n" +
                    $"Version: {AppInfo.Version}",
                    "No updates available",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void ButtonConfigRescan_Click(object sender, EventArgs e)
        {
            RescanConfigInstallations();
        }

        private void RescanConfigInstallations()
        {
            comboConfigVersion.Items.Clear();
            if (configDirectoryPath != "")
            {
                HandleDirectory(configDirectoryPath);
            }
            else
            {
                HandleDirectory("C:\\Program Files (x86)\\Loxone");
            }
        }

        private void HandleDirectory(string dir)
        {
            string path = "";
            var di = new DirectoryInfo(dir);
            try
            {
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
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
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
            if (File.Exists($"{appDataLocal}\\Programs\\kerberos\\Loxone.exe"))
                Process.Start($"{appDataLocal}\\Programs\\kerberos\\Loxone.exe", debugAttr);
            else MessageBox.Show("No Loxone App found, please install it first.");

        }

        #region INI saving and loading
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


            foreach (ToolStripMenuItem item in languageSelectToolStripMenuItem.DropDownItems
                                                     .OfType<ToolStripMenuItem>())
            {
                if (item.Tag is string code)
                    MyIni.Write(code, item.Checked ? "1" : "0", "LanguageList");
            }


            foreach (string item in comboConfigVersion.Items)
            {
                MyIni.Write(item, "1", "Versions");
            }
            if (comboConfigLanguage.SelectedItem != null)

                MyIni.Write("DefaultLang", comboConfigLanguage.SelectedItem.ToString(), "Misc");

            else
                MyIni.Write("DefaultLang", "ENG", "Misc");
            if (comboConfigVersion.SelectedItem != null)
                MyIni.Write("DefaultVer", comboConfigVersion.SelectedItem.ToString(), "Misc");
            if (checkBoxAppDebug.Checked)
                MyIni.Write("DefaultDebugCheck", "1", "Misc");
            else
                MyIni.Write("DefaultDebugCheck", "0", "Misc");
            if (comboReleaseType.SelectedItem != null)
                MyIni.Write("ReleaseType", comboReleaseType.SelectedItem.ToString(), "Misc");
            if (BackgroundImage != Resources.LoxBg)
                MyIni.Write("Background", backgroundPath, "Misc");
            else
                MyIni.Write("Background", "LoxBg", "Misc");


        }

        private async void OnLoadChecks(List<string> languageList)
        {
            var MyIni = new IniFile($"{appData}\\EagleDiagnostics\\config.ini");
            List<string> newList = [.. languageList];
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
                if (MyIni.KeyExists("DefaultLang", "Misc"))
                    comboConfigLanguage.SelectedItem = MyIni.Read("DefaultLang", "Misc");
                else
                    comboConfigLanguage.SelectedItem = comboConfigLanguage.Items.Count - 1;

                foreach (ToolStripMenuItem item in languageSelectToolStripMenuItem.DropDownItems
                                                         .OfType<ToolStripMenuItem>())
                {
                    if (item.Tag is string code)
                    {
                        item.Checked = MyIni.Read(code, "LanguageList") == "1";
                    }
                }


                if (MyIni.KeyExists("DefaultVer", "Misc"))
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

                if (MyIni.KeyExists("Background", "Misc"))
                {
                    var bg = MyIni.Read("Background", "Misc");
                    if (bg != "LoxBg")
                    {
                        backgroundPath = bg;
                        SwitchBackground();
                    }
                }


            }
            else
            {
                FirstStart();
            }
            if (checkForUpdatesToolStripMenuItem.Checked)
            {
                await CheckForApplicationUpdateAsync(showUpToDateMessage: false);
            }
        }

        private bool IniExists()
        {
            if (File.Exists($"{appData}\\EagleDiagnostics\\config.ini")) return true;
            else return false;
        }


        private void FillLangCombo(List<string> languageList)
        {
            comboConfigLanguage.Items.AddRange([.. languageList]);
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
        #endregion
        private void FirstStart()
        {
            RescanConfigInstallations();
            CreateIni(languageList);
            FillLangCombo(languageList);
            comboReleaseType.SelectedIndex = 0;
            comboConfigLanguage.SelectedIndex = 4;
        }

        private string FindConfig()
        {
            string configPath = ExternalHelpers.RegistryRead("SOFTWARE\\WOW6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\LoxoneConfig_is1", "InstallLocation");
            if (configPath != "")
            {
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
            }
            else configDirectoryPath = "C:\\Program Files (x86)\\Loxone";
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
            if (slovakianToolStripMenuItem.Checked && !comboConfigLanguage.Items.Contains("SKY"))
            {
                comboConfigLanguage.Items.Add("SKY");
                comboConfigLanguage.SelectedIndex = comboConfigLanguage.Items.IndexOf("SKY");
            }
            else if (!slovakianToolStripMenuItem.Checked)
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

        private void BulgarianToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (bulgarianToolStripMenuItem.Checked && !comboConfigLanguage.Items.Contains("BGR"))
            {
                comboConfigLanguage.Items.Add("BGR");
                comboConfigLanguage.SelectedIndex = comboConfigLanguage.Items.IndexOf("BGR");
            }
            else if (!bulgarianToolStripMenuItem.Checked)
            {
                comboConfigLanguage.Items.Remove("BGR");
                comboConfigLanguage.SelectedIndex = comboConfigLanguage.Items.Count - 1;
            }
        }

        private void VietnameseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (vietnameseToolStripMenuItem.Checked && !comboConfigLanguage.Items.Contains("VNM"))
            {
                comboConfigLanguage.Items.Add("VNM");
                comboConfigLanguage.SelectedIndex = comboConfigLanguage.Items.IndexOf("VNM");
            }
            else if (!vietnameseToolStripMenuItem.Checked)
            {
                comboConfigLanguage.Items.Remove("VNM");
                comboConfigLanguage.SelectedIndex = comboConfigLanguage.Items.Count - 1;
            }
        }
        private void PortugeseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (portugeseToolStripMenuItem.Checked && !comboConfigLanguage.Items.Contains("PTG"))
            {
                comboConfigLanguage.Items.Add("PTG");
                comboConfigLanguage.SelectedIndex = comboConfigLanguage.Items.IndexOf("PTG");
            }
            else if (!portugeseToolStripMenuItem.Checked)
            {
                comboConfigLanguage.Items.Remove("PTG");
                comboConfigLanguage.SelectedIndex = comboConfigLanguage.Items.Count - 1;
            }
        }

        private void UkrainianToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ukrainianToolStripMenuItem.Checked && !comboConfigLanguage.Items.Contains("UKR"))
            {
                comboConfigLanguage.Items.Add("UKR");
                comboConfigLanguage.SelectedIndex = comboConfigLanguage.Items.IndexOf("UKR");
            }
            else if (!ukrainianToolStripMenuItem.Checked)
            {
                comboConfigLanguage.Items.Remove("UKR");
                comboConfigLanguage.SelectedIndex = comboConfigLanguage.Items.Count - 1;
            }
        }

        private void TaiwaneseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (taiwaneseToolStripMenuItem.Checked && !comboConfigLanguage.Items.Contains("THA"))
            {
                comboConfigLanguage.Items.Add("THA");
                comboConfigLanguage.SelectedIndex = comboConfigLanguage.Items.IndexOf("THA");
            }
            else if (!taiwaneseToolStripMenuItem.Checked)
            {
                comboConfigLanguage.Items.Remove("THA");
                comboConfigLanguage.SelectedIndex = comboConfigLanguage.Items.Count - 1;
            }
        }
        #endregion

        private async void ButtonUpdateCheck_Click(object sender, EventArgs e)
        {
            buttonUpdateCheck.Enabled = false;
            bool oneClickInstall = oneClickConfigInstallToolStripMenuItem.Checked;
            try
            {
                // Take copies of UI values before entering Task.Run().
                // WinForms controls must only be accessed from the UI thread.
                var installedConfigs = comboConfigVersion.Items
                    .Cast<object>()
                    .Select(x => x.ToString()!)
                    .ToList();

                string releaseType = comboReleaseType.Text;
                string configPath = configDirectoryPath;

                // Progress<T> posts updates back onto the WinForms UI thread.
                IProgress<(int Current, int Total)> progress =
                    new Progress<(int Current, int Total)>(p =>
                    {
                        downloadLabel.Text =
                            $"Checking installed Config versions... {p.Current}/{p.Total}";
                    });

                // Scan installed Config versions on a worker thread.
                var scanResult = await Task.Run(() =>
                {
                    var versions = new List<int>();
                    bool hadError = false;

                    int current = 0;
                    int total = installedConfigs.Count;

                    foreach (string config in installedConfigs)
                    {
                        current++;
                        progress.Report((current, total));

                        try
                        {
                            string filePath = $"{configPath}\\{config}\\LoxoneConfig.exe";

                            var versionInfo = FileVersionInfo.GetVersionInfo(filePath);

                            string? version = versionInfo.FileVersion;

                            if (string.IsNullOrWhiteSpace(version))
                            {
                                hadError = true;
                                continue;
                            }

                            string[] versionArr = version.Split('.');
                            StringBuilder stringBuilder = new();

                            foreach (string part in versionArr)
                            {
                                stringBuilder.Append(part.PadLeft(2, '0'));
                            }

                            versions.Add(Convert.ToInt32(stringBuilder.ToString()));
                        }
                        catch
                        {
                            hadError = true;
                        }
                    }

                    return (
                        Versions: versions,
                        HadError: hadError
                    );
                });

                configVersionList.Clear();
                configVersionList.AddRange(scanResult.Versions);

                errorflag = scanResult.HadError;

                if (errorflag)
                {
                    MessageBox.Show("Missing one or more Loxone Config.exe from list. " + "Please rescan and re-run UpdateCheck");
                }

                int max = configVersionList.Count > 0
                    ? configVersionList.Max()
                    : 0;

                labelLastVersion.Text =$"Last installed version: {max}";

                downloadLabel.Text ="Checking for latest Config version...";

                XmlNode xml;

                try
                {
                    xml = await HttpGetXMLAsync("http://update.loxone.com/updatecheck.xml");
                }
                catch (Exception ex)
                {
                    downloadLabel.Text = "Update check failed";

                    MessageBox.Show($"Updatecheck failed:\n{ex.Message}",
                        "Updatecheck",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return;
                }

                if (xml == null)
                {
                    downloadLabel.Text ="Update check failed";

                    MessageBox.Show("Updatecheck XML request returned null");

                    return;
                }

                int ver = 0;
                string? downloadUrl = null;
                string? versionString = null;

                XmlNode? attrNode =
                    xml.SelectSingleNode($"/Miniserversoftware/{releaseType}");

                XmlAttributeCollection? attributes =
                    attrNode?.Attributes;

                if (attributes == null)
                {
                    downloadLabel.Text ="Update check failed";

                    MessageBox.Show("AttributeXML is null");

                    return;
                }

                foreach (XmlNode x in attributes)
                {
                    if (x.Name == "Version" && x.Value != null)
                    {
                        versionString = x.Value;

                        string[] versionParts = x.Value.Split('.');

                        StringBuilder stringBuilder = new();

                        foreach (string part in versionParts)
                        {
                            stringBuilder.Append(part.PadLeft(2, '0'));
                        }

                        ver = Convert.ToInt32(stringBuilder.ToString());
                    }
                    if (x.Name == "Path")
                    {
                        downloadUrl = x.Value;
                    }
                }

                if (ver > max)
                {
                    downloadLabel.Text = "New Config version available";

                    DialogResult result = MessageBox.Show(
                        $"New {releaseType} version is available, press OK to download.",
                        "New version!",
                        MessageBoxButtons.OKCancel);

                    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(downloadUrl))
                    {
                        // Timer transfers downloadLabelString to downloadLabel.
                        timer1.Start();

                        if (!string.IsNullOrWhiteSpace(versionString))
                        {
                            await DownloadAsync(downloadUrl,versionString,releaseType,oneClickInstall);
                        }
                        RescanConfigInstallations();
                    }
                    else
                    {
                        downloadLabel.Text = "Download cancelled";
                    }
                }
                else
                {
                    downloadLabel.Text = "Update check complete";

                    MessageBox.Show(
                        $"You already have the latest {releaseType} version or newer: " +
                        $"Need:{ver}/Have:{max}");
                }
            }
            catch (Exception ex)
            {
                downloadLabel.Text = "Update check failed";

                MessageBox.Show(
                    $"Update check failed:\n{ex.Message}",
                    "Updatecheck",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                buttonUpdateCheck.Enabled = true;
            }
        }

        /*
        private static XmlNode HttpGetXMLAsync(string url)
        {

            HttpWebRequest? request = (HttpWebRequest)WebRequest.Create(url);

            XmlDocument xmlDoc = new();
            if (request.GetResponse() is not HttpWebResponse response) { MessageBox.Show("XML request returned null"); }
            else
            {
                xmlDoc.Load(response.GetResponseStream());
            }

            return xmlDoc;

        }*/
        private static string GetReleaseSuffix(string releaseType)
        {
            return releaseType.ToLowerInvariant() switch
            {
                "test" => "A",
                "beta" => "B",
                "release" => "R",
                _ => throw new ArgumentException(
                    $"Unknown release type: {releaseType}")
            };
        }
        private static async Task InstallConfigAsync(string installerPath, string version, string releaseType, bool oneClickInstall)
        {
            string suffix = GetReleaseSuffix(releaseType);
            string folderName = $"{version} {suffix}";
            string installPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),"Loxone",folderName);
            string startMenuName = $"Loxone Config {folderName}";

            var arguments = new List<string>();

            if (oneClickInstall) {
                arguments.Add("/VERYSILENT");
                arguments.Add("/SUPPRESSMSGBOXES");
                arguments.Add("/NORESTART");
            }
                
            arguments.Add($"/DIR=\"{installPath}\"");
            arguments.Add($"/GROUP=\"{startMenuName}\"");

            var startInfo = new ProcessStartInfo
            {
                FileName = installerPath,
                Arguments = string.Join(" ", arguments),
                UseShellExecute = true,
                Verb = "runas"
            };

            using Process? process = Process.Start(startInfo);

            if (process == null)
                throw new InvalidOperationException("Could not start Loxone Config installer.");

            await process.WaitForExitAsync();

            if (process.ExitCode != 0)
            {
                throw new InvalidOperationException($"Loxone Config installer exited with code {process.ExitCode}.");
            }
        }

        private static async Task<XmlNode> HttpGetXMLAsync(string url)
        {
            using HttpClient client = new();
            using HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            using HttpContent content = response.Content;
            string xmlString = await content.ReadAsStringAsync();

            XmlDocument xmlDoc = new();
            xmlDoc.LoadXml(xmlString);
            return xmlDoc;
        }

        private static async Task DownloadAsync(string url,string versionString,string releaseType,bool oneClickInstall)
        {
            var destinationFilePath = Path.GetFullPath("Config.zip");

            using (var client = new HttpClientDownloadWithProgress(url, destinationFilePath))
            {
                client.ProgressChanged += (totalFileSize,totalBytesDownloaded,progressPercentage) =>
                    {
                        var megaBytesDownloaded = totalBytesDownloaded / 1000000;

                        var megaBytesSize = totalFileSize / 1000000;

                        downloadLabelString =
                            $"{progressPercentage}% " +
                            $"({megaBytesDownloaded}/{megaBytesSize}MB)";
                    };

                await client.StartDownload();
            }

            downloadLabelString = "Extracting download...";

            var destinationFolder =destinationFilePath[..^10];

            // ZIP extraction is synchronous and can take a while,
            // so keep it off the WinForms UI thread.
            await Task.Run(() =>
            {
                System.IO.Compression.ZipFile
                    .ExtractToDirectory(destinationFilePath, destinationFolder, true);
            });

            File.Delete(destinationFilePath);

            downloadLabelString = "Download Complete";

            string installerPath = Path.Combine(destinationFolder,"LoxoneConfigSetup.exe");
            await InstallConfigAsync(installerPath, versionString, releaseType, oneClickInstall);
        }


        private void EagleLoxMonitorToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Form a = new EagleLoxMonitor();
            a.Show();
        }

        #region Background switching
        private void SwitchBackground()
        {
            try
            {
                backgroundPanel.BackgroundImage = Image.FromFile(backgroundPath);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void BackgroundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var backgroundDialogResult = backgroundFileDialog.ShowDialog();
            if (backgroundDialogResult == DialogResult.OK)
            {
                backgroundPath = backgroundFileDialog.FileName;
                SwitchBackground();
            }
        }
        #endregion

        private void Timer1_Tick(object sender, EventArgs e)
        {
            downloadLabel.Text = downloadLabelString;
            if (downloadLabelString == "Download Complete") timer1.Stop();
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("EagleDiagnostics\nVersion: " + AppInfo.Version);
        }

        private void WSSenderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form a = new WSSenderForm();
            a.Show();
        }

        private void OpenGitHubPageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var url = "https://github.com/NeutralEagle/EagleDiagnostics";

            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }

        private void DeflogParserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form a = new DeflogAnalyzerForm();
            a.Show();
        }

        
    }
    public static class AppInfo
    {
        public static string Version =>
            Assembly.GetExecutingAssembly()
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                ?.InformationalVersion
            ?? "Unknown";
    }
    public class HttpClientDownloadWithProgress(string downloadUrl, string destinationFilePath) : IDisposable
    {
        private readonly string _downloadUrl = downloadUrl;
        private readonly string _destinationFilePath = destinationFilePath;

        private HttpClient? _httpClient;

        public delegate void ProgressChangedHandler(long? totalFileSize, long totalBytesDownloaded, double? progressPercentage);

        public event ProgressChangedHandler? ProgressChanged;

        public async Task StartDownload()
        {
            _httpClient = new HttpClient { Timeout = TimeSpan.FromDays(1) };

            using var response = await _httpClient.GetAsync(_downloadUrl, HttpCompletionOption.ResponseHeadersRead);
            await DownloadFileFromHttpResponseMessage(response);
        }

        private async Task DownloadFileFromHttpResponseMessage(HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();

            var totalBytes = response.Content.Headers.ContentLength;

            using var contentStream = await response.Content.ReadAsStreamAsync();
            await ProcessContentStream(totalBytes, contentStream);
        }

        private async Task ProcessContentStream(long? totalDownloadSize, Stream contentStream)
        {
            var totalBytesRead = 0L;
            var readCount = 0L;
            var buffer = new byte[8192];
            var isMoreToRead = true;

            using var fileStream = new FileStream(_destinationFilePath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true);
            do
            {
                var bytesRead = await contentStream.ReadAsync(buffer);
                if (bytesRead == 0)
                {
                    isMoreToRead = false;
                    TriggerProgressChanged(totalDownloadSize, totalBytesRead);
                    continue;
                }

                await fileStream.WriteAsync(buffer.AsMemory(0, bytesRead));

                totalBytesRead += bytesRead;
                readCount += 1;

                if (readCount % 100 == 0)
                    TriggerProgressChanged(totalDownloadSize, totalBytesRead);
            }
            while (isMoreToRead);
        }

        private void TriggerProgressChanged(long? totalDownloadSize, long totalBytesRead)
        {
            if (ProgressChanged == null)
                return;

            double? progressPercentage = null;
            if (totalDownloadSize.HasValue)
                progressPercentage = Math.Round((double)totalBytesRead / totalDownloadSize.Value * 100, 2);

            ProgressChanged(totalDownloadSize, totalBytesRead, progressPercentage);
        }

        

        
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}