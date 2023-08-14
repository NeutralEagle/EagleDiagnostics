using System;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace EagleDiagnostics
{
    partial class EagleLoxMonitor
    {
        /// <summary>
        /// Vyžaduje se proměnná návrháře.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Uvolněte všechny používané prostředky.
        /// </summary>
        /// <param name="disposing">hodnota true, když by se měl spravovaný prostředek odstranit; jinak false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }


        #region Kód generovaný Návrhářem Windows Form

        /// <summary>
        /// Metoda vyžadovaná pro podporu Návrháře - neupravovat
        /// obsah této metody v editoru kódu.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EagleLoxMonitor));
            mainListBox = new ListBox();
            autoSaveTimer = new System.Windows.Forms.Timer(components);
            checkBox1 = new CheckBox();
            saveButton = new Button();
            saveFileDialog1 = new SaveFileDialog();
            loadButton = new Button();
            openFileDialog1 = new OpenFileDialog();
            receiveButton = new Button();
            ppsLabel = new Label();
            ppsTimer = new System.Windows.Forms.Timer(components);
            ppsTextLabel = new Label();
            totalPacketLabel = new Label();
            totalTextLabel = new Label();
            mainProgressBar = new ProgressBar();
            SuspendLayout();
            // 
            // mainListBox
            // 
            mainListBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            mainListBox.Font = new Font("Monospac821 BT", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            mainListBox.FormattingEnabled = true;
            mainListBox.ItemHeight = 14;
            mainListBox.Location = new Point(1, 47);
            mainListBox.Margin = new Padding(4, 3, 4, 23);
            mainListBox.Name = "mainListBox";
            mainListBox.ScrollAlwaysVisible = true;
            mainListBox.SelectionMode = SelectionMode.MultiExtended;
            mainListBox.Size = new Size(935, 466);
            mainListBox.TabIndex = 0;
            // 
            // autoSaveTimer
            // 
            autoSaveTimer.Tick += AutoSaveTimer_Tick;
            // 
            // checkBox1
            // 
            checkBox1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            checkBox1.AutoSize = true;
            checkBox1.Checked = true;
            checkBox1.CheckState = CheckState.Checked;
            checkBox1.Location = new Point(750, 18);
            checkBox1.Margin = new Padding(4, 3, 4, 3);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(80, 19);
            checkBox1.TabIndex = 1;
            checkBox1.Text = "Autoscroll";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += CheckBox1_CheckedChanged;
            // 
            // saveButton
            // 
            saveButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            saveButton.Location = new Point(836, 14);
            saveButton.Margin = new Padding(4, 3, 4, 3);
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(88, 27);
            saveButton.TabIndex = 2;
            saveButton.Text = "Save";
            saveButton.UseVisualStyleBackColor = true;
            saveButton.Click += SaveButton_Click;
            // 
            // saveFileDialog1
            // 
            saveFileDialog1.DefaultExt = "txt";
            saveFileDialog1.Filter = "|*.txt";
            // 
            // loadButton
            // 
            loadButton.Location = new Point(14, 14);
            loadButton.Margin = new Padding(4, 3, 4, 3);
            loadButton.Name = "loadButton";
            loadButton.Size = new Size(88, 27);
            loadButton.TabIndex = 3;
            loadButton.Text = "Load";
            loadButton.UseVisualStyleBackColor = true;
            loadButton.Click += LoadButton_Click;
            // 
            // openFileDialog1
            // 
            openFileDialog1.Filter = "Loxone Monitor or Txt|*.LxMon;*.txt|Loxone Monitor|*.LxMon|Text File|*.txt";
            // 
            // receiveButton
            // 
            receiveButton.Enabled = false;
            receiveButton.Location = new Point(108, 14);
            receiveButton.Margin = new Padding(4, 3, 4, 3);
            receiveButton.Name = "receiveButton";
            receiveButton.Size = new Size(88, 27);
            receiveButton.TabIndex = 4;
            receiveButton.Text = "Receive";
            receiveButton.UseVisualStyleBackColor = true;
            receiveButton.Click += ReceiveButton_Click;
            // 
            // ppsLabel
            // 
            ppsLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            ppsLabel.AutoSize = true;
            ppsLabel.Location = new Point(401, 524);
            ppsLabel.Margin = new Padding(4, 0, 4, 0);
            ppsLabel.Name = "ppsLabel";
            ppsLabel.Size = new Size(13, 15);
            ppsLabel.TabIndex = 5;
            ppsLabel.Text = "0";
            // 
            // ppsTimer
            // 
            ppsTimer.Tick += PpsTimer_Tick;
            // 
            // ppsTextLabel
            // 
            ppsTextLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            ppsTextLabel.AutoSize = true;
            ppsTextLabel.Location = new Point(243, 524);
            ppsTextLabel.Margin = new Padding(4, 0, 4, 0);
            ppsTextLabel.Name = "ppsTextLabel";
            ppsTextLabel.Size = new Size(138, 15);
            ppsTextLabel.TabIndex = 6;
            ppsTextLabel.Text = "Avg. Packets per second:";
            ppsTextLabel.TextAlign = ContentAlignment.TopRight;
            // 
            // totalPacketLabel
            // 
            totalPacketLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            totalPacketLabel.AutoSize = true;
            totalPacketLabel.Location = new Point(163, 524);
            totalPacketLabel.Margin = new Padding(4, 0, 4, 0);
            totalPacketLabel.Name = "totalPacketLabel";
            totalPacketLabel.Size = new Size(13, 15);
            totalPacketLabel.TabIndex = 7;
            totalPacketLabel.Text = "0";
            // 
            // totalTextLabel
            // 
            totalTextLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            totalTextLabel.AutoSize = true;
            totalTextLabel.Location = new Point(14, 524);
            totalTextLabel.Margin = new Padding(4, 0, 4, 0);
            totalTextLabel.Name = "totalTextLabel";
            totalTextLabel.Size = new Size(128, 15);
            totalTextLabel.TabIndex = 8;
            totalTextLabel.Text = "Total packets received: ";
            // 
            // mainProgressBar
            // 
            mainProgressBar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            mainProgressBar.Location = new Point(684, 518);
            mainProgressBar.Name = "mainProgressBar";
            mainProgressBar.Size = new Size(249, 23);
            mainProgressBar.Step = 1;
            mainProgressBar.Style = ProgressBarStyle.Continuous;
            mainProgressBar.TabIndex = 9;
            // 
            // EagleLoxMonitor
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(938, 545);
            Controls.Add(mainProgressBar);
            Controls.Add(totalTextLabel);
            Controls.Add(totalPacketLabel);
            Controls.Add(ppsTextLabel);
            Controls.Add(ppsLabel);
            Controls.Add(receiveButton);
            Controls.Add(loadButton);
            Controls.Add(saveButton);
            Controls.Add(checkBox1);
            Controls.Add(mainListBox);
            Icon = (Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            Margin = new Padding(4, 3, 4, 3);
            MinimumSize = new Size(464, 225);
            Name = "EagleLoxMonitor";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "EagleLoxMonitor";
            FormClosing += EagleLoxMonitor_FormClosing;
            KeyDown += Form1_KeyDown;
            ResumeLayout(false);
            PerformLayout();
        }




        #endregion

        private ListBox mainListBox;
        private System.Windows.Forms.Timer autoSaveTimer;
        private CheckBox checkBox1;
        private Button saveButton;
        private SaveFileDialog saveFileDialog1;
        private Button loadButton;
        private OpenFileDialog openFileDialog1;
        private Button receiveButton;
        private Label ppsLabel;
        private System.Windows.Forms.Timer ppsTimer;
        private Label ppsTextLabel;
        private Label totalPacketLabel;
        private Label totalTextLabel;
        private ProgressBar mainProgressBar;
    }
}