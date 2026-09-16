namespace EagleDiagnostics
{
    using EagleDiagnostics.DeflogParser;
    using EagleDiagnostics.Properties;
    //using EagleDiagnostics.NFCPlot;
    using EagleDiagnostics.WSSender;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    public partial class MainWindow : Form
    {

        static string downloadLabelString = "";
        string backgroundPath = "";
        readonly string appDataLocal = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        readonly string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        readonly string configDirectoryPath = "";
        readonly List<string> languageList = ["BGR", "CAT", "CHS", "CSY", "DEU", "ENG", "ENU", "ESN", "FRA", "ITA", "HUN", "NLD", "NOR", "PLK", "PTG", "ROM", "RUS", "SKY", "THA", "TRK", "UKR", "VNM"];

        #region Startup handling
        public MainWindow()
        {
            InitializeComponent();
            configDirectoryPath = LoxoneConfigHelpers.FindRootDirectory();

            OnLoadChecks(languageList);
        }
        private static void OpenLatestReleasePage()
        {
            Process.Start(new ProcessStartInfo
            {
                FileName =
                    "https://github.com/NeutralEagle/EagleDiagnostics/releases/latest",
                UseShellExecute = true
            });
        }

        private static async Task CheckForApplicationUpdateAsync(bool showUpToDateMessage)
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

            if (UpdateChecker.IsNewerVersion(latestVersion,AppInfo.Version))
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
        private void FirstStart()
        {
            RescanConfigInstallations();
            CreateIni(languageList);
            FillLangCombo(languageList);
            comboReleaseType.SelectedIndex = 0;
            comboConfigLanguage.SelectedIndex = 4;
        }
        #endregion

        #region ToolStrip menu item handlers
        #region Language switcher
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
        private void EagleLoxMonitorToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Form a = new EagleLoxMonitor();
            a.Show();
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
        #endregion

        #region Button handlers
        private void ButtonConfigRescan_Click(object sender, EventArgs e)
        {
            RescanConfigInstallations();
        }

        private void RescanConfigInstallations()
        {
            comboConfigVersion.Items.Clear();
            if (configDirectoryPath != "")
            {
                comboConfigVersion.Items.AddRange(
                    [.. LoxoneConfigHelpers.FindInstallations(configDirectoryPath)]);
            }
            else
            {
                comboConfigVersion.Items.AddRange(
                    [.. LoxoneConfigHelpers.FindInstallations("C:\\Program Files (x86)\\Loxone")]);
            }
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
                    LoxoneConfigHelpers.ScanInstalledVersions(configPath, installedConfigs, progress));

                if (scanResult.HadError)
                {
                    MessageBox.Show("Missing one or more Loxone Config.exe from list. " + "Please rescan and re-run UpdateCheck");
                }

                int max = scanResult.LatestVersion;

                labelLastVersion.Text = $"Last installed version: {max}";

                downloadLabel.Text = "Checking for latest Config version...";

                var latestRelease = await LoxoneConfigHelpers.GetLatestReleaseAsync(releaseType);
                int ver = latestRelease.Version;
                string downloadUrl = latestRelease.DownloadUrl;
                string versionString = latestRelease.VersionString;

                if (ver > max)
                {
                    downloadLabel.Text = "New Config version available";

                    DialogResult result = MessageBox.Show(
                        $"New {releaseType} version is available, press OK to download.",
                        "New version!",
                        MessageBoxButtons.OKCancel);

                    if (result == DialogResult.OK)
                    {
                        timer1.Start();
                        await LoxoneConfigHelpers.DownloadAndInstallAsync(
                            downloadUrl,
                            versionString,
                            releaseType,
                            oneClickInstall,
                            progressText => downloadLabelString = progressText);
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

                MessageBox.Show($"Updatecheck failed:\n{ex.Message}",
                    "Updatecheck",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                buttonUpdateCheck.Enabled = true;
            }
        }
        #endregion

        #region Other UI event handlers
        private void Timer1_Tick(object sender, EventArgs e)
        {
            downloadLabel.Text = downloadLabelString;
            if (downloadLabelString == "Download Complete") timer1.Stop();
        }
        private void SwitchBackground()
        {
            try
            {
                backgroundPanel.BackgroundImage = Image.FromFile(backgroundPath);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        #endregion
    }
}
