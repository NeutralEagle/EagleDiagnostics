using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;

namespace EagleDiagnostics
{
    public partial class NFCPlot : Form
    {
        [GeneratedRegex(@"value=""([^""]*)""", RegexOptions.Singleline)]
        private static partial Regex XmlValueRegex();
        private readonly HttpClient _http;
        private readonly System.Windows.Forms.Timer _timer = new();

        private bool _running;
        private int _acceptedSampleIndex;     // sample index for X axis (accepted samples only)
        private int _dropped;
        private int? _fixedChannelCount;
        private int _currentIteration;

        // ZedGraph series
        private readonly List<RollingPointPairList> _channelLists = [];
        private readonly List<LineItem> _channelCurves = [];

        private readonly string _baseUrl;
        private readonly string _deviceName;
        private readonly string _username;
        private readonly string _password;


        


        public NFCPlot(string baseUrl, string deviceName, string username, string password)
        {
            InitializeComponent();

            _baseUrl = (baseUrl ?? "").Trim().TrimEnd('/');
            txtResolvedUrl.Text = _baseUrl;
            _deviceName = (deviceName ?? "").Trim();
            txtDevice.Text = _deviceName;
            _username = username ?? "";
            txtUsername.Text = _username;
            _password = password ?? "";
            txtPassword.Text = _password;

            _http = new HttpClient(new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            });

            // Defaults
            nudIntervalMs.Value = 1000;
            nudMaxSamples.Value = 500;
            nudIterations.Value = 500;

            InitGraph();

            _timer.Interval = (int)nudIntervalMs.Value;
            _timer.Tick += async (_, _) => await PollOnceAsync();
        }

        private void InitGraph()
        {
            var pane = zedGraphControl1.GraphPane;

            pane.CurveList.Clear();
            pane.Title.Text = "CS - LTA";
            pane.XAxis.Title.Text = "Sample";
            pane.YAxis.Title.Text = "CS - LTA";
            pane.XAxis.Type = AxisType.Linear;

            _channelLists.Clear();
            _channelCurves.Clear();

            _fixedChannelCount = null;
            _acceptedSampleIndex = 0;
            _dropped = 0;

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

        private string BuildWsUrl()
        {
            var devEscaped = Uri.EscapeDataString(_deviceName);
            return $"{_baseUrl}/dev/sys/wsdevice/{devEscaped}/IQS626ReadAllValues";
        }

        private async Task<string> RequestWSRawAsync()
        {
            ApplyBasicAuth();
            var url = BuildWsUrl();

            using var resp = await _http.GetAsync(url).ConfigureAwait(false);
            var text = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);

            // Loxone sometimes returns HTTP 200 even for timeouts; treat payload content as truth.
            if (!resp.IsSuccessStatusCode)
                throw new Exception($"HTTP {(int)resp.StatusCode} {resp.ReasonPhrase} - {text}");

            return text;
        }

        // ---- Normalization (Python-equivalent behavior) ----

        private static string? NormalizePayload(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
                return null;

            var t = raw.Trim();

            // Plain Reg text (fast path)
            if (t.Contains("Reg9:", StringComparison.OrdinalIgnoreCase) &&
                !t.Contains("<LL", StringComparison.OrdinalIgnoreCase))
            {
                return t;
            }

            // XML envelope: extract value="..."
            var m = XmlValueRegex().Match(t);
            if (!m.Success)
                return null;

            var val = m.Groups[1].Value;

            if (string.Equals(val.Trim(), "timeout", StringComparison.OrdinalIgnoreCase))
                return null;

            return val;
        }

        // ---- CS/LTA parsing from Reg9 only ----

        private static bool TryParseHexByte(string s, out int value)
        {
            value = 0;
            if (s is null) return false;

            s = s.Trim();
            if (s.Length == 0) return false;

            if (s.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                s = s[2..];

            return int.TryParse(s, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out value);
        }

        private static bool TryExtractCsLta(string payload, out int[] cs, out int[] lta)
        {
            cs = [];
            lta = [];

            // Python: r1 = payload.split(';')  (keep empties)
            var r1 = payload.Split(';');

            // Find segment containing Reg9:
            string? seg = null;
            foreach (var s in r1)
            {
                if (s.Contains("Reg9:", StringComparison.OrdinalIgnoreCase))
                {
                    seg = s;
                    break;
                }
            }
            if (seg is null)
                return false;

            // Python: r2 = seg.split(' ')  (keeps empties, including leading space token)
            var r2 = seg.Split(' ');

            // Find where "Reg9:" token is; bytes start after it
            int regIdx = Array.FindIndex(r2, x => x.Contains("Reg9:", StringComparison.OrdinalIgnoreCase));
            if (regIdx < 0)
                return false;

            int start = regIdx + 1;

            // Need full groups of 4 bytes (CS lo/hi, LTA lo/hi)
            int remaining = r2.Length - start;
            if (remaining < 4 || (remaining % 4) != 0)
                return false;

            var csList = new List<int>(remaining / 4);
            var ltaList = new List<int>(remaining / 4);

            for (int j = start; j + 3 < r2.Length; j += 4)
            {
                if (!TryParseHexByte(r2[j], out int csLo) ||
                    !TryParseHexByte(r2[j + 1], out int csHi) ||
                    !TryParseHexByte(r2[j + 2], out int ltaLo) ||
                    !TryParseHexByte(r2[j + 3], out int ltaHi))
                {
                    return false;
                }

                csList.Add((csHi << 8) | csLo);
                ltaList.Add((ltaHi << 8) | ltaLo);
            }

            if (csList.Count == 0 || csList.Count != ltaList.Count)
                return false;

            cs = [.. csList];
            lta = [.. ltaList];
            return true;
        }

        // ---- Graph series management ----

        private bool EnsureCurvesFixed(int channelCount)
        {
            if (_fixedChannelCount is null)
            {
                _fixedChannelCount = channelCount;

                var pane = zedGraphControl1.GraphPane;
                pane.CurveList.Clear();
                _channelLists.Clear();
                _channelCurves.Clear();

                int maxPts = (int)nudMaxSamples.Value;

                var colors = new[]
                {
                    System.Drawing.Color.Blue,
                    System.Drawing.Color.Red,
                    System.Drawing.Color.Green,
                    System.Drawing.Color.Orange,
                    System.Drawing.Color.Purple,
                    System.Drawing.Color.Brown
                };

                for (int ch = 0; ch < channelCount; ch++)
                {
                    var list = new RollingPointPairList(maxPts);
                    _channelLists.Add(list);
                    _channelCurves.Add(pane.AddCurve($"Ch {ch + 1}", list, colors[ch % colors.Length], SymbolType.None));
                }

                zedGraphControl1.AxisChange();
                zedGraphControl1.Invalidate();
                return true;
            }

            return channelCount == _fixedChannelCount.Value;
        }

        private void PlotCsMinusLta(int[] cs, int[] lta)
        {
            int n = Math.Min(cs.Length, lta.Length);
            if (n <= 0)
                return;

            if (!EnsureCurvesFixed(n))
            {
                // don’t wipe plot; skip inconsistent sample
                _dropped++;
                Log($"Dropped (channel mismatch). got={n}, expected={_fixedChannelCount}, dropped={_dropped}");
                return;
            }

            double x = _acceptedSampleIndex;

            for (int ch = 0; ch < n; ch++)
            {
                
                int diff = cs[ch] - lta[ch];
                if (diff < -1000 || diff > 1000)
                {
                    // don’t wipe plot; skip inconsistent sample
                    _dropped++;
                    Log($"Dropped (diff over +-1000). diff={diff}, dropped={_dropped}");
                    return;
                }
                _channelLists[ch].Add(x, diff);
            }

            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();
        }

        // ---- Poll loop ----

        private async Task PollOnceAsync()
        {
            if (!_running)
                return;
            if (_currentIteration > nudIterations.Value)
                {
                    StopRun();
                    Log($"Reached iteration limit ({nudIterations.Value}). Stopping.");
                    return;
                }

            _timer.Interval = (int)nudIntervalMs.Value;

            try
            {
                var raw = await RequestWSRawAsync().ConfigureAwait(true);

                var payload = NormalizePayload(raw);
                if (payload is null)
                {
                    _dropped++;
                    Log($"Dropped (timeout/no payload). dropped={_dropped}");
                    return;
                }

                // CS/LTA only: drop entire read if we can't parse it cleanly
                if (!TryExtractCsLta(payload, out var cs, out var lta))
                {
                    _dropped++;
                    Log($"Dropped (missing/invalid CS/LTA). dropped={_dropped} payload={payload}");
                    return;
                }

                // accepted sample
                _acceptedSampleIndex++;
                Log($"Accepted #{_acceptedSampleIndex}");

                // plot (needs at least 1 sample; python starts at >=2, but not required here)
                PlotCsMinusLta(cs, lta);
            }
            catch (Exception ex)
            {
                _dropped++;
                Log($"Dropped (exception): {ex.Message}");
            }
        }

        private void StartRun()
        {
            _running = true;
            _currentIteration = 0;
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
