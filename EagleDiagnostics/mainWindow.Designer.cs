namespace EagleDiagnostics
{
    partial class MainWindow
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.comboConfigLanguage = new System.Windows.Forms.ComboBox();
            this.comboConfigVersion = new System.Windows.Forms.ComboBox();
            this.buttonConfigLaunch = new System.Windows.Forms.Button();
            this.buttonConfigRescan = new System.Windows.Forms.Button();
            this.buttonStartApp = new System.Windows.Forms.Button();
            this.checkBoxAppDebug = new System.Windows.Forms.CheckBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.languageSelectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.catalanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chineseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.czechToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.germanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.englishToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.englishUSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.spanishToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.frenchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.italianToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hungarianToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dutchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.norwegianToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.polishToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.romanianToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.russianToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.slovakianToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.turkishToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonUpdateCheck = new System.Windows.Forms.Button();
            this.labelLastVersion = new System.Windows.Forms.Label();
            this.comboReleaseType = new System.Windows.Forms.ComboBox();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboConfigLanguage
            // 
            this.comboConfigLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboConfigLanguage.FormattingEnabled = true;
            this.comboConfigLanguage.Location = new System.Drawing.Point(611, 405);
            this.comboConfigLanguage.Name = "comboConfigLanguage";
            this.comboConfigLanguage.Size = new System.Drawing.Size(88, 23);
            this.comboConfigLanguage.TabIndex = 0;
            // 
            // comboConfigVersion
            // 
            this.comboConfigVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboConfigVersion.FormattingEnabled = true;
            this.comboConfigVersion.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.comboConfigVersion.Location = new System.Drawing.Point(355, 405);
            this.comboConfigVersion.Name = "comboConfigVersion";
            this.comboConfigVersion.Size = new System.Drawing.Size(250, 23);
            this.comboConfigVersion.TabIndex = 1;
            // 
            // buttonConfigLaunch
            // 
            this.buttonConfigLaunch.Location = new System.Drawing.Point(705, 405);
            this.buttonConfigLaunch.Name = "buttonConfigLaunch";
            this.buttonConfigLaunch.Size = new System.Drawing.Size(87, 23);
            this.buttonConfigLaunch.TabIndex = 2;
            this.buttonConfigLaunch.Text = "Start";
            this.buttonConfigLaunch.UseVisualStyleBackColor = true;
            this.buttonConfigLaunch.Click += new System.EventHandler(this.ButtonConfigLaunch_Click);
            // 
            // buttonConfigRescan
            // 
            this.buttonConfigRescan.Location = new System.Drawing.Point(274, 405);
            this.buttonConfigRescan.Name = "buttonConfigRescan";
            this.buttonConfigRescan.Size = new System.Drawing.Size(75, 23);
            this.buttonConfigRescan.TabIndex = 3;
            this.buttonConfigRescan.Text = "Rescan";
            this.buttonConfigRescan.UseVisualStyleBackColor = true;
            this.buttonConfigRescan.Click += new System.EventHandler(this.ButtonConfigRescan_Click);
            // 
            // buttonStartApp
            // 
            this.buttonStartApp.Location = new System.Drawing.Point(12, 406);
            this.buttonStartApp.Name = "buttonStartApp";
            this.buttonStartApp.Size = new System.Drawing.Size(75, 23);
            this.buttonStartApp.TabIndex = 4;
            this.buttonStartApp.Text = "Start App";
            this.buttonStartApp.UseVisualStyleBackColor = true;
            this.buttonStartApp.Click += new System.EventHandler(this.ButtonStartApp_Click);
            // 
            // checkBoxAppDebug
            // 
            this.checkBoxAppDebug.AutoSize = true;
            this.checkBoxAppDebug.BackColor = System.Drawing.Color.Transparent;
            this.checkBoxAppDebug.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.checkBoxAppDebug.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.checkBoxAppDebug.Location = new System.Drawing.Point(93, 409);
            this.checkBoxAppDebug.Name = "checkBoxAppDebug";
            this.checkBoxAppDebug.Size = new System.Drawing.Size(68, 19);
            this.checkBoxAppDebug.TabIndex = 5;
            this.checkBoxAppDebug.Text = "Debug?";
            this.checkBoxAppDebug.UseVisualStyleBackColor = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(804, 24);
            this.menuStrip1.TabIndex = 6;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.languageSelectToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // languageSelectToolStripMenuItem
            // 
            this.languageSelectToolStripMenuItem.CheckOnClick = true;
            this.languageSelectToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.catalanToolStripMenuItem,
            this.chineseToolStripMenuItem,
            this.czechToolStripMenuItem,
            this.germanToolStripMenuItem,
            this.englishToolStripMenuItem,
            this.englishUSToolStripMenuItem,
            this.spanishToolStripMenuItem,
            this.frenchToolStripMenuItem,
            this.italianToolStripMenuItem,
            this.hungarianToolStripMenuItem,
            this.dutchToolStripMenuItem,
            this.norwegianToolStripMenuItem,
            this.polishToolStripMenuItem,
            this.romanianToolStripMenuItem,
            this.russianToolStripMenuItem,
            this.slovakianToolStripMenuItem,
            this.turkishToolStripMenuItem});
            this.languageSelectToolStripMenuItem.Name = "languageSelectToolStripMenuItem";
            this.languageSelectToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.languageSelectToolStripMenuItem.Text = "Language select";
            this.languageSelectToolStripMenuItem.Click += new System.EventHandler(this.LanguageSelectToolStripMenuItem_Click);
            // 
            // catalanToolStripMenuItem
            // 
            this.catalanToolStripMenuItem.CheckOnClick = true;
            this.catalanToolStripMenuItem.Name = "catalanToolStripMenuItem";
            this.catalanToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.catalanToolStripMenuItem.Text = "CAT - Catalan";
            this.catalanToolStripMenuItem.Click += new System.EventHandler(this.CatalanToolStripMenuItem_Click);
            // 
            // chineseToolStripMenuItem
            // 
            this.chineseToolStripMenuItem.CheckOnClick = true;
            this.chineseToolStripMenuItem.Name = "chineseToolStripMenuItem";
            this.chineseToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.chineseToolStripMenuItem.Text = "CHS - Chinese";
            this.chineseToolStripMenuItem.Click += new System.EventHandler(this.ChineseToolStripMenuItem_Click);
            // 
            // czechToolStripMenuItem
            // 
            this.czechToolStripMenuItem.CheckOnClick = true;
            this.czechToolStripMenuItem.Name = "czechToolStripMenuItem";
            this.czechToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.czechToolStripMenuItem.Text = "CSY - Czech";
            this.czechToolStripMenuItem.Click += new System.EventHandler(this.CzechToolStripMenuItem_Click);
            // 
            // germanToolStripMenuItem
            // 
            this.germanToolStripMenuItem.CheckOnClick = true;
            this.germanToolStripMenuItem.Name = "germanToolStripMenuItem";
            this.germanToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.germanToolStripMenuItem.Text = "DEU - German";
            this.germanToolStripMenuItem.Click += new System.EventHandler(this.GermanToolStripMenuItem_Click);
            // 
            // englishToolStripMenuItem
            // 
            this.englishToolStripMenuItem.CheckOnClick = true;
            this.englishToolStripMenuItem.Name = "englishToolStripMenuItem";
            this.englishToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.englishToolStripMenuItem.Text = "ENG - English (UK)";
            this.englishToolStripMenuItem.Click += new System.EventHandler(this.EnglishToolStripMenuItem_Click);
            // 
            // englishUSToolStripMenuItem
            // 
            this.englishUSToolStripMenuItem.CheckOnClick = true;
            this.englishUSToolStripMenuItem.Name = "englishUSToolStripMenuItem";
            this.englishUSToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.englishUSToolStripMenuItem.Text = "ENU - English (US)";
            this.englishUSToolStripMenuItem.Click += new System.EventHandler(this.EnglishUSToolStripMenuItem_Click);
            // 
            // spanishToolStripMenuItem
            // 
            this.spanishToolStripMenuItem.CheckOnClick = true;
            this.spanishToolStripMenuItem.Name = "spanishToolStripMenuItem";
            this.spanishToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.spanishToolStripMenuItem.Text = "ESN - Spanish";
            this.spanishToolStripMenuItem.Click += new System.EventHandler(this.SpanishToolStripMenuItem_Click);
            // 
            // frenchToolStripMenuItem
            // 
            this.frenchToolStripMenuItem.CheckOnClick = true;
            this.frenchToolStripMenuItem.Name = "frenchToolStripMenuItem";
            this.frenchToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.frenchToolStripMenuItem.Text = "FRA - French";
            this.frenchToolStripMenuItem.Click += new System.EventHandler(this.FrenchToolStripMenuItem_Click);
            // 
            // italianToolStripMenuItem
            // 
            this.italianToolStripMenuItem.CheckOnClick = true;
            this.italianToolStripMenuItem.Name = "italianToolStripMenuItem";
            this.italianToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.italianToolStripMenuItem.Text = "ITA - Italian";
            this.italianToolStripMenuItem.Click += new System.EventHandler(this.ItalianToolStripMenuItem_Click);
            // 
            // hungarianToolStripMenuItem
            // 
            this.hungarianToolStripMenuItem.CheckOnClick = true;
            this.hungarianToolStripMenuItem.Name = "hungarianToolStripMenuItem";
            this.hungarianToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.hungarianToolStripMenuItem.Text = "HUN - Hungarian";
            this.hungarianToolStripMenuItem.Click += new System.EventHandler(this.HungarianToolStripMenuItem_Click);
            // 
            // dutchToolStripMenuItem
            // 
            this.dutchToolStripMenuItem.CheckOnClick = true;
            this.dutchToolStripMenuItem.Name = "dutchToolStripMenuItem";
            this.dutchToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.dutchToolStripMenuItem.Text = "NLD - Dutch";
            this.dutchToolStripMenuItem.Click += new System.EventHandler(this.DutchToolStripMenuItem_Click);
            // 
            // norwegianToolStripMenuItem
            // 
            this.norwegianToolStripMenuItem.CheckOnClick = true;
            this.norwegianToolStripMenuItem.Name = "norwegianToolStripMenuItem";
            this.norwegianToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.norwegianToolStripMenuItem.Text = "NOR - Norwegian";
            this.norwegianToolStripMenuItem.Click += new System.EventHandler(this.NorwegianToolStripMenuItem_Click);
            // 
            // polishToolStripMenuItem
            // 
            this.polishToolStripMenuItem.CheckOnClick = true;
            this.polishToolStripMenuItem.Name = "polishToolStripMenuItem";
            this.polishToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.polishToolStripMenuItem.Text = "PLK - Polish";
            this.polishToolStripMenuItem.Click += new System.EventHandler(this.PolishToolStripMenuItem_Click);
            // 
            // romanianToolStripMenuItem
            // 
            this.romanianToolStripMenuItem.CheckOnClick = true;
            this.romanianToolStripMenuItem.Name = "romanianToolStripMenuItem";
            this.romanianToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.romanianToolStripMenuItem.Text = "ROM - Romanian";
            this.romanianToolStripMenuItem.Click += new System.EventHandler(this.RomanianToolStripMenuItem_Click);
            // 
            // russianToolStripMenuItem
            // 
            this.russianToolStripMenuItem.CheckOnClick = true;
            this.russianToolStripMenuItem.Name = "russianToolStripMenuItem";
            this.russianToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.russianToolStripMenuItem.Text = "RUS - Russian";
            this.russianToolStripMenuItem.Click += new System.EventHandler(this.RussianToolStripMenuItem_Click);
            // 
            // slovakianToolStripMenuItem
            // 
            this.slovakianToolStripMenuItem.CheckOnClick = true;
            this.slovakianToolStripMenuItem.Name = "slovakianToolStripMenuItem";
            this.slovakianToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.slovakianToolStripMenuItem.Text = "SKY - Slovakian";
            this.slovakianToolStripMenuItem.Click += new System.EventHandler(this.SlovakianToolStripMenuItem_Click);
            // 
            // turkishToolStripMenuItem
            // 
            this.turkishToolStripMenuItem.CheckOnClick = true;
            this.turkishToolStripMenuItem.Name = "turkishToolStripMenuItem";
            this.turkishToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.turkishToolStripMenuItem.Text = "TRK - Turkish";
            this.turkishToolStripMenuItem.Click += new System.EventHandler(this.TurkishToolStripMenuItem_Click);
            // 
            // buttonUpdateCheck
            // 
            this.buttonUpdateCheck.Location = new System.Drawing.Point(705, 375);
            this.buttonUpdateCheck.Name = "buttonUpdateCheck";
            this.buttonUpdateCheck.Size = new System.Drawing.Size(87, 23);
            this.buttonUpdateCheck.TabIndex = 7;
            this.buttonUpdateCheck.Text = "UpdateCheck";
            this.buttonUpdateCheck.UseVisualStyleBackColor = true;
            this.buttonUpdateCheck.Click += new System.EventHandler(this.ButtonUpdateCheck_Click);
            // 
            // labelLastVersion
            // 
            this.labelLastVersion.AutoSize = true;
            this.labelLastVersion.BackColor = System.Drawing.Color.Black;
            this.labelLastVersion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.labelLastVersion.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.labelLastVersion.Location = new System.Drawing.Point(611, 357);
            this.labelLastVersion.Name = "labelLastVersion";
            this.labelLastVersion.Size = new System.Drawing.Size(181, 15);
            this.labelLastVersion.TabIndex = 8;
            this.labelLastVersion.Text = "UpdateCheck to scan last version";
            // 
            // comboReleaseType
            // 
            this.comboReleaseType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboReleaseType.FormattingEnabled = true;
            this.comboReleaseType.Items.AddRange(new object[] {
            "Test",
            "Beta",
            "Release"});
            this.comboReleaseType.Location = new System.Drawing.Point(611, 376);
            this.comboReleaseType.MaxDropDownItems = 3;
            this.comboReleaseType.Name = "comboReleaseType";
            this.comboReleaseType.Size = new System.Drawing.Size(88, 23);
            this.comboReleaseType.TabIndex = 9;
            this.comboReleaseType.SelectedIndex = 2;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::EagleDiagnostics.Properties.Resources.Background;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(804, 441);
            this.Controls.Add(this.comboReleaseType);
            this.Controls.Add(this.labelLastVersion);
            this.Controls.Add(this.buttonUpdateCheck);
            this.Controls.Add(this.checkBoxAppDebug);
            this.Controls.Add(this.buttonStartApp);
            this.Controls.Add(this.buttonConfigRescan);
            this.Controls.Add(this.buttonConfigLaunch);
            this.Controls.Add(this.comboConfigVersion);
            this.Controls.Add(this.comboConfigLanguage);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "MainWindow";
            this.Text = "EagleDiagnostics";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void languageSelectToolStripMenuItem_closed(object sender, EventArgs e)
        {
            comboConfigLanguage.SelectedIndex = comboConfigLanguage.Items.Count - 1;
        }



        #endregion

        private ComboBox comboConfigLanguage;
        private ComboBox comboConfigVersion;
        private Button buttonConfigLaunch;
        private Button buttonConfigRescan;
        private Button buttonStartApp;
        private CheckBox checkBoxAppDebug;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private ToolStripMenuItem languageSelectToolStripMenuItem;
        private ToolStripMenuItem catalanToolStripMenuItem;
        private ToolStripMenuItem chineseToolStripMenuItem;
        private ToolStripMenuItem czechToolStripMenuItem;
        private ToolStripMenuItem germanToolStripMenuItem;
        private ToolStripMenuItem englishToolStripMenuItem;
        private ToolStripMenuItem englishUSToolStripMenuItem;
        private ToolStripMenuItem spanishToolStripMenuItem;
        private ToolStripMenuItem frenchToolStripMenuItem;
        private ToolStripMenuItem italianToolStripMenuItem;
        private ToolStripMenuItem hungarianToolStripMenuItem;
        private ToolStripMenuItem dutchToolStripMenuItem;
        private ToolStripMenuItem norwegianToolStripMenuItem;
        private ToolStripMenuItem polishToolStripMenuItem;
        private ToolStripMenuItem romanianToolStripMenuItem;
        private ToolStripMenuItem russianToolStripMenuItem;
        private ToolStripMenuItem slovakianToolStripMenuItem;
        private ToolStripMenuItem turkishToolStripMenuItem;
        private Button buttonUpdateCheck;
        private Label labelLastVersion;
        private ComboBox comboReleaseType;
    }
}