using System.Diagnostics;
using System.Net;
using ZedGraph;
using System.Net.Http;

namespace EagleDiagnostics
{
    public partial class NFCPlot : Form
    {
        private static string url = "";
        private string devName = "";
        private readonly Dictionary<string, int[]> regVal = new Dictionary<string, int[]>();
        private readonly List<DateTime> timeVec = new List<DateTime>();
        private const int itCount = 300;
        private const double tSleep = 0.25;
        private static string username = "";
        private static string password = "";
        private static Color[] colorList = { Color.Aqua, Color.Green, Color.Blue, Color.Purple, Color.Red, Color.Orange, Color.Navy };

        // Initialize with null-forgiving to satisfy the compiler: it will be assigned in InitializeChart()
        private ZedGraphControl zedGraphControl = null!;

        public NFCPlot(string urlParam, string devNameParam, string usernameParam, string passwordParam)
        {
            url = urlParam;
            devName = devNameParam;
            username = usernameParam;
            password = passwordParam;
            InitializeComponent();
            InitializeChart();
            Task.Run(() => GetData()); // Start data retrieval in the background
        }

        private void InitializeChart()
        {
            zedGraphControl = new ZedGraphControl
            {
                Dock = DockStyle.Fill
            };
            Controls.Add(zedGraphControl);

            GraphPane myPane = zedGraphControl.GraphPane;
            myPane.Title.Text = "CS-LTA";
            myPane.XAxis.Title.Text = "Time";
            myPane.YAxis.Title.Text = "Value";
            int x = 0;
            foreach (var key in regVal.Keys)
            {

                LineItem curve = myPane.AddCurve(key, new PointPairList(), colorList[x], SymbolType.None);
                curve.Line.IsSmooth = false;
                x++;
            }

            zedGraphControl.AxisChange();
        }


        private void PlotData()
        {
            GraphPane myPane = zedGraphControl.GraphPane;

            for (int i = 1; i < timeVec.Count; i++)
            {
                foreach (var key in regVal.Keys)
                {
                    // Use safe nullable cast so FirstOrDefault() null results are handled without warnings
                    LineItem? curve = myPane.CurveList.FirstOrDefault(c => c.Label.Text == key) as LineItem;
                    if (curve != null)
                    {
                        double x = new XDate(timeVec[i]);
                        double y = regVal[key][i];
                        curve.AddPoint(x, y);
                    }
                }
            }

            zedGraphControl.AxisChange();
            zedGraphControl.Invalidate();
        }

        private async Task GetData()
        {
            string response = await RequestWebService("/dev/sys/wsdevice/" + devName + "/IQS626ReadAllValues");
            timeVec.Add(DateTime.Now);

            List<string> regKeys = new List<string>
        {
            "Reg0x02",
            "Reg0x04",
            "Reg0x09",
            "Reg0x80",
            "Reg0x91",
            "Reg0xA3"
        };
            string[] r1 = response.Split("; ");
            Dictionary<string, int[]> regVal = new Dictionary<string, int[]>();

            for (int i = 0; i < r1.Count() - 1; i++)
            {
                string[] r2 = r1[i].Split(' ');

                if (i == 0) // Reg0x02
                {
                    string currKey = regKeys[i];
                    string[] h = r1[0].Substring(r1[0].IndexOf("Reg2:")).Split(' ');
                    int[] hArray = new int[] { Convert.ToInt32(h[1], 16), Convert.ToInt32(h[2], 16) };

                    if (!regVal.ContainsKey(currKey) || regVal[currKey].Length == 0)
                    {
                        regVal[currKey] = hArray;
                    }
                    else
                    {
                        regVal[currKey] = regVal[currKey].Concat(hArray).ToArray();
                    }
                }
                else if (i == 1) // Reg0x04
                {
                    string currKey = regKeys[i];
                    regVal[currKey] = RegStr2Num(r2, currKey, regVal);
                }
                else if (i == 2) // Reg0x09
                {
                    List<int> hCS = new List<int>();
                    List<int> hLTA = new List<int>();

                    for (int j = 2; j < r2.Length; j += 4)
                    {
                        hCS.Add(Convert.ToInt32(r2[j + 1] + r2[j], 16));
                        hLTA.Add(Convert.ToInt32(r2[j + 3] + r2[j + 2], 16));
                    }

                    if (!regVal.ContainsKey("LTA") || regVal["LTA"].Length == 0)
                    {
                        regVal["LTA"] = hLTA.ToArray();
                        regVal["CS"] = hCS.ToArray();
                    }
                    else
                    {
                        regVal["CS"] = regVal["CS"].Concat(hCS).ToArray();
                        regVal["LTA"] = regVal["LTA"].Concat(hLTA).ToArray();
                    }
                }
                else if (i == 3) // Reg0x80
                {
                    string currKey = regKeys[i + 1];
                    regVal[currKey] = RegStr2Num(r2, currKey, regVal);
                }
                else if (i == 4) // Reg0x91
                {
                    string currKey = regKeys[i + 1];
                    regVal[currKey] = RegStr2Num(r2, currKey, regVal);
                }
                else if (i == 5) // Reg0xA3
                {
                    string currKey = regKeys[i + 1];
                    regVal[currKey] = RegStr2Num(r2, currKey, regVal);
                }
            }

            PlotData();

            // Output the regVal dictionary
            foreach (var entry in regVal)
            {
                Debug.Write(entry.Key + ": ");
                Debug.WriteLine(string.Join(" ", entry.Value.Select(x => "0x" + x.ToString("X2"))));
            }
        }

        static int[] RegStr2Num(string[] r2, string currKey, Dictionary<string, int[]> regVal)
        {
            List<int> result = new List<int>();

            foreach (string val in r2)
            {
                result.Add(Convert.ToInt32(val, 16));
            }

            return result.ToArray();
        }




        private async Task<string> RequestWebService(string webService)
        {
            string urlRC = url + webService;
            Debug.WriteLine(urlRC);
            await Task.Delay(1000);

            // Use HttpClient with a handler that supports credentials to replace obsolete WebClient
            var handler = new HttpClientHandler
            {
                Credentials = new NetworkCredential(username, password)
            };

            using (var client = new HttpClient(handler, disposeHandler: true))
            {
                // Set a reasonable timeout
                client.Timeout = TimeSpan.FromSeconds(30);

                var response = await client.GetAsync(urlRC);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
        }
    }
}