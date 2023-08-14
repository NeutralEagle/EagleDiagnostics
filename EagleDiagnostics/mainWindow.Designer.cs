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
            comboConfigLanguage = new ComboBox();
            comboConfigVersion = new ComboBox();
            buttonConfigLaunch = new Button();
            buttonConfigRescan = new Button();
            buttonStartApp = new Button();
            checkBoxAppDebug = new CheckBox();
            menuStrip1 = new MenuStrip();
            settingsToolStripMenuItem = new ToolStripMenuItem();
            languageSelectToolStripMenuItem = new ToolStripMenuItem();
            catalanToolStripMenuItem = new ToolStripMenuItem();
            chineseToolStripMenuItem = new ToolStripMenuItem();
            czechToolStripMenuItem = new ToolStripMenuItem();
            germanToolStripMenuItem = new ToolStripMenuItem();
            englishToolStripMenuItem = new ToolStripMenuItem();
            englishUSToolStripMenuItem = new ToolStripMenuItem();
            spanishToolStripMenuItem = new ToolStripMenuItem();
            frenchToolStripMenuItem = new ToolStripMenuItem();
            italianToolStripMenuItem = new ToolStripMenuItem();
            hungarianToolStripMenuItem = new ToolStripMenuItem();
            dutchToolStripMenuItem = new ToolStripMenuItem();
            norwegianToolStripMenuItem = new ToolStripMenuItem();
            polishToolStripMenuItem = new ToolStripMenuItem();
            romanianToolStripMenuItem = new ToolStripMenuItem();
            russianToolStripMenuItem = new ToolStripMenuItem();
            slovakianToolStripMenuItem = new ToolStripMenuItem();
            turkishToolStripMenuItem = new ToolStripMenuItem();
            toolsToolStripMenuItem = new ToolStripMenuItem();
            eagleLoxMonitorToolStripMenuItem = new ToolStripMenuItem();
            buttonUpdateCheck = new Button();
            labelLastVersion = new Label();
            comboReleaseType = new ComboBox();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // comboConfigLanguage
            // 
            comboConfigLanguage.DropDownStyle = ComboBoxStyle.DropDownList;
            comboConfigLanguage.FormattingEnabled = true;
            comboConfigLanguage.Location = new Point(611, 405);
            comboConfigLanguage.Name = "comboConfigLanguage";
            comboConfigLanguage.Size = new Size(88, 23);
            comboConfigLanguage.TabIndex = 0;
            // 
            // comboConfigVersion
            // 
            comboConfigVersion.DropDownStyle = ComboBoxStyle.DropDownList;
            comboConfigVersion.FormattingEnabled = true;
            comboConfigVersion.ImeMode = ImeMode.NoControl;
            comboConfigVersion.Location = new Point(355, 405);
            comboConfigVersion.Name = "comboConfigVersion";
            comboConfigVersion.Size = new Size(250, 23);
            comboConfigVersion.TabIndex = 1;
            // 
            // buttonConfigLaunch
            // 
            buttonConfigLaunch.Location = new Point(705, 405);
            buttonConfigLaunch.Name = "buttonConfigLaunch";
            buttonConfigLaunch.Size = new Size(87, 23);
            buttonConfigLaunch.TabIndex = 2;
            buttonConfigLaunch.Text = "Start";
            buttonConfigLaunch.UseVisualStyleBackColor = true;
            buttonConfigLaunch.Click += ButtonConfigLaunch_Click;
            // 
            // buttonConfigRescan
            // 
            buttonConfigRescan.Location = new Point(274, 405);
            buttonConfigRescan.Name = "buttonConfigRescan";
            buttonConfigRescan.Size = new Size(75, 23);
            buttonConfigRescan.TabIndex = 3;
            buttonConfigRescan.Text = "Rescan";
            buttonConfigRescan.UseVisualStyleBackColor = true;
            buttonConfigRescan.Click += ButtonConfigRescan_Click;
            // 
            // buttonStartApp
            // 
            buttonStartApp.Location = new Point(12, 406);
            buttonStartApp.Name = "buttonStartApp";
            buttonStartApp.Size = new Size(75, 23);
            buttonStartApp.TabIndex = 4;
            buttonStartApp.Text = "Start App";
            buttonStartApp.UseVisualStyleBackColor = true;
            buttonStartApp.Click += ButtonStartApp_Click;
            // 
            // checkBoxAppDebug
            // 
            checkBoxAppDebug.AutoSize = true;
            checkBoxAppDebug.BackColor = Color.Transparent;
            checkBoxAppDebug.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            checkBoxAppDebug.ForeColor = SystemColors.ButtonFace;
            checkBoxAppDebug.Location = new Point(93, 409);
            checkBoxAppDebug.Name = "checkBoxAppDebug";
            checkBoxAppDebug.Size = new Size(68, 19);
            checkBoxAppDebug.TabIndex = 5;
            checkBoxAppDebug.Text = "Debug?";
            checkBoxAppDebug.UseVisualStyleBackColor = false;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { settingsToolStripMenuItem, toolsToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(804, 24);
            menuStrip1.TabIndex = 6;
            menuStrip1.Text = "menuStrip1";
            // 
            // settingsToolStripMenuItem
            // 
            settingsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { languageSelectToolStripMenuItem });
            settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            settingsToolStripMenuItem.Size = new Size(61, 20);
            settingsToolStripMenuItem.Text = "Settings";
            // 
            // languageSelectToolStripMenuItem
            // 
            languageSelectToolStripMenuItem.CheckOnClick = true;
            languageSelectToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { catalanToolStripMenuItem, chineseToolStripMenuItem, czechToolStripMenuItem, germanToolStripMenuItem, englishToolStripMenuItem, englishUSToolStripMenuItem, spanishToolStripMenuItem, frenchToolStripMenuItem, italianToolStripMenuItem, hungarianToolStripMenuItem, dutchToolStripMenuItem, norwegianToolStripMenuItem, polishToolStripMenuItem, romanianToolStripMenuItem, russianToolStripMenuItem, slovakianToolStripMenuItem, turkishToolStripMenuItem });
            languageSelectToolStripMenuItem.Name = "languageSelectToolStripMenuItem";
            languageSelectToolStripMenuItem.Size = new Size(159, 22);
            languageSelectToolStripMenuItem.Text = "Language select";
            languageSelectToolStripMenuItem.Click += LanguageSelectToolStripMenuItem_Click;
            // 
            // catalanToolStripMenuItem
            // 
            catalanToolStripMenuItem.CheckOnClick = true;
            catalanToolStripMenuItem.Name = "catalanToolStripMenuItem";
            catalanToolStripMenuItem.Size = new Size(172, 22);
            catalanToolStripMenuItem.Text = "CAT - Catalan";
            catalanToolStripMenuItem.Click += CatalanToolStripMenuItem_Click;
            // 
            // chineseToolStripMenuItem
            // 
            chineseToolStripMenuItem.CheckOnClick = true;
            chineseToolStripMenuItem.Name = "chineseToolStripMenuItem";
            chineseToolStripMenuItem.Size = new Size(172, 22);
            chineseToolStripMenuItem.Text = "CHS - Chinese";
            chineseToolStripMenuItem.Click += ChineseToolStripMenuItem_Click;
            // 
            // czechToolStripMenuItem
            // 
            czechToolStripMenuItem.CheckOnClick = true;
            czechToolStripMenuItem.Name = "czechToolStripMenuItem";
            czechToolStripMenuItem.Size = new Size(172, 22);
            czechToolStripMenuItem.Text = "CSY - Czech";
            czechToolStripMenuItem.Click += CzechToolStripMenuItem_Click;
            // 
            // germanToolStripMenuItem
            // 
            germanToolStripMenuItem.CheckOnClick = true;
            germanToolStripMenuItem.Name = "germanToolStripMenuItem";
            germanToolStripMenuItem.Size = new Size(172, 22);
            germanToolStripMenuItem.Text = "DEU - German";
            germanToolStripMenuItem.Click += GermanToolStripMenuItem_Click;
            // 
            // englishToolStripMenuItem
            // 
            englishToolStripMenuItem.CheckOnClick = true;
            englishToolStripMenuItem.Name = "englishToolStripMenuItem";
            englishToolStripMenuItem.Size = new Size(172, 22);
            englishToolStripMenuItem.Text = "ENG - English (UK)";
            englishToolStripMenuItem.Click += EnglishToolStripMenuItem_Click;
            // 
            // englishUSToolStripMenuItem
            // 
            englishUSToolStripMenuItem.CheckOnClick = true;
            englishUSToolStripMenuItem.Name = "englishUSToolStripMenuItem";
            englishUSToolStripMenuItem.Size = new Size(172, 22);
            englishUSToolStripMenuItem.Text = "ENU - English (US)";
            englishUSToolStripMenuItem.Click += EnglishUSToolStripMenuItem_Click;
            // 
            // spanishToolStripMenuItem
            // 
            spanishToolStripMenuItem.CheckOnClick = true;
            spanishToolStripMenuItem.Name = "spanishToolStripMenuItem";
            spanishToolStripMenuItem.Size = new Size(172, 22);
            spanishToolStripMenuItem.Text = "ESN - Spanish";
            spanishToolStripMenuItem.Click += SpanishToolStripMenuItem_Click;
            // 
            // frenchToolStripMenuItem
            // 
            frenchToolStripMenuItem.CheckOnClick = true;
            frenchToolStripMenuItem.Name = "frenchToolStripMenuItem";
            frenchToolStripMenuItem.Size = new Size(172, 22);
            frenchToolStripMenuItem.Text = "FRA - French";
            frenchToolStripMenuItem.Click += FrenchToolStripMenuItem_Click;
            // 
            // italianToolStripMenuItem
            // 
            italianToolStripMenuItem.CheckOnClick = true;
            italianToolStripMenuItem.Name = "italianToolStripMenuItem";
            italianToolStripMenuItem.Size = new Size(172, 22);
            italianToolStripMenuItem.Text = "ITA - Italian";
            italianToolStripMenuItem.Click += ItalianToolStripMenuItem_Click;
            // 
            // hungarianToolStripMenuItem
            // 
            hungarianToolStripMenuItem.CheckOnClick = true;
            hungarianToolStripMenuItem.Name = "hungarianToolStripMenuItem";
            hungarianToolStripMenuItem.Size = new Size(172, 22);
            hungarianToolStripMenuItem.Text = "HUN - Hungarian";
            hungarianToolStripMenuItem.Click += HungarianToolStripMenuItem_Click;
            // 
            // dutchToolStripMenuItem
            // 
            dutchToolStripMenuItem.CheckOnClick = true;
            dutchToolStripMenuItem.Name = "dutchToolStripMenuItem";
            dutchToolStripMenuItem.Size = new Size(172, 22);
            dutchToolStripMenuItem.Text = "NLD - Dutch";
            dutchToolStripMenuItem.Click += DutchToolStripMenuItem_Click;
            // 
            // norwegianToolStripMenuItem
            // 
            norwegianToolStripMenuItem.CheckOnClick = true;
            norwegianToolStripMenuItem.Name = "norwegianToolStripMenuItem";
            norwegianToolStripMenuItem.Size = new Size(172, 22);
            norwegianToolStripMenuItem.Text = "NOR - Norwegian";
            norwegianToolStripMenuItem.Click += NorwegianToolStripMenuItem_Click;
            // 
            // polishToolStripMenuItem
            // 
            polishToolStripMenuItem.CheckOnClick = true;
            polishToolStripMenuItem.Name = "polishToolStripMenuItem";
            polishToolStripMenuItem.Size = new Size(172, 22);
            polishToolStripMenuItem.Text = "PLK - Polish";
            polishToolStripMenuItem.Click += PolishToolStripMenuItem_Click;
            // 
            // romanianToolStripMenuItem
            // 
            romanianToolStripMenuItem.CheckOnClick = true;
            romanianToolStripMenuItem.Name = "romanianToolStripMenuItem";
            romanianToolStripMenuItem.Size = new Size(172, 22);
            romanianToolStripMenuItem.Text = "ROM - Romanian";
            romanianToolStripMenuItem.Click += RomanianToolStripMenuItem_Click;
            // 
            // russianToolStripMenuItem
            // 
            russianToolStripMenuItem.CheckOnClick = true;
            russianToolStripMenuItem.Name = "russianToolStripMenuItem";
            russianToolStripMenuItem.Size = new Size(172, 22);
            russianToolStripMenuItem.Text = "RUS - Russian";
            russianToolStripMenuItem.Click += RussianToolStripMenuItem_Click;
            // 
            // slovakianToolStripMenuItem
            // 
            slovakianToolStripMenuItem.CheckOnClick = true;
            slovakianToolStripMenuItem.Name = "slovakianToolStripMenuItem";
            slovakianToolStripMenuItem.Size = new Size(172, 22);
            slovakianToolStripMenuItem.Text = "SKY - Slovakian";
            slovakianToolStripMenuItem.Click += SlovakianToolStripMenuItem_Click;
            // 
            // turkishToolStripMenuItem
            // 
            turkishToolStripMenuItem.CheckOnClick = true;
            turkishToolStripMenuItem.Name = "turkishToolStripMenuItem";
            turkishToolStripMenuItem.Size = new Size(172, 22);
            turkishToolStripMenuItem.Text = "TRK - Turkish";
            turkishToolStripMenuItem.Click += TurkishToolStripMenuItem_Click;
            // 
            // toolsToolStripMenuItem
            // 
            toolsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { eagleLoxMonitorToolStripMenuItem });
            toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            toolsToolStripMenuItem.Size = new Size(46, 20);
            toolsToolStripMenuItem.Text = "Tools";
            // 
            // eagleLoxMonitorToolStripMenuItem
            // 
            eagleLoxMonitorToolStripMenuItem.Name = "eagleLoxMonitorToolStripMenuItem";
            eagleLoxMonitorToolStripMenuItem.Size = new Size(164, 22);
            eagleLoxMonitorToolStripMenuItem.Text = "EagleLoxMonitor";
            eagleLoxMonitorToolStripMenuItem.Click += EagleLoxMonitorToolStripMenuItem_Click;
            // 
            // buttonUpdateCheck
            // 
            buttonUpdateCheck.Location = new Point(705, 375);
            buttonUpdateCheck.Name = "buttonUpdateCheck";
            buttonUpdateCheck.Size = new Size(87, 23);
            buttonUpdateCheck.TabIndex = 7;
            buttonUpdateCheck.Text = "UpdateCheck";
            buttonUpdateCheck.UseVisualStyleBackColor = true;
            buttonUpdateCheck.Click += ButtonUpdateCheck_Click;
            // 
            // labelLastVersion
            // 
            labelLastVersion.AutoSize = true;
            labelLastVersion.BackColor = Color.Black;
            labelLastVersion.FlatStyle = FlatStyle.Flat;
            labelLastVersion.ForeColor = SystemColors.ButtonHighlight;
            labelLastVersion.Location = new Point(611, 357);
            labelLastVersion.Name = "labelLastVersion";
            labelLastVersion.Size = new Size(181, 15);
            labelLastVersion.TabIndex = 8;
            labelLastVersion.Text = "UpdateCheck to scan last version";
            // 
            // comboReleaseType
            // 
            comboReleaseType.DropDownStyle = ComboBoxStyle.DropDownList;
            comboReleaseType.FormattingEnabled = true;
            comboReleaseType.Items.AddRange(new object[] { "Test", "Beta", "Release" });
            comboReleaseType.Location = new Point(611, 376);
            comboReleaseType.MaxDropDownItems = 3;
            comboReleaseType.Name = "comboReleaseType";
            comboReleaseType.Size = new Size(88, 23);
            comboReleaseType.TabIndex = 9;
            // 
            // MainWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Zoom;
            ClientSize = new Size(804, 441);
            Controls.Add(comboReleaseType);
            Controls.Add(labelLastVersion);
            Controls.Add(buttonUpdateCheck);
            Controls.Add(checkBoxAppDebug);
            Controls.Add(buttonStartApp);
            Controls.Add(buttonConfigRescan);
            Controls.Add(buttonConfigLaunch);
            Controls.Add(comboConfigVersion);
            Controls.Add(comboConfigLanguage);
            Controls.Add(menuStrip1);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            MaximizeBox = false;
            Name = "MainWindow";
            Text = "EagleDiagnostics";
            FormClosing += MainWindow_FormClosing;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
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
        private ToolStripMenuItem toolsToolStripMenuItem;
        private ToolStripMenuItem eagleLoxMonitorToolStripMenuItem;
    }
}