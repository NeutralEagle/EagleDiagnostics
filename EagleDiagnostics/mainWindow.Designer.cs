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
            components = new System.ComponentModel.Container();
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
            bulgarianToolStripMenuItem = new ToolStripMenuItem();
            vietnameseToolStripMenuItem = new ToolStripMenuItem();
            backgroundToolStripMenuItem = new ToolStripMenuItem();
            toolsToolStripMenuItem = new ToolStripMenuItem();
            eagleLoxMonitorToolStripMenuItem = new ToolStripMenuItem();
            wSSenderToolStripMenuItem = new ToolStripMenuItem();
            helpToolStripMenuItem = new ToolStripMenuItem();
            aboutToolStripMenuItem = new ToolStripMenuItem();
            openGitHubPageToolStripMenuItem = new ToolStripMenuItem();
            buttonUpdateCheck = new Button();
            labelLastVersion = new Label();
            comboReleaseType = new ComboBox();
            backgroundFileDialog = new OpenFileDialog();
            downloadLabel = new Label();
            timer1 = new System.Windows.Forms.Timer(components);
            backgroundPanel = new Panel();
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
            checkBoxAppDebug.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
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
            menuStrip1.Items.AddRange(new ToolStripItem[] { settingsToolStripMenuItem, toolsToolStripMenuItem, helpToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(804, 24);
            menuStrip1.TabIndex = 6;
            menuStrip1.Text = "menuStrip1";
            // 
            // settingsToolStripMenuItem
            // 
            settingsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { languageSelectToolStripMenuItem, backgroundToolStripMenuItem });
            settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            settingsToolStripMenuItem.Size = new Size(61, 20);
            settingsToolStripMenuItem.Text = "Settings";
            // 
            // languageSelectToolStripMenuItem
            // 
            languageSelectToolStripMenuItem.CheckOnClick = true;
            languageSelectToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { catalanToolStripMenuItem, chineseToolStripMenuItem, czechToolStripMenuItem, germanToolStripMenuItem, englishToolStripMenuItem, englishUSToolStripMenuItem, spanishToolStripMenuItem, frenchToolStripMenuItem, italianToolStripMenuItem, hungarianToolStripMenuItem, dutchToolStripMenuItem, norwegianToolStripMenuItem, polishToolStripMenuItem, romanianToolStripMenuItem, russianToolStripMenuItem, slovakianToolStripMenuItem, turkishToolStripMenuItem, bulgarianToolStripMenuItem, vietnameseToolStripMenuItem });
            languageSelectToolStripMenuItem.Name = "languageSelectToolStripMenuItem";
            languageSelectToolStripMenuItem.Size = new Size(180, 22);
            languageSelectToolStripMenuItem.Text = "Language select";
            languageSelectToolStripMenuItem.Click += LanguageSelectToolStripMenuItem_Click;
            // 
            // catalanToolStripMenuItem
            // 
            catalanToolStripMenuItem.CheckOnClick = true;
            catalanToolStripMenuItem.Name = "catalanToolStripMenuItem";
            catalanToolStripMenuItem.Size = new Size(180, 22);
            catalanToolStripMenuItem.Tag = "CAT";
            catalanToolStripMenuItem.Text = "CAT - Catalan";
            catalanToolStripMenuItem.Click += CatalanToolStripMenuItem_Click;
            // 
            // chineseToolStripMenuItem
            // 
            chineseToolStripMenuItem.CheckOnClick = true;
            chineseToolStripMenuItem.Name = "chineseToolStripMenuItem";
            chineseToolStripMenuItem.Size = new Size(180, 22);
            chineseToolStripMenuItem.Tag = "CHS";
            chineseToolStripMenuItem.Text = "CHS - Chinese";
            chineseToolStripMenuItem.Click += ChineseToolStripMenuItem_Click;
            // 
            // czechToolStripMenuItem
            // 
            czechToolStripMenuItem.CheckOnClick = true;
            czechToolStripMenuItem.Name = "czechToolStripMenuItem";
            czechToolStripMenuItem.Size = new Size(180, 22);
            czechToolStripMenuItem.Tag = "CSY";
            czechToolStripMenuItem.Text = "CSY - Czech";
            czechToolStripMenuItem.Click += CzechToolStripMenuItem_Click;
            // 
            // germanToolStripMenuItem
            // 
            germanToolStripMenuItem.CheckOnClick = true;
            germanToolStripMenuItem.Name = "germanToolStripMenuItem";
            germanToolStripMenuItem.Size = new Size(180, 22);
            germanToolStripMenuItem.Tag = "DEU";
            germanToolStripMenuItem.Text = "DEU - German";
            germanToolStripMenuItem.Click += GermanToolStripMenuItem_Click;
            // 
            // englishToolStripMenuItem
            // 
            englishToolStripMenuItem.CheckOnClick = true;
            englishToolStripMenuItem.Name = "englishToolStripMenuItem";
            englishToolStripMenuItem.Size = new Size(180, 22);
            englishToolStripMenuItem.Tag = "ENG";
            englishToolStripMenuItem.Text = "ENG - English (UK)";
            englishToolStripMenuItem.Click += EnglishToolStripMenuItem_Click;
            // 
            // englishUSToolStripMenuItem
            // 
            englishUSToolStripMenuItem.CheckOnClick = true;
            englishUSToolStripMenuItem.Name = "englishUSToolStripMenuItem";
            englishUSToolStripMenuItem.Size = new Size(180, 22);
            englishUSToolStripMenuItem.Tag = "ENU";
            englishUSToolStripMenuItem.Text = "ENU - English (US)";
            englishUSToolStripMenuItem.Click += EnglishUSToolStripMenuItem_Click;
            // 
            // spanishToolStripMenuItem
            // 
            spanishToolStripMenuItem.CheckOnClick = true;
            spanishToolStripMenuItem.Name = "spanishToolStripMenuItem";
            spanishToolStripMenuItem.Size = new Size(180, 22);
            spanishToolStripMenuItem.Tag = "ESN";
            spanishToolStripMenuItem.Text = "ESN - Spanish";
            spanishToolStripMenuItem.Click += SpanishToolStripMenuItem_Click;
            // 
            // frenchToolStripMenuItem
            // 
            frenchToolStripMenuItem.CheckOnClick = true;
            frenchToolStripMenuItem.Name = "frenchToolStripMenuItem";
            frenchToolStripMenuItem.Size = new Size(180, 22);
            frenchToolStripMenuItem.Tag = "FRA";
            frenchToolStripMenuItem.Text = "FRA - French";
            frenchToolStripMenuItem.Click += FrenchToolStripMenuItem_Click;
            // 
            // italianToolStripMenuItem
            // 
            italianToolStripMenuItem.CheckOnClick = true;
            italianToolStripMenuItem.Name = "italianToolStripMenuItem";
            italianToolStripMenuItem.Size = new Size(180, 22);
            italianToolStripMenuItem.Tag = "ITA";
            italianToolStripMenuItem.Text = "ITA - Italian";
            italianToolStripMenuItem.Click += ItalianToolStripMenuItem_Click;
            // 
            // hungarianToolStripMenuItem
            // 
            hungarianToolStripMenuItem.CheckOnClick = true;
            hungarianToolStripMenuItem.Name = "hungarianToolStripMenuItem";
            hungarianToolStripMenuItem.Size = new Size(180, 22);
            hungarianToolStripMenuItem.Tag = "HUN";
            hungarianToolStripMenuItem.Text = "HUN - Hungarian";
            hungarianToolStripMenuItem.Click += HungarianToolStripMenuItem_Click;
            // 
            // dutchToolStripMenuItem
            // 
            dutchToolStripMenuItem.CheckOnClick = true;
            dutchToolStripMenuItem.Name = "dutchToolStripMenuItem";
            dutchToolStripMenuItem.Size = new Size(180, 22);
            dutchToolStripMenuItem.Tag = "NLD";
            dutchToolStripMenuItem.Text = "NLD - Dutch";
            dutchToolStripMenuItem.Click += DutchToolStripMenuItem_Click;
            // 
            // norwegianToolStripMenuItem
            // 
            norwegianToolStripMenuItem.CheckOnClick = true;
            norwegianToolStripMenuItem.Name = "norwegianToolStripMenuItem";
            norwegianToolStripMenuItem.Size = new Size(180, 22);
            norwegianToolStripMenuItem.Tag = "NOR";
            norwegianToolStripMenuItem.Text = "NOR - Norwegian";
            norwegianToolStripMenuItem.Click += NorwegianToolStripMenuItem_Click;
            // 
            // polishToolStripMenuItem
            // 
            polishToolStripMenuItem.CheckOnClick = true;
            polishToolStripMenuItem.Name = "polishToolStripMenuItem";
            polishToolStripMenuItem.Size = new Size(180, 22);
            polishToolStripMenuItem.Tag = "PLK";
            polishToolStripMenuItem.Text = "PLK - Polish";
            polishToolStripMenuItem.Click += PolishToolStripMenuItem_Click;
            // 
            // romanianToolStripMenuItem
            // 
            romanianToolStripMenuItem.CheckOnClick = true;
            romanianToolStripMenuItem.Name = "romanianToolStripMenuItem";
            romanianToolStripMenuItem.Size = new Size(180, 22);
            romanianToolStripMenuItem.Tag = "ROM";
            romanianToolStripMenuItem.Text = "ROM - Romanian";
            romanianToolStripMenuItem.Click += RomanianToolStripMenuItem_Click;
            // 
            // russianToolStripMenuItem
            // 
            russianToolStripMenuItem.CheckOnClick = true;
            russianToolStripMenuItem.Name = "russianToolStripMenuItem";
            russianToolStripMenuItem.Size = new Size(180, 22);
            russianToolStripMenuItem.Tag = "RUS";
            russianToolStripMenuItem.Text = "RUS - Russian";
            russianToolStripMenuItem.Click += RussianToolStripMenuItem_Click;
            // 
            // slovakianToolStripMenuItem
            // 
            slovakianToolStripMenuItem.CheckOnClick = true;
            slovakianToolStripMenuItem.Name = "slovakianToolStripMenuItem";
            slovakianToolStripMenuItem.Size = new Size(180, 22);
            slovakianToolStripMenuItem.Tag = "SKY";
            slovakianToolStripMenuItem.Text = "SKY - Slovakian";
            slovakianToolStripMenuItem.Click += SlovakianToolStripMenuItem_Click;
            // 
            // turkishToolStripMenuItem
            // 
            turkishToolStripMenuItem.CheckOnClick = true;
            turkishToolStripMenuItem.Name = "turkishToolStripMenuItem";
            turkishToolStripMenuItem.Size = new Size(180, 22);
            turkishToolStripMenuItem.Tag = "TRK";
            turkishToolStripMenuItem.Text = "TRK - Turkish";
            turkishToolStripMenuItem.Click += TurkishToolStripMenuItem_Click;
            // 
            // bulgarianToolStripMenuItem
            // 
            bulgarianToolStripMenuItem.CheckOnClick = true;
            bulgarianToolStripMenuItem.Name = "bulgarianToolStripMenuItem";
            bulgarianToolStripMenuItem.Size = new Size(180, 22);
            bulgarianToolStripMenuItem.Tag = "BGR";
            bulgarianToolStripMenuItem.Text = "BGR - Bulgarian";
            bulgarianToolStripMenuItem.Click += BulgarianToolStripMenuItem_Click;
            // 
            // vietnameseToolStripMenuItem
            // 
            vietnameseToolStripMenuItem.CheckOnClick = true;
            vietnameseToolStripMenuItem.Name = "vietnameseToolStripMenuItem";
            vietnameseToolStripMenuItem.Size = new Size(180, 22);
            vietnameseToolStripMenuItem.Tag = "VNM";
            vietnameseToolStripMenuItem.Text = "VNM - Vietnamese";
            vietnameseToolStripMenuItem.Click += VietnameseToolStripMenuItem_Click;
            // 
            // backgroundToolStripMenuItem
            // 
            backgroundToolStripMenuItem.Name = "backgroundToolStripMenuItem";
            backgroundToolStripMenuItem.Size = new Size(180, 22);
            backgroundToolStripMenuItem.Text = "Background";
            backgroundToolStripMenuItem.Click += BackgroundToolStripMenuItem_Click;
            // 
            // toolsToolStripMenuItem
            // 
            toolsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { eagleLoxMonitorToolStripMenuItem, wSSenderToolStripMenuItem });
            toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            toolsToolStripMenuItem.Size = new Size(47, 20);
            toolsToolStripMenuItem.Text = "Tools";
            // 
            // eagleLoxMonitorToolStripMenuItem
            // 
            eagleLoxMonitorToolStripMenuItem.Name = "eagleLoxMonitorToolStripMenuItem";
            eagleLoxMonitorToolStripMenuItem.Size = new Size(180, 22);
            eagleLoxMonitorToolStripMenuItem.Text = "EagleLoxMonitor";
            eagleLoxMonitorToolStripMenuItem.Click += EagleLoxMonitorToolStripMenuItem_Click;
            // 
            // wSSenderToolStripMenuItem
            // 
            wSSenderToolStripMenuItem.Name = "wSSenderToolStripMenuItem";
            wSSenderToolStripMenuItem.Size = new Size(180, 22);
            wSSenderToolStripMenuItem.Text = "WSSender";
            wSSenderToolStripMenuItem.Click += WSSenderToolStripMenuItem_Click;
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { aboutToolStripMenuItem, openGitHubPageToolStripMenuItem });
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new Size(44, 20);
            helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new Size(173, 22);
            aboutToolStripMenuItem.Text = "About";
            aboutToolStripMenuItem.Click += AboutToolStripMenuItem_Click;
            // 
            // openGitHubPageToolStripMenuItem
            // 
            openGitHubPageToolStripMenuItem.Name = "openGitHubPageToolStripMenuItem";
            openGitHubPageToolStripMenuItem.Size = new Size(173, 22);
            openGitHubPageToolStripMenuItem.Text = "Open GitHub page";
            openGitHubPageToolStripMenuItem.Click += OpenGitHubPageToolStripMenuItem_Click;
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
            // backgroundFileDialog
            // 
            backgroundFileDialog.DefaultExt = "png";
            backgroundFileDialog.FileName = "openFileDialog1";
            backgroundFileDialog.Filter = "PNG|*.png";
            // 
            // downloadLabel
            // 
            downloadLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            downloadLabel.AutoSize = true;
            downloadLabel.ForeColor = SystemColors.ControlLightLight;
            downloadLabel.Location = new Point(611, 342);
            downloadLabel.Name = "downloadLabel";
            downloadLabel.Size = new Size(0, 15);
            downloadLabel.TabIndex = 10;
            // 
            // timer1
            // 
            timer1.Tick += Timer1_Tick;
            // 
            // backgroundPanel
            // 
            backgroundPanel.BackgroundImage = Properties.Resources.LoxBg;
            backgroundPanel.BackgroundImageLayout = ImageLayout.Zoom;
            backgroundPanel.Location = new Point(0, 24);
            backgroundPanel.Name = "backgroundPanel";
            backgroundPanel.Size = new Size(804, 417);
            backgroundPanel.TabIndex = 11;
            // 
            // MainWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            BackgroundImageLayout = ImageLayout.Zoom;
            ClientSize = new Size(804, 441);
            Controls.Add(menuStrip1);
            Controls.Add(downloadLabel);
            Controls.Add(comboReleaseType);
            Controls.Add(labelLastVersion);
            Controls.Add(buttonUpdateCheck);
            Controls.Add(checkBoxAppDebug);
            Controls.Add(buttonStartApp);
            Controls.Add(buttonConfigRescan);
            Controls.Add(buttonConfigLaunch);
            Controls.Add(comboConfigVersion);
            Controls.Add(comboConfigLanguage);
            Controls.Add(backgroundPanel);
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
        private ToolStripMenuItem backgroundToolStripMenuItem;
        private OpenFileDialog backgroundFileDialog;
        private Label downloadLabel;
        private System.Windows.Forms.Timer timer1;
        private ToolStripMenuItem bulgarianToolStripMenuItem;
        private ToolStripMenuItem vietnameseToolStripMenuItem;
        private Panel backgroundPanel;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStripMenuItem wSSenderToolStripMenuItem;
        private ToolStripMenuItem openGitHubPageToolStripMenuItem;
    }
}