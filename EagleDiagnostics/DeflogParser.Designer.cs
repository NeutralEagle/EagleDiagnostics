namespace EagleDiagnostics.DeflogParser
{
    partial class DeflogAnalyzerForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            pnlTop = new System.Windows.Forms.Panel();
            btnOpenFile = new System.Windows.Forms.Button();
            btnPaste = new System.Windows.Forms.Button();
            btnAnalyze = new System.Windows.Forms.Button();
            btnClear = new System.Windows.Forms.Button();
            btnPrev = new System.Windows.Forms.Button();
            btnNext = new System.Windows.Forms.Button();
            btnClearFilter = new System.Windows.Forms.Button();
            lblStatus = new System.Windows.Forms.Label();
            splitMain = new System.Windows.Forms.SplitContainer();
            grpIssues = new System.Windows.Forms.GroupBox();
            lvIssues = new System.Windows.Forms.ListView();
            colSeverity = new System.Windows.Forms.ColumnHeader();
            colType = new System.Windows.Forms.ColumnHeader();
            colCount = new System.Windows.Forms.ColumnHeader();
            colFirstLine = new System.Windows.Forms.ColumnHeader();
            colExample = new System.Windows.Forms.ColumnHeader();
            grpLog = new System.Windows.Forms.GroupBox();
            rtbLog = new System.Windows.Forms.RichTextBox();
            ofd = new System.Windows.Forms.OpenFileDialog();

            pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitMain).BeginInit();
            splitMain.Panel1.SuspendLayout();
            splitMain.Panel2.SuspendLayout();
            splitMain.SuspendLayout();
            grpIssues.SuspendLayout();
            grpLog.SuspendLayout();
            SuspendLayout();

            // Top panel
            pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            pnlTop.Height = 50;
            pnlTop.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                btnOpenFile, btnPaste, btnAnalyze, btnClear,
                btnPrev, btnNext, btnClearFilter, lblStatus
            });

            btnOpenFile.Text = "Open";
            btnOpenFile.Location = new System.Drawing.Point(10, 10);
            btnOpenFile.Click += BtnOpenFile_Click;

            btnPaste.Text = "Paste";
            btnPaste.Location = new System.Drawing.Point(90, 10);
            btnPaste.Click += BtnPaste_Click;

            btnAnalyze.Text = "Analyze";
            btnAnalyze.Location = new System.Drawing.Point(170, 10);
            btnAnalyze.Click += BtnAnalyze_Click;

            btnClear.Text = "Clear";
            btnClear.Location = new System.Drawing.Point(260, 10);
            btnClear.Click += BtnClear_Click;

            btnPrev.Text = "Prev";
            btnPrev.Location = new System.Drawing.Point(340, 10);
            btnPrev.Click += (s, e) => PrevMatch();

            btnNext.Text = "Next";
            btnNext.Location = new System.Drawing.Point(410, 10);
            btnNext.Click += (s, e) => NextMatch();

            btnClearFilter.Text = "Clear Filter";
            btnClearFilter.Location = new System.Drawing.Point(480, 10);
            btnClearFilter.Click += (s, e) => ClearFilter();

            lblStatus.Location = new System.Drawing.Point(600, 14);
            lblStatus.AutoSize = true;
            lblStatus.Text = "Ready";

            // Split
            splitMain.Dock = System.Windows.Forms.DockStyle.Fill;

            // Left panel
            grpIssues.Dock = System.Windows.Forms.DockStyle.Fill;
            grpIssues.Text = "Issues";
            grpIssues.Controls.Add(lvIssues);

            lvIssues.Dock = System.Windows.Forms.DockStyle.Fill;
            lvIssues.View = System.Windows.Forms.View.Details;
            lvIssues.FullRowSelect = true;
            lvIssues.Columns.AddRange(new[]
            {
                colSeverity, colType, colCount, colFirstLine, colExample
            });
            lvIssues.SelectedIndexChanged += LvIssues_SelectedIndexChanged;

            colSeverity.Text = "Severity";
            colType.Text = "Type";
            colCount.Text = "Count";
            colFirstLine.Text = "Line";
            colExample.Text = "Example";

            // Right panel
            grpLog.Dock = System.Windows.Forms.DockStyle.Fill;
            grpLog.Text = "Log";
            grpLog.Controls.Add(rtbLog);

            rtbLog.Dock = System.Windows.Forms.DockStyle.Fill;
            rtbLog.Font = new System.Drawing.Font("Consolas", 9F);
            rtbLog.WordWrap = false;

            splitMain.Panel1.Controls.Add(grpIssues);
            splitMain.Panel2.Controls.Add(grpLog);

            // Form
            Controls.Add(splitMain);
            Controls.Add(pnlTop);
            Text = "Deflog Parser";
            WindowState = System.Windows.Forms.FormWindowState.Maximized;

            pnlTop.ResumeLayout(false);
            pnlTop.PerformLayout();
            splitMain.Panel1.ResumeLayout(false);
            splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitMain).EndInit();
            splitMain.ResumeLayout(false);
            grpIssues.ResumeLayout(false);
            grpLog.ResumeLayout(false);
            ResumeLayout(false);
        }

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Button btnOpenFile, btnPaste, btnAnalyze, btnClear;
        private System.Windows.Forms.Button btnPrev, btnNext, btnClearFilter;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.GroupBox grpIssues, grpLog;
        private System.Windows.Forms.ListView lvIssues;
        private System.Windows.Forms.ColumnHeader colSeverity, colType, colCount, colFirstLine, colExample;
        private System.Windows.Forms.RichTextBox rtbLog;
        private System.Windows.Forms.OpenFileDialog ofd;
    }
}