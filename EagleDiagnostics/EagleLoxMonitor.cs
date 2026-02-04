using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace EagleDiagnostics
{
    public partial class EagleLoxMonitor : Form
    {
        private const int Port = 7777;

        private volatile bool refreshFlag = true;
        private volatile bool loadFlag = false;
        private volatile bool formClosing = false;

        private volatile bool UDPActive = false;
        private UdpClient? udpClient;

        private readonly string autoSaveFolder = Directory.GetCurrentDirectory() + "\\autosave\\";
        private readonly string autoSaveFile = "autosave";
        private readonly string autoSaveExtension = ".txt";
        private int autoSaveID;

        private int packetsTotal = 0;
        private int packetsThisSecond = 0;

        private static readonly byte[] LxMonStart = [0x1F, 0xFA];
        private static readonly byte[] LxMonEnd = [0x1F, 0x1F];

        // NOTE: Your regex expects embedded control markers in the log payload
        private readonly string regexPattern = @"\0\u001f.*?\0\0\u0001";

        private readonly Thread UDPthread;

        // --- Filter helpers ---
        private sealed record FilterTerm(string Text, bool Negated);

        public EagleLoxMonitor()
        {
            InitializeComponent();

            // Allow selecting multiple files to merge
            openFileDialog1.Multiselect = true;

            autoSaveTimer.Interval = 300000; // 5 min
            autoSaveTimer.Start();

            ppsTimer.Interval = 1000;
            ppsTimer.Start();

            Show();

            UDPthread = new Thread(UDP_setup)
            {
                IsBackground = true,
                Name = "UDP Receiver"
            };

            try
            {
                UDPthread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // =========================
        // UDP
        // =========================
        public void UDP_setup()
        {
            if (UDPActive) return;

            try
            {
                udpClient = new UdpClient(Port);
                udpClient.Client.ReceiveTimeout = 500; // allows clean shutdown
                UDPActive = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while opening UDP socket:\n" + ex.Message);
                Invoke((MethodInvoker)(() => receiveButton.Enabled = true));
                UDPActive = false;
                return;
            }

            IPEndPoint endpoint = new(IPAddress.Any, Port);

            while (!formClosing)
            {
                if (loadFlag)
                {
                    Thread.Sleep(50);
                    continue;
                }

                try
                {
                    if (udpClient is null) break;

                    byte[] pkt = udpClient.Receive(ref endpoint);
                    packetsTotal++;
                    packetsThisSecond++;

                    if (TryParseMessageLine(pkt, recv: true, out string? line) && line is not null)
                    {
                        BeginInvoke((MethodInvoker)(() =>
                        {
                            mainListBox.Items.Add(line);
                            if (refreshFlag) mainListBox.TopIndex = mainListBox.Items.Count - 1;
                        }));
                    }
                }
                catch (SocketException se) when (se.SocketErrorCode == SocketError.TimedOut)
                {
                    // expected
                }
                catch (ObjectDisposedException)
                {
                    break;
                }
                catch
                {
                    // optionally log
                }
            }

            UDPActive = false;
        }

        // =========================
        // KMP helpers
        // =========================
        static int[] CalculatePrefixTable(byte[] pattern)
        {
            int[] prefixTable = new int[pattern.Length];
            int length = 0;
            int i = 1;

            while (i < pattern.Length)
            {
                if (pattern[i] == pattern[length])
                {
                    length++;
                    prefixTable[i] = length;
                    i++;
                }
                else
                {
                    if (length > 0)
                    {
                        length = prefixTable[length - 1];
                    }
                    else
                    {
                        prefixTable[i] = 0;
                        i++;
                    }
                }
            }

            return prefixTable;
        }

        static int FindPattern(byte[] text, byte[] pattern, int startByte)
        {
            int[] prefixTable = CalculatePrefixTable(pattern);
            int i = startByte; // index for text
            int j = 0; // index for pattern

            while (i < text.Length)
            {
                if (pattern[j] == text[i])
                {
                    i++;
                    j++;

                    if (j == pattern.Length)
                    {
                        return i - j;
                    }
                }
                else
                {
                    if (j > 0)
                    {
                        j = prefixTable[j - 1];
                    }
                    else
                    {
                        i++;
                    }
                }
            }

            return -1;
        }

        // =========================
        // Parsing: returns a ready-to-display line
        // =========================
        private bool TryParseMessageLine(byte[] raw, bool recv, out string? line)
        {
            line = null;

            if (raw is null || raw.Length < 30)
                return false;

            // Old NYI filters (safe)
            if (raw.Length > 2)
            {
                if (raw[2] == 0xA0) return false;
                if (raw[2] == 0xA4) return false;
                if (raw[2] == 0x14) return false;
                if (raw[2] == 0x9C) return false;
            }

            if (raw.Length < 6) return false;

            // Trim like original
            byte[] data = [.. raw.Skip(2).Take(raw.Length - 4)];
            int dataLen = data.Length;

            if (dataLen < 27) return false;

            // Valid message gate
            if (!(data[24] == 0 && data[25] == 0 && data[26] == 1))
                return false;

            short millisec = BitConverter.ToInt16(data, 8);
            int time = BitConverter.ToInt32(data, 10);

            IPAddress address = new([.. data.Skip(14).Take(4)]);
            string IP = address.ToString();

            byte spacer = 32;
            int headerEnd = Array.IndexOf(data, spacer, 27);
            if (headerEnd == -1) return false;

            int indexOfDataEnd;
            byte[] pattern = [0x00, 0x01];

            if (!recv)
                indexOfDataEnd = FindPattern(data, pattern, headerEnd);
            else
                indexOfDataEnd = dataLen;

            if (indexOfDataEnd == -1) return false;

            string str = Encoding.UTF8.GetString(
                data,
                headerEnd + 1,
                (dataLen - headerEnd) - (dataLen - indexOfDataEnd) - 1
            );

            string header = Encoding.UTF8.GetString(data, 27, headerEnd - 26);
            str = Regex.Replace(str, regexPattern, "\r\n");

            int messageNr = BitConverter.ToUInt16(data, 6);

            line =
                $"{messageNr.ToString().PadLeft(6, '0')}  {LoxTimeStampToDateTime(time)}.{millisec.ToString().PadLeft(3, '0')};  {IP,-16}{header}{str}";

            return true;
        }

        private static DateTime LoxTimeStampToDateTime(double unixTimeStamp)
        {
            DateTime dateTime = new(2009, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }

        // =========================
        // Multi-file loading with progress
        // =========================
        private static long GetTotalBytes(string[] files)
        {
            long total = 0;
            foreach (var f in files)
            {
                try { total += new FileInfo(f).Length; } catch { }
            }
            return Math.Max(total, 1);
        }

        private void AddItemsBatch(List<string> items)
        {
            if (items.Count == 0) return;

            mainListBox.BeginUpdate();
            try
            {
                foreach (var s in items)
                    mainListBox.Items.Add(s);
            }
            finally
            {
                mainListBox.EndUpdate();
            }
        }

        private async void LoadButton_Click(object sender, EventArgs e)
        {
            openFileDialog1.Multiselect = true;

            var dlg = openFileDialog1.ShowDialog();
            if (dlg != DialogResult.OK) return;

            var files = openFileDialog1.FileNames
                .Where(File.Exists)
                .OrderBy(f => f, StringComparer.OrdinalIgnoreCase)
                .ToArray();

            if (files.Length == 0) return;

            // Reset filter view
            FilterButtonState(false);
            FilterTextBox.Text = "";

            // Prep UI
            mainListBox.Items.Clear();

            loadFlag = true;

            // Disable inputs during load
            loadButton.Enabled = false;
            receiveButton.Enabled = false;
            FilterButton.Enabled = false;

            long totalBytes = GetTotalBytes(files);

            // Progress bar setup (normalized 0..1000)
            mainProgressBar.Style = ProgressBarStyle.Continuous;
            mainProgressBar.Minimum = 0;
            mainProgressBar.Maximum = 1000;
            mainProgressBar.Value = 0;

            var progress = new Progress<int>(p =>
            {
                p = Math.Clamp(p, 0, 1000);
                mainProgressBar.Value = p;
            });

            try
            {
                await Task.Run(() => LoadFilesMergedWorker(files, totalBytes, progress));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                mainProgressBar.Value = 0;

                loadButton.Enabled = false;
                receiveButton.Enabled = true;
                FilterButton.Enabled = true;

                if (refreshFlag && mainListBox.Items.Count > 0)
                    mainListBox.TopIndex = mainListBox.Items.Count - 1;
            }
        }

        private void LoadFilesMergedWorker(string[] files, long totalBytes, IProgress<int> progress)
        {
            long completedBytes = 0;

            const int BatchSize = 400;
            var batch = new List<string>(BatchSize);

            void Flush()
            {
                if (batch.Count == 0) return;
                var copy = new List<string>(batch);
                batch.Clear();
                Invoke((MethodInvoker)(() => AddItemsBatch(copy)));
            }

            void ReportNormalized(long absoluteBytesDone)
            {
                int p = (int)Math.Clamp((absoluteBytesDone * 1000L) / totalBytes, 0, 1000);
                progress.Report(p);
            }

            foreach (var file in files)
            {
                if (formClosing) return;

                string ext = Path.GetExtension(file).ToLowerInvariant();

                // Optional file boundary marker
                batch.Add($"----- {Path.GetFileName(file)} -----");
                if (batch.Count >= BatchSize) Flush();

                if (ext == ".lxmon")
                {
                    // Read entire file (count it once), and update progress while parsing by position
                    byte[] lx = File.ReadAllBytes(file);
                    long fileLen = lx.LongLength;

                    // Parse frames and add lines to batch (no UI work here)
                    int i = 0;
                    while (i < lx.Length - 1)
                    {
                        if (formClosing) return;

                        int start = IndexOfSequence(lx, LxMonStart, i);
                        if (start < 0) break;

                        int end = IndexOfSequence(lx, LxMonEnd, start + LxMonStart.Length);
                        if (end < 0) break;

                        int frameLen = (end - start) + LxMonEnd.Length;
                        if (frameLen <= 0) { i = start + 2; continue; }

                        byte[] frame = new byte[frameLen];
                        Buffer.BlockCopy(lx, start, frame, 0, frameLen);

                        if (TryParseMessageLine(frame, recv: false, out string? line) && line is not null)
                        {
                            batch.Add(line);
                            if (batch.Count >= BatchSize) Flush();
                        }

                        // Progress: completed bytes of previous files + current parse position
                        long absolute = completedBytes + Math.Min(fileLen, (long)(start + frameLen));
                        ReportNormalized(absolute);

                        i = start + frameLen;
                    }

                    completedBytes += fileLen;
                    ReportNormalized(completedBytes);
                }
                else
                {
                    // Text file: stream line-by-line; track progress by fs.Position
                    using var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read);
                    using var sr = new StreamReader(fs, Encoding.UTF8, detectEncodingFromByteOrderMarks: true);

                    long fileLen = fs.Length;
                    int lineCounter = 0;

                    while (!sr.EndOfStream)
                    {
                        if (formClosing) return;

                        string? line = sr.ReadLine();
                        if (line is not null) batch.Add(line);

                        if (batch.Count >= BatchSize) Flush();

                        // Update progress every ~200 lines to reduce overhead
                        lineCounter++;
                        if (lineCounter % 200 == 0)
                        {
                            long absolute = completedBytes + fs.Position;
                            ReportNormalized(absolute);
                        }
                    }

                    completedBytes += fileLen;
                    ReportNormalized(completedBytes);
                }
            }

            Flush();
            progress.Report(1000);
        }

        private static int IndexOfSequence(byte[] haystack, byte[] needle, int startIndex)
        {
            if (needle.Length == 0) return -1;
            for (int i = startIndex; i <= haystack.Length - needle.Length; i++)
            {
                bool ok = true;
                for (int j = 0; j < needle.Length; j++)
                {
                    if (haystack[i + j] != needle[j]) { ok = false; break; }
                }
                if (ok) return i;
            }
            return -1;
        }

        // =========================
        // Receive button (switch back to UDP view)
        // =========================
        private void ReceiveButton_Click(object sender, EventArgs e)
        {
            mainListBox.Items.Clear();
            loadFlag = false;
            loadButton.Enabled = true;
            receiveButton.Enabled = false;
            FilterButtonState(false);
            FilterButton.Enabled = false;
        }

        // =========================
        // PPS display
        // =========================
        private void PpsTimer_Tick(object sender, EventArgs e)
        {
            ppsLabel.Text = packetsThisSecond.ToString();
            totalPacketLabel.Text = packetsTotal.ToString();
            packetsThisSecond = 0;
        }

        // =========================
        // Close
        // =========================
        void EagleLoxMonitor_FormClosing(object sender, FormClosingEventArgs e)
        {
            formClosing = true;

            try { udpClient?.Close(); } catch { }
            try { udpClient?.Dispose(); } catch { }
        }

        // =========================
        // Autosave + Save
        // =========================
        private void SaveButton_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
            string savepath = saveFileDialog1.FileName;
            if (savepath != "")
                File.WriteAllLines(savepath, [.. mainListBox.Items.Cast<object>().Select(x => x?.ToString() ?? "")]);
        }

        private void AutoSaveTimer_Tick(object sender, EventArgs e)
        {
            string savepath = autoSaveFolder + autoSaveFile + autoSaveID + autoSaveExtension;
            if (!Directory.Exists(autoSaveFolder)) Directory.CreateDirectory(autoSaveFolder);

            while (File.Exists(savepath))
            {
                autoSaveID++;
                savepath = autoSaveFolder + autoSaveFile + autoSaveID + autoSaveExtension;
            }

            File.WriteAllLines(savepath, [.. mainListBox.Items.Cast<object>().Select(x => x?.ToString() ?? "")]);
        }

        // =========================
        // Refresh checkbox
        // =========================
        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            refreshFlag = !refreshFlag;
        }

        // =========================
        // Ctrl+A / Ctrl+C
        // =========================
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                SelectAllListBoxItems();
            }
            else if (e.Control && e.KeyCode == Keys.C)
            {
                CopySelectedListBoxItems();
            }
        }

        private void SelectAllListBoxItems()
        {
            for (int i = 0; i < mainListBox.Items.Count; i++)
            {
                mainListBox.SetSelected(i, true);
            }
        }

        private void CopySelectedListBoxItems()
        {
            if (mainListBox.SelectedItems.Count > 0)
            {
                string selectedItemsText = "";

                foreach (var selectedItem in mainListBox.SelectedItems)
                {
                    selectedItemsText += selectedItem?.ToString() + Environment.NewLine;
                }

                Clipboard.SetText(selectedItemsText);
            }
        }

        // =========================
        // Filter: +terms, !negation, "quoted phrase"
        // =========================
        private static List<FilterTerm> ParseFilterQuery(string query)
        {
            var terms = new List<FilterTerm>();
            if (string.IsNullOrWhiteSpace(query))
                return terms;

            // Optional ! then either "phrase" or token
            var rx = new Regex(@"(?<neg>!)?(?:(?:""(?<q>[^""]*)"")|(?<w>\S+))",
                RegexOptions.Compiled);

            foreach (Match m in rx.Matches(query))
            {
                bool neg = m.Groups["neg"].Success;
                string text = m.Groups["q"].Success ? m.Groups["q"].Value : m.Groups["w"].Value;

                if (string.IsNullOrWhiteSpace(text))
                    continue;

                terms.Add(new FilterTerm(text, neg));
            }

            return terms;
        }

        private static bool MatchesFilter(string haystack, List<FilterTerm> terms)
        {
            if (terms.Count == 0)
                return true;

            foreach (var t in terms)
            {
                bool contains = haystack.Contains(t.Text, StringComparison.OrdinalIgnoreCase);

                if (!t.Negated && !contains)
                    return false;

                if (t.Negated && contains)
                    return false;
            }

            return true;
        }

        private void FilterButton_Click(object sender, EventArgs e)
        {
            string query = FilterTextBox.Text ?? "";
            if (string.IsNullOrWhiteSpace(query))
            {
                FilterButtonState(false);
                return;
            }

            var terms = ParseFilterQuery(query);

            FilterListBox.BeginUpdate();
            FilterButtonState(true);

            foreach (var item in mainListBox.Items)
            {
                if (item is null) continue;

                string line = item.ToString() ?? "";
                if (MatchesFilter(line, terms))
                    FilterListBox.Items.Add(item);
            }

            FilterListBox.EndUpdate();
        }

        private void FilterButtonState(bool state)
        {
            FilterListBox.Items.Clear();
            if (state)
            {
                FilterListBox.Visible = true;
                FilterButton.FlatStyle = FlatStyle.Flat;
                FilterButton.Enabled = true;
            }
            else
            {
                FilterListBox.Visible = false;
                FilterButton.FlatStyle = FlatStyle.Standard;
            }
        }
    }
}
