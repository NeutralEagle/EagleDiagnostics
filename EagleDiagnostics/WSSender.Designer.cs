namespace EagleDiagnostics
{
    partial class WSSender
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            msTextBox = new TextBox();
            userTextBox = new TextBox();
            pwTextBox = new TextBox();
            deviceTextBox = new TextBox();
            btnGetvar = new Button();
            rBSet = new RadioButton();
            rBStore = new RadioButton();
            rBErase = new RadioButton();
            btnReboot = new Button();
            btnSend = new Button();
            panel = new Panel();
            logger = new RichTextBox();
            SuspendLayout();
            // 
            // msTextBox
            // 
            msTextBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            msTextBox.Location = new Point(12, 826);
            msTextBox.Name = "msTextBox";
            msTextBox.Size = new Size(100, 23);
            msTextBox.TabIndex = 0;
            msTextBox.Text = "MS_SN";
            // 
            // userTextBox
            // 
            userTextBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            userTextBox.Location = new Point(118, 826);
            userTextBox.Name = "userTextBox";
            userTextBox.Size = new Size(100, 23);
            userTextBox.TabIndex = 1;
            userTextBox.Text = "username";
            // 
            // pwTextBox
            // 
            pwTextBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            pwTextBox.Location = new Point(224, 826);
            pwTextBox.Name = "pwTextBox";
            pwTextBox.Size = new Size(100, 23);
            pwTextBox.TabIndex = 2;
            pwTextBox.Text = "password";
            pwTextBox.UseSystemPasswordChar = true;
            // 
            // deviceTextBox
            // 
            deviceTextBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            deviceTextBox.Location = new Point(330, 826);
            deviceTextBox.Name = "deviceTextBox";
            deviceTextBox.Size = new Size(100, 23);
            deviceTextBox.TabIndex = 3;
            deviceTextBox.Text = "deviceSN";
            // 
            // btnGetvar
            // 
            btnGetvar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnGetvar.Location = new Point(436, 825);
            btnGetvar.Name = "btnGetvar";
            btnGetvar.Size = new Size(75, 23);
            btnGetvar.TabIndex = 4;
            btnGetvar.Text = "getvar";
            btnGetvar.UseVisualStyleBackColor = true;
            btnGetvar.Click += button1_Click;
            // 
            // rBSet
            // 
            rBSet.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            rBSet.AutoSize = true;
            rBSet.Location = new Point(434, 14);
            rBSet.Name = "rBSet";
            rBSet.Size = new Size(41, 19);
            rBSet.TabIndex = 5;
            rBSet.TabStop = true;
            rBSet.Text = "Set";
            rBSet.UseVisualStyleBackColor = true;
            // 
            // rBStore
            // 
            rBStore.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            rBStore.AutoSize = true;
            rBStore.Location = new Point(481, 14);
            rBStore.Name = "rBStore";
            rBStore.Size = new Size(52, 19);
            rBStore.TabIndex = 6;
            rBStore.TabStop = true;
            rBStore.Text = "Store";
            rBStore.UseVisualStyleBackColor = true;
            // 
            // rBErase
            // 
            rBErase.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            rBErase.AutoSize = true;
            rBErase.Location = new Point(539, 14);
            rBErase.Name = "rBErase";
            rBErase.Size = new Size(52, 19);
            rBErase.TabIndex = 7;
            rBErase.TabStop = true;
            rBErase.Text = "Erase";
            rBErase.UseVisualStyleBackColor = true;
            // 
            // btnReboot
            // 
            btnReboot.Location = new Point(12, 12);
            btnReboot.Name = "btnReboot";
            btnReboot.Size = new Size(93, 23);
            btnReboot.TabIndex = 8;
            btnReboot.Text = "Device Reboot";
            btnReboot.UseVisualStyleBackColor = true;
            btnReboot.Click += btnReboot_Click;
            // 
            // btnSend
            // 
            btnSend.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSend.Location = new Point(597, 12);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(75, 23);
            btnSend.TabIndex = 9;
            btnSend.Text = "Send";
            btnSend.UseVisualStyleBackColor = true;
            btnSend.Click += BtnSend_Click;
            // 
            // panel
            // 
            panel.Location = new Point(12, 41);
            panel.Name = "panel";
            panel.Size = new Size(660, 779);
            panel.TabIndex = 10;
            // 
            // logger
            // 
            logger.Location = new Point(678, 12);
            logger.Name = "logger";
            logger.ReadOnly = true;
            logger.Size = new Size(694, 837);
            logger.TabIndex = 11;
            logger.Text = "";
            // 
            // WSSender
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1384, 861);
            Controls.Add(logger);
            Controls.Add(panel);
            Controls.Add(btnSend);
            Controls.Add(btnReboot);
            Controls.Add(rBErase);
            Controls.Add(rBStore);
            Controls.Add(rBSet);
            Controls.Add(btnGetvar);
            Controls.Add(deviceTextBox);
            Controls.Add(pwTextBox);
            Controls.Add(userTextBox);
            Controls.Add(msTextBox);
            MinimumSize = new Size(700, 900);
            Name = "WSSender";
            Text = "WSSender";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox msTextBox;
        private TextBox userTextBox;
        private TextBox pwTextBox;
        private TextBox deviceTextBox;
        private Button btnGetvar;
        private RadioButton rBSet;
        private RadioButton rBStore;
        private RadioButton rBErase;
        private Button btnReboot;
        private Button btnSend;
        private Panel panel;
        private RichTextBox logger;
    }
}