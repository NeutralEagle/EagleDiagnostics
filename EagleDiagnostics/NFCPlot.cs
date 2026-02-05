using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;

namespace EagleDiagnostics
{
    public partial class NFCPlot : Form
    {
        private static readonly string[] RegKeys =
        [
            "System", "States", "CS", "LTA", "Settings", "Trackpad", "Generic Channel"
        ];

        private readonly Dictionary<string, List<int[]>> _regHistory = [];

        private readonly HttpClient _http;
        private readonly System.Windows.Forms.Timer _timer = new();

        private bool _running;
        private int _sampleIndex;

        // ZedGraph series
        private readonly List<RollingPointPairList> _channelLists = [];
        private readonly List<LineItem> _channelCurves = [];

        // ✅ Injected connection data
        private readonly string _baseUrl;     // e.g. "https://dns.loxonecloud.com/504F..."
        private readonly string _deviceName;  // your wsdevice name (Python devName), ex: "B0307126"
        private readonly string _username;
        private readonly string _password;

        // ✅ New constructor matching your call:
        // new NFCPlot($"{httpPrefix}{finalHost}", deviceSN, user, pw)
        public NFCPlot(string baseUrl, string deviceName, string username, string password)
        {
            InitializeComponent();

            _baseUrl = (baseUrl ?? "").Trim().TrimEnd('/');
            _deviceName = deviceName ?? "";
            _username = username ?? "";
            _password = password ?? "";

            foreach (var k in RegKeys)
                _regHistory[k] = [];

            _http = new HttpClient(new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            });

            // Populate UI for visibility/debug (optional)
            txtResolvedUrl.ReadOnly = true;
            txtResolvedUrl.Text = _baseUrl;

            txtDevice.Text = _deviceName;
            txtUsername.Text = _username;
            txtPassword.Text = _password;


            // If you don't want them shown, set Visible=false in designer instead.

            // Defaults
            nudIntervalMs.Value = 250;
            nudIterations.Value = 500;
            nudMaxSamples.Value = 500;

            InitGraph();

            _timer.Interval = (int)nudIntervalMs.Value;
            _timer.Tick += async (_, _) => await PollOnceAsync();
        }
        private void InitGraph()
        {
            var pane = zedGraphControl1.GraphPane;
            pane.Title.Text = "CS - LTA";
            pane.XAxis.Title.Text = "Time";
            pane.YAxis.Title.Text = "CS - LTA";
            pane.XAxis.Type = AxisType.Date;
            pane.XAxis.Scale.Format = "HH:mm:ss";

            pane.CurveList.Clear();
            _channelLists.Clear();
            _channelCurves.Clear();

            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();
        }
        private void Log(string msg)
        {
            var line = $"[{DateTime.Now:HH:mm:ss}] {msg}";
            lstLog.Items.Add(line);
            lstLog.TopIndex = lstLog.Items.Count - 1;
        }
        private void ApplyBasicAuth()
        {
            var token = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_username}:{_password}"));
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", token);
        }

        private string BuildWebServiceUrl()
        {
            // Python: /dev/sys/wsdevice/{devName}/IQS626ReadAllValues
            var devEscaped = Uri.EscapeDataString(_deviceName.Trim());
            return $"{_baseUrl}/dev/sys/wsdevice/{devEscaped}/IQS626ReadAllValues";
        }

        private async Task<string> RequestWSAsync()
        {
            ApplyBasicAuth();

            // Probe (optional) to follow redirect like your Python r.url usage
            // If you already pass finalHost that doesn't redirect, you can remove probe.
            string resolvedBase = _baseUrl;
            try
            {
                using var probe = await _http.GetAsync(_baseUrl);
                if (probe.RequestMessage?.RequestUri is Uri finalUri)
                    resolvedBase = finalUri.ToString().TrimEnd('/');
            }
            catch
            {
                // If probe fails, still try direct call based on provided baseUrl
                resolvedBase = _baseUrl;
            }

            var devEscaped = Uri.EscapeDataString(_deviceName.Trim());
            var url = $"{resolvedBase}/dev/sys/wsdevice/{devEscaped}/IQS626ReadAllValues";

            txtResolvedUrl.Text = url;

            using var resp = await _http.GetAsync(url);
            var text = await resp.Content.ReadAsStringAsync();
            if (!resp.IsSuccessStatusCode)
                throw new Exception($"HTTP {(int)resp.StatusCode} {resp.ReasonPhrase} - {text}");

            return text;
        }

        private static bool TryParseHexByte(string s, out int value)
        {
            s = s.Trim();
            if (s.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                s = s[2..];

            return int.TryParse(s, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out value);
        }

        private static int[] ParseRegStrToNumbers(string[] parts, int startIndex)
        {
            var nums = new List<int>();
            for (int i = startIndex; i < parts.Length; i++)
            {
                if (TryParseHexByte(parts[i], out int v))
                    nums.Add(v);
            }
            return [.. nums];
        }

        private void AppendHistory(string key, int[] row)
        {
            _regHistory[key].Add(row);

            int max = (int)nudMaxSamples.Value;
            if (_regHistory[key].Count > max)
                _regHistory[key].RemoveRange(0, _regHistory[key].Count - max);
        }

        private void ParseAllValuesResponse(string text)
        {
            var r1 = text.Split([';'], StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < r1.Length; i++)
            {
                var line = r1[i].Trim();
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var parts = line.Split([' '], StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length < 3)
                    continue;

                try
                {
                    if (i == 0)
                    {
                        int idx = Array.FindIndex(parts, p => p.Contains("Reg2:", StringComparison.OrdinalIgnoreCase));
                        if (idx >= 0 && idx + 2 < parts.Length)
                        {
                            if (TryParseHexByte(parts[idx + 1], out int a) &&
                                TryParseHexByte(parts[idx + 2], out int b))
                            {
                                AppendHistory("System", [a, b]);
                            }
                        }
                    }
                    else if (i == 1)
                    {
                        AppendHistory("States", ParseRegStrToNumbers(parts, 2));
                    }
                    else if (i == 2)
                    {
                        var cs = new List<int>();
                        var lta = new List<int>();

                        for (int j = 2; j + 3 < parts.Length; j += 4)
                        {
                            if (TryParseHexByte(parts[j], out int csLo) &&
                                TryParseHexByte(parts[j + 1], out int csHi) &&
                                TryParseHexByte(parts[j + 2], out int ltaLo) &&
                                TryParseHexByte(parts[j + 3], out int ltaHi))
                            {
                                cs.Add((csHi << 8) | csLo);
                                lta.Add((ltaHi << 8) | ltaLo);
                            }
                        }

                        if (cs.Count > 0) AppendHistory("CS", [.. cs]);
                        if (lta.Count > 0) AppendHistory("LTA", [.. lta]);
                    }
                    else if (i == 3)
                    {
                        AppendHistory("Settings", ParseRegStrToNumbers(parts, 2));
                    }
                    else if (i == 4)
                    {
                        AppendHistory("Trackpad", ParseRegStrToNumbers(parts, 2));
                    }
                    else if (i == 5)
                    {
                        AppendHistory("Generic Channel", ParseRegStrToNumbers(parts, 2));
                    }
                }
                catch
                {
                    // Match your Python "keep going" behavior
                }
            }
        }

        private void EnsureCurves(int channelCount)
        {
            if (_channelCurves.Count == channelCount)
                return;

            var pane = zedGraphControl1.GraphPane;
            pane.CurveList.Clear();
            _channelLists.Clear();
            _channelCurves.Clear();

            int maxPts = (int)nudMaxSamples.Value;

            for (int ch = 0; ch < channelCount; ch++)
            {
                var list = new RollingPointPairList(maxPts);
                _channelLists.Add(list);

                var colors = new[]
                {
                    System.Drawing.Color.Blue,
                    System.Drawing.Color.Red,
                    System.Drawing.Color.Green,
                    System.Drawing.Color.Orange,
                    System.Drawing.Color.Purple,
                    System.Drawing.Color.Brown
                };

                var curve = pane.AddCurve($"Ch {ch + 1}", list, colors[ch % colors.Length], SymbolType.None);
                _channelCurves.Add(curve);
            }

            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();
        }

        private void PlotCsMinusLta(DateTime ts)
        {
            var csHist = _regHistory["CS"];
            var ltaHist = _regHistory["LTA"];
            if (csHist.Count == 0 || ltaHist.Count == 0)
                return;

            var cs = csHist.Last();
            var lta = ltaHist.Last();

            int n = Math.Min(cs.Length, lta.Length);
            if (n <= 0)
                return;

            EnsureCurves(n);

            double x = new XDate(ts);

            for (int ch = 0; ch < n; ch++)
            {
                int diff = cs[ch] - lta[ch];
                _channelLists[ch].Add(x, diff);
            }

            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();
        }

        private async Task PollOnceAsync()
        {
            if (!_running) return;

            _timer.Interval = (int)nudIntervalMs.Value;

            try
            {
                var body = await RequestWSAsync();
                ParseAllValuesResponse(body);

                _sampleIndex++;
                Log($"Reading #{_sampleIndex}");

                if (_sampleIndex >= 2)
                    PlotCsMinusLta(DateTime.Now);

                int maxIt = (int)nudIterations.Value;
                if (maxIt > 0 && _sampleIndex >= maxIt)
                    StopRun();
            }
            catch (Exception ex)
            {
                Log("Failed to read: " + ex.Message);
            }
        }

        private void StartRun()
        {
            _running = true;
            _sampleIndex = 0;

            foreach (var k in RegKeys) _regHistory[k].Clear();
            InitGraph();

            _timer.Start();
            Log("Started.");
        }

        private void StopRun()
        {
            _running = false;
            _timer.Stop();
            Log("Stopped.");
        }

        private async void BtnReadOnce_Click(object sender, EventArgs e)
        {
            try
            {
                var body = await RequestWSAsync();
                ParseAllValuesResponse(body);

                _sampleIndex++;
                Log($"Single read #{_sampleIndex}");

                if (_sampleIndex >= 2)
                    PlotCsMinusLta(DateTime.Now);
            }
            catch (Exception ex)
            {
                Log("ReadOnce failed: " + ex.Message);
            }
        }

        private void BtnStart_Click(object sender, EventArgs e) => StartRun();
        private void BtnStop_Click(object sender, EventArgs e) => StopRun();

        private void NFCPlot_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                StopRun();
                _http.Dispose();
                _timer.Dispose();
            }
            catch { }
        }
    }
}
