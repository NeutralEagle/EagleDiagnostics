using System.Windows.Forms;
using ZedGraph;

namespace EagleDiagnostics
{
    partial class NFCPlot
    {
        private System.ComponentModel.IContainer components = null;

        private TextBox txtIP;
        private TextBox txtSerialNumber;
        private CheckBox chkUseDns;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private TextBox txtDevice;
        private TextBox txtResolvedUrl;

        private NumericUpDown nudIntervalMs;
        private NumericUpDown nudIterations;
        private NumericUpDown nudMaxSamples;

        private Button btnStart;
        private Button btnStop;
        private Button btnReadOnce;

        private ListBox lstLog;
        private ZedGraphControl zedGraphControl1;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            txtIP = new TextBox();
            txtSerialNumber = new TextBox();
            chkUseDns = new CheckBox();
            txtUsername = new TextBox();
            txtPassword = new TextBox();
            txtDevice = new TextBox();
            txtResolvedUrl = new TextBox();
            nudIntervalMs = new NumericUpDown();
            nudIterations = new NumericUpDown();
            nudMaxSamples = new NumericUpDown();
            btnStart = new Button();
            btnStop = new Button();
            btnReadOnce = new Button();
            lstLog = new ListBox();
            zedGraphControl1 = new ZedGraphControl();
            lblIP = new System.Windows.Forms.Label();
            lblSN = new System.Windows.Forms.Label();
            lblUser = new System.Windows.Forms.Label();
            lblPass = new System.Windows.Forms.Label();
            lblDev = new System.Windows.Forms.Label();
            lblUrl = new System.Windows.Forms.Label();
            lblInterval = new System.Windows.Forms.Label();
            lblIterations = new System.Windows.Forms.Label();
            lblMaxSamples = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)nudIntervalMs).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudIterations).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudMaxSamples).BeginInit();
            SuspendLayout();
            // 
            // txtIP
            // 
            txtIP.Location = new Point(120, 8);
            txtIP.Name = "txtIP";
            txtIP.Size = new Size(380, 23);
            txtIP.TabIndex = 6;
            // 
            // txtSerialNumber
            // 
            txtSerialNumber.Location = new Point(120, 38);
            txtSerialNumber.Name = "txtSerialNumber";
            txtSerialNumber.Size = new Size(250, 23);
            txtSerialNumber.TabIndex = 7;
            // 
            // chkUseDns
            // 
            chkUseDns.Location = new Point(380, 40);
            chkUseDns.Name = "chkUseDns";
            chkUseDns.Size = new Size(120, 24);
            chkUseDns.TabIndex = 8;
            chkUseDns.Text = "Use DNS";
            // 
            // txtUsername
            // 
            txtUsername.Location = new Point(120, 68);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(380, 23);
            txtUsername.TabIndex = 9;
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(120, 98);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(380, 23);
            txtPassword.TabIndex = 10;
            txtPassword.UseSystemPasswordChar = true;
            // 
            // txtDevice
            // 
            txtDevice.Location = new Point(120, 128);
            txtDevice.Name = "txtDevice";
            txtDevice.Size = new Size(380, 23);
            txtDevice.TabIndex = 11;
            // 
            // txtResolvedUrl
            // 
            txtResolvedUrl.Location = new Point(120, 158);
            txtResolvedUrl.Name = "txtResolvedUrl";
            txtResolvedUrl.ReadOnly = true;
            txtResolvedUrl.Size = new Size(740, 23);
            txtResolvedUrl.TabIndex = 12;
            // 
            // nudIntervalMs
            // 
            nudIntervalMs.Location = new Point(650, 8);
            nudIntervalMs.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            nudIntervalMs.Minimum = new decimal(new int[] { 50, 0, 0, 0 });
            nudIntervalMs.Name = "nudIntervalMs";
            nudIntervalMs.Size = new Size(90, 23);
            nudIntervalMs.TabIndex = 16;
            nudIntervalMs.Value = new decimal(new int[] { 250, 0, 0, 0 });
            // 
            // nudIterations
            // 
            nudIterations.Location = new Point(650, 38);
            nudIterations.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            nudIterations.Name = "nudIterations";
            nudIterations.Size = new Size(90, 23);
            nudIterations.TabIndex = 17;
            nudIterations.Value = new decimal(new int[] { 500, 0, 0, 0 });
            // 
            // nudMaxSamples
            // 
            nudMaxSamples.Location = new Point(650, 68);
            nudMaxSamples.Maximum = new decimal(new int[] { 20000, 0, 0, 0 });
            nudMaxSamples.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            nudMaxSamples.Name = "nudMaxSamples";
            nudMaxSamples.Size = new Size(90, 23);
            nudMaxSamples.TabIndex = 18;
            nudMaxSamples.Value = new decimal(new int[] { 500, 0, 0, 0 });
            // 
            // btnStart
            // 
            btnStart.Location = new Point(770, 8);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(90, 23);
            btnStart.TabIndex = 19;
            btnStart.Text = "Start";
            btnStart.Click += BtnStart_Click;
            // 
            // btnStop
            // 
            btnStop.Location = new Point(770, 38);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(90, 23);
            btnStop.TabIndex = 20;
            btnStop.Text = "Stop";
            btnStop.Click += BtnStop_Click;
            // 
            // btnReadOnce
            // 
            btnReadOnce.Location = new Point(770, 68);
            btnReadOnce.Name = "btnReadOnce";
            btnReadOnce.Size = new Size(90, 23);
            btnReadOnce.TabIndex = 21;
            btnReadOnce.Text = "Read Once";
            btnReadOnce.Click += BtnReadOnce_Click;
            // 
            // lstLog
            // 
            lstLog.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lstLog.Location = new Point(12, 625);
            lstLog.Name = "lstLog";
            lstLog.Size = new Size(850, 139);
            lstLog.TabIndex = 23;
            // 
            // zedGraphControl1
            // 
            zedGraphControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            zedGraphControl1.Location = new Point(12, 195);
            zedGraphControl1.Margin = new Padding(4, 3, 4, 3);
            zedGraphControl1.Name = "zedGraphControl1";
            zedGraphControl1.ScrollGrace = 0D;
            zedGraphControl1.ScrollMaxX = 0D;
            zedGraphControl1.ScrollMaxY = 0D;
            zedGraphControl1.ScrollMaxY2 = 0D;
            zedGraphControl1.ScrollMinX = 0D;
            zedGraphControl1.ScrollMinY = 0D;
            zedGraphControl1.ScrollMinY2 = 0D;
            zedGraphControl1.Size = new Size(850, 420);
            zedGraphControl1.TabIndex = 22;
            zedGraphControl1.UseExtendedPrintDialog = true;
            // 
            // lblIP
            // 
            lblIP.AutoSize = true;
            lblIP.Location = new Point(12, 12);
            lblIP.Name = "lblIP";
            lblIP.Size = new Size(78, 15);
            lblIP.TabIndex = 0;
            lblIP.Text = "Miniserver IP:";
            // 
            // lblSN
            // 
            lblSN.AutoSize = true;
            lblSN.Location = new Point(12, 42);
            lblSN.Name = "lblSN";
            lblSN.Size = new Size(83, 15);
            lblSN.TabIndex = 1;
            lblSN.Text = "Miniserver SN:";
            // 
            // lblUser
            // 
            lblUser.AutoSize = true;
            lblUser.Location = new Point(12, 72);
            lblUser.Name = "lblUser";
            lblUser.Size = new Size(63, 15);
            lblUser.TabIndex = 2;
            lblUser.Text = "Username:";
            // 
            // lblPass
            // 
            lblPass.AutoSize = true;
            lblPass.Location = new Point(12, 102);
            lblPass.Name = "lblPass";
            lblPass.Size = new Size(60, 15);
            lblPass.TabIndex = 3;
            lblPass.Text = "Password:";
            // 
            // lblDev
            // 
            lblDev.AutoSize = true;
            lblDev.Location = new Point(12, 132);
            lblDev.Name = "lblDev";
            lblDev.Size = new Size(45, 15);
            lblDev.TabIndex = 4;
            lblDev.Text = "Device:";
            // 
            // lblUrl
            // 
            lblUrl.AutoSize = true;
            lblUrl.Location = new Point(12, 162);
            lblUrl.Name = "lblUrl";
            lblUrl.Size = new Size(81, 15);
            lblUrl.TabIndex = 5;
            lblUrl.Text = "Resolved URL:";
            // 
            // lblInterval
            // 
            lblInterval.AutoSize = true;
            lblInterval.Location = new Point(520, 12);
            lblInterval.Name = "lblInterval";
            lblInterval.Size = new Size(76, 15);
            lblInterval.TabIndex = 13;
            lblInterval.Text = "Interval (ms):";
            // 
            // lblIterations
            // 
            lblIterations.AutoSize = true;
            lblIterations.Location = new Point(520, 42);
            lblIterations.Name = "lblIterations";
            lblIterations.Size = new Size(94, 15);
            lblIterations.TabIndex = 14;
            lblIterations.Text = "Iterations (0=∞):";
            // 
            // lblMaxSamples
            // 
            lblMaxSamples.AutoSize = true;
            lblMaxSamples.Location = new Point(520, 72);
            lblMaxSamples.Name = "lblMaxSamples";
            lblMaxSamples.Size = new Size(78, 15);
            lblMaxSamples.TabIndex = 15;
            lblMaxSamples.Text = "Max samples:";
            // 
            // NFCPlot
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(874, 780);
            Controls.Add(lblIP);
            Controls.Add(lblSN);
            Controls.Add(lblUser);
            Controls.Add(lblPass);
            Controls.Add(lblDev);
            Controls.Add(lblUrl);
            Controls.Add(txtIP);
            Controls.Add(txtSerialNumber);
            Controls.Add(chkUseDns);
            Controls.Add(txtUsername);
            Controls.Add(txtPassword);
            Controls.Add(txtDevice);
            Controls.Add(txtResolvedUrl);
            Controls.Add(lblInterval);
            Controls.Add(lblIterations);
            Controls.Add(lblMaxSamples);
            Controls.Add(nudIntervalMs);
            Controls.Add(nudIterations);
            Controls.Add(nudMaxSamples);
            Controls.Add(btnStart);
            Controls.Add(btnStop);
            Controls.Add(btnReadOnce);
            Controls.Add(zedGraphControl1);
            Controls.Add(lstLog);
            Name = "NFCPlot";
            Text = "NFC Plot (CS - LTA)";
            FormClosing += NFCPlot_FormClosing;
            ((System.ComponentModel.ISupportInitialize)nudIntervalMs).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudIterations).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudMaxSamples).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.Label lblIP;
        private System.Windows.Forms.Label lblSN;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.Label lblPass;
        private System.Windows.Forms.Label lblDev;
        private System.Windows.Forms.Label lblUrl;
        private System.Windows.Forms.Label lblInterval;
        private System.Windows.Forms.Label lblIterations;
        private System.Windows.Forms.Label lblMaxSamples;
    }
}
