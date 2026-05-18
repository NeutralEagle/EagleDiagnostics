using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace EagleDiagnostics.DeflogParser
{
    public partial class DeflogAnalyzerForm : Form
    {
        private readonly List<IssueDefinition> _issueDefs =
        [
            new("SDC Errors", Severity.High, [@"\bSDC\b"], ""),
            new("Device Restart", Severity.Low, [@"start.*reason.*reset"], ""),
            new("Warning ###", Severity.Medium, [@"Warning.\d+"], ""),
            new("Critical ###", Severity.High, [@"Critical.\d+"], ""),
            new("No Internet Connection", Severity.Medium, [@"no\s+internet"], ""),
            new("Firewall Blocked Ports", Severity.Medium, [@"Firewall.*enabled"], ""),
            new("Update of MS", Severity.Medium, [@"PRG Start \d+"], ""),
        ];

        private string[] _lines = [];
        private readonly List<IssueFinding> _findings = [];

        private IssueFinding? _selectedFinding;
        private IssueFinding? _activeFilter;
        private int _currentMatchIndex = -1;

        public DeflogAnalyzerForm()
        {
            InitializeComponent();
        }

        private void BtnOpenFile_Click(object sender, EventArgs e)
        {
            if (ofd.ShowDialog() != DialogResult.OK) return;
            LoadLogText(File.ReadAllText(ofd.FileName));
        }

        private void BtnPaste_Click(object sender, EventArgs e)
        {
            LoadLogText(Clipboard.GetText());
        }

        private void BtnAnalyze_Click(object sender, EventArgs e)
        {
            Analyze();
            PopulateIssuesList();
            ApplyHighlights();
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            rtbLog.Clear();
            lvIssues.Items.Clear();
            _lines = [];
            _findings.Clear();
        }

        private void LoadLogText(string text)
        {
            text = text.Replace("\r\n", "\n").Replace("\r", "\n");
            rtbLog.Text = text;
            _lines = text.Split('\n');
        }

        private void Analyze()
        {
            _findings.Clear();
            var defs = _issueDefs.Select(d => d.WithCompiledRegex()).ToList();

            for (int i = 0; i < _lines.Length; i++)
            {
                var line = _lines[i];

                foreach (var def in defs)
                {
                    foreach (var rx in def.CompiledPatterns!)
                    {
                        foreach (Match m in rx.Matches(line))
                        {
                            var finding = _findings.FirstOrDefault(f => f.Definition.Key == def.Key);
                            if (finding == null)
                            {
                                finding = new IssueFinding(def);
                                _findings.Add(finding);
                            }

                            finding.Matches.Add(new IssueMatch(i, line, m.Index, m.Length));
                        }
                    }
                }
            }
        }

        private void PopulateIssuesList()
        {
            lvIssues.Items.Clear();

            var src = _activeFilter != null ? _findings.Where(f => f == _activeFilter) : _findings;

            foreach (var f in src)
            {
                var item = new ListViewItem(f.Definition.Severity.ToString());
                item.SubItems.Add(f.Definition.Key);
                item.SubItems.Add(f.Matches.Count.ToString());
                item.SubItems.Add((f.Matches.FirstOrDefault().LineIndex + 1).ToString());
                item.SubItems.Add(f.Matches.FirstOrDefault().LineText ?? "");
                item.Tag = f;
                lvIssues.Items.Add(item);
            }
        }

        private void LvIssues_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvIssues.SelectedItems.Count == 0) return;

            _selectedFinding = (IssueFinding)(lvIssues.SelectedItems[0].Tag??"");
            _activeFilter = _selectedFinding;
            _currentMatchIndex = 0;

            ApplyHighlights();
            GoToMatch(0);
        }

        private void GoToMatch(int index)
        {
            if (_selectedFinding == null || _selectedFinding.Matches.Count == 0) return;

            if (index < 0) index = _selectedFinding.Matches.Count - 1;
            if (index >= _selectedFinding.Matches.Count) index = 0;

            _currentMatchIndex = index;

            var m = _selectedFinding.Matches[index];
            int start = rtbLog.GetFirstCharIndexFromLine(m.LineIndex) + m.MatchIndexInLine;

            rtbLog.Select(start, m.MatchLength);
            rtbLog.ScrollToCaret();

            lblStatus.Text = $"{_selectedFinding.Definition.Key} {index + 1}/{_selectedFinding.Matches.Count}";
        }

        private void NextMatch() => GoToMatch(_currentMatchIndex + 1);
        private void PrevMatch() => GoToMatch(_currentMatchIndex - 1);

        private void ClearFilter()
        {
            _activeFilter = null;
            _selectedFinding = null;
            ApplyHighlights();
        }

        private void ApplyHighlights()
        {
            rtbLog.SelectAll();
            rtbLog.SelectionBackColor = Color.White;
            IEnumerable<IssueFinding> src;
            if (_activeFilter != null)
                src = _findings.Where(f => f == _activeFilter);
            else
                src = _findings;
            

            foreach (var f in src)
            {
                var color = f == _selectedFinding ? Color.Orange : Color.LightYellow;

                foreach (var m in f.Matches)
                {
                    int start = rtbLog.GetFirstCharIndexFromLine(m.LineIndex) + m.MatchIndexInLine;
                    rtbLog.Select(start, m.MatchLength);
                    rtbLog.SelectionBackColor = color;
                }
            }
        }

        private class IssueDefinition(string key, Severity severity, string[] patterns, string desc)
        {
            public string Key = key;
            public Severity Severity = severity;
            public string[] Patterns = patterns;
            public Regex[]? CompiledPatterns;

            public IssueDefinition WithCompiledRegex()
            {
                CompiledPatterns = [.. Patterns.Select(p => new Regex(p, RegexOptions.IgnoreCase))];
                return this;
            }
        }

        private class IssueFinding(IssueDefinition def)
        {
            public IssueDefinition Definition = def;
            public List<IssueMatch> Matches = [];
        }

        private struct IssueMatch(int line, string text, int idx, int len)
        {
            public int LineIndex = line;
            public string LineText = text;
            public int MatchIndexInLine = idx;
            public int MatchLength = len;
        }

        private enum Severity { Low, Medium, High }
    }
}