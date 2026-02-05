using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Linq;

namespace EagleDiagnostics
{
    public partial class WSSender : Form
    {
        // Backing fields made non-public to satisfy CA2211 (non-constant fields should not be visible)
        private static string finalHost = "";
        private static string finalUrl = "";
        private static string deviceSN = "";
        private static string msSN = "";
        private static string httpPrefix = "https://";
        private static string login = string.Empty;

        // Public properties replace the previously public mutable fields
        public static string FinalHost { get => finalHost; set => finalHost = value; }
        public static string FinalUrl { get => finalUrl; set => finalUrl = value; }
        public static string DeviceSN { get => deviceSN; set => deviceSN = value; }
        public static string MsSN { get => msSN; set => msSN = value; }
        public static string HttpPrefix { get => httpPrefix; set => httpPrefix = value; }
        public static string Login { get => login; set => login = value; }

        readonly Dictionary<string, string> shellVarValuePairs = new();

        public WSSender()
        {
            InitializeComponent();
            logger.TextChanged += Logger_TextChanged;
        }

        private static bool CheckSN(string input)
        {
            msSN = Regex.Replace(input, "[^a-fA-F0-9]", "").ToUpper();
            /*if (msSN.Length == 12 && (msSN.StartsWith("EEE000")|| msSN.StartsWith("504F94")))
            char msTypeChar = msSN[6];
            if (msTypeChar == 'A'||msTypeChar =='D') httpPrefix = "https://";
            else if (msTypeChar == '1') httpPrefix = "http://";
            else return false;
            return true;
            {
                char msTypeChar = msSN[6];
                if (msTypeChar == 'A'||msTypeChar =='D') httpPrefix = "https://";
                else if (msTypeChar == '1') httpPrefix = "http://";
                else return false;
                return true;
            }
            else if */
            if (msSN.Length == 12)
            {
                if (msSN.StartsWith("504F94A") || msSN.StartsWith("504F94D")) httpPrefix = "https://";
                else if (msSN.StartsWith("EEE000") || msSN.StartsWith("504F941")) httpPrefix = "http://";
                else return false;
            }
            return true;
        }
        private async Task CheckFinalURL()
        {
            if (deviceSN == "") deviceSN = deviceTextBox.Text;
            deviceSN = Regex.Replace(deviceSN, "[^a-zA-Z0-9]", "");


            string initialUrl = $"{httpPrefix}dns.loxonecloud.com/{msSN}";
            // Await a nullable string result
            string? url = await GetRedirectedUrlAsync(initialUrl, login);
            if (url != null)
            {
                finalHost = GetHostFromUrl(url);
                finalUrl = $"{url}dev/sys/wsdevice/{deviceSN}/getvar";
            }
            else { MessageBox.Show("Unable to connect, please check MS SN and login info"); }


        }
        private async void Button1_Click(object sender, EventArgs e)
        {
            if (CheckSN(msTextBox.Text) == false)
            {
                MessageBox.Show("Invalid MS Serial number");
                return;
            }


            deviceSN = deviceTextBox.Text;
            login = EncodeToBase64(userTextBox.Text, pwTextBox.Text);
            await CheckFinalURL();


            if (!string.IsNullOrEmpty(finalUrl))
            {
                string xmlData = await GetXmlDataAsync(finalUrl, login);
                logger.Text += xmlData + "\r\n";
                List<string> shellVars = ParseXmlData(xmlData);

                if (shellVars.Count > 0)
                {
                    // Now you have the list of ShellVars, you can work with it as needed
                    panel.Controls.Clear();
                    await CreateTable(shellVars, panel, logger);
                    NfcPlotBtn.Enabled = true;
                }
                else
                {
                    NfcPlotBtn.Enabled = false;
                    // Handle the case when no ShellVars were found
                }
            }
            else
            {
                NfcPlotBtn.Enabled = false;
                // Handle the case when the redirection URL is not available
            }
        }

        static async Task CreateTable(List<string> shellVars, Control panel, Control logger)
        {
            int y = 20;
            int x = 10;
            foreach (string shellVar in shellVars)
            {
                Label label = new()
                {
                    Text = shellVar,
                    Location = new Point(x, y)
                };
                string url = $"{httpPrefix}{finalHost}/dev/sys/wsdevice/{deviceSN}/get/{shellVar}";
                TextBox textBox = new()
                {

                    Text = $"{SendWebServiceRequest(url, login, logger)}",
                    Location = new Point(x + label.Width + 20, y)

                };

                panel.Controls.Add(label);
                panel.Controls.Add(textBox);
                if (y > 720) { y = 20; x += label.Width + textBox.Width + 40; }
                else
                    y += 24;

            }
        }
        public static string GetHostFromUrl(string url)
        {
            try
            {
                Uri uri = new(url);
                return uri.Host + ":" + uri.Port;
            }
            catch (UriFormatException ex)
            {
                throw new Exception($"Failed to extract host from URL: {ex.Message}");
            }
        }
        public static string SendWebServiceRequest(string url, string accessToken, Control logger)
        {
            using var client = new HttpClient();
            // Set the Authorization header
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", accessToken);
            logger.Text += "Sending request: " + url + "\r\n";

            HttpResponseMessage response = client.GetAsync(url).Result; // Synchronous call
            logger.Text += response.StatusCode + "\r\n";

            if (response.IsSuccessStatusCode)
            {
                string xmlContent = response.Content.ReadAsStringAsync().Result; // Synchronous read of response content
                logger.Text += xmlContent;
                XmlDocument xmlDoc = new();

                xmlDoc.LoadXml(xmlContent);

                XmlNode? valueNode = xmlDoc.SelectSingleNode("//LL[@value]");
                // Use null‑propagating operators and a local variable to avoid possible null dereference
                var valueAttr = valueNode?.Attributes?["value"];
                if (valueAttr != null)
                {
                    return valueAttr.Value.Trim();
                }
                else
                {
                    throw new Exception("XML response does not contain a value attribute.");
                }
            }
            else
            {
                throw new Exception($"Failed to send web service request. Status Code: {response.StatusCode}");
            }
        }

        static async Task<string?> GetRedirectedUrlAsync(string initialUrl, string accessToken)
        {
            using var client = new HttpClient(new HttpClientHandler { AllowAutoRedirect = false });

            // Use the provided access token to set the Authorization header when available.
            if (!string.IsNullOrEmpty(accessToken))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", accessToken);
            }

            HttpResponseMessage response = await client.GetAsync(initialUrl);

            // Ensure there's a Location header to return (redirect target).
            if (response.Headers.Location != null)
            {
                return response.Headers.Location.OriginalString;
            }

            return null; // nullable return permitted by signature
        }

        static async Task<string> GetXmlDataAsync(string url, string accessToken)
        {
            using var client = new HttpClient();
            // Set the Authorization header
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", accessToken);

            string xmlContent = await client.GetStringAsync(url);

            return xmlContent;
        }

        public static string EncodeToBase64(string user, string password)
        {
            // Combine the user and password with a colon separator
            string combinedString = $"{user}:{password}";

            // Convert the combined string to a byte array
            byte[] bytes = Encoding.UTF8.GetBytes(combinedString);

            // Encode the byte array to Base64
            string base64String = Convert.ToBase64String(bytes);

            return base64String;
        }

        static List<string> ParseXmlData(string xmlData)
        {
            List<string> shellVars = new();

            // Guard against null/empty input
            if (string.IsNullOrWhiteSpace(xmlData))
                return shellVars;

            try
            {
                XmlDocument xmlDoc = new();
                xmlDoc.LoadXml(xmlData);

                XmlNodeList llNodes = xmlDoc.GetElementsByTagName("LL");

                foreach (XmlNode llNode in llNodes)
                {
                    // XmlNode.Attributes can be null; check it first and then the "value" attribute
                    var attrs = llNode.Attributes;
                    var valueAttr = attrs?["value"];
                    if (valueAttr != null)
                    {
                        string[] varNames = valueAttr.Value.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                        foreach (string varName in varNames)
                        {
                            if (varName.Contains("ShellSysVars :"))
                                break;
                            // Exclude entries with "ShellVars" or "ShellSysVars"
                            if (!varName.Contains("ShellVars :"))
                            {
                                shellVars.Add(varName.Trim()); // Trim to remove leading/trailing whitespace
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any parsing exceptions here
                MessageBox.Show("Error parsing XML: " + ex.Message);
            }

            return shellVars;
        }
        static void UpdateValuePairsFromPanel(Control panel, Dictionary<string, string> shellVarValuePairs)
        {
            foreach (Control control in panel.Controls)
            {
                if (control is Label label)
                {
                    // Check if this control is a Label (represents a ShellVar)
                    string shellVar = label.Text;

                    // Find the corresponding TextBox control (nullable)
                    TextBox? textBox = panel.Controls
                        .OfType<TextBox>()
                        .FirstOrDefault(tb => tb.Location.X == label.Location.X + label.Width + 20 && tb.Location.Y == label.Location.Y);

                    if (textBox != null)
                    {
                        // Update the value pair in the dictionary
                        string value = textBox.Text;
                        shellVarValuePairs[shellVar] = value;
                    }
                }
            }
        }
        private void BtnSend_Click(object sender, EventArgs e)
        {
            UpdateValuePairsFromPanel(panel, shellVarValuePairs);
            if (rBErase.Checked) SendWebservices("erase");
            else if (rBSet.Checked) SendWebservices("set");
            else if (rBStore.Checked) SendWebservices("store");


        }

        private void SendWebservices(string command)
        {
            if (command == "erase")
                foreach (var shellVar in shellVarValuePairs)
                {
                    SendWebServiceRequest($"{httpPrefix}{finalHost}/dev/sys/wsdevice/{deviceSN}/{command}/{shellVar.Key}", login, logger);
                }
            else
                foreach (var shellVar in shellVarValuePairs)
                {
                    SendWebServiceRequest($"{httpPrefix}{finalHost}/dev/sys/wsdevice/{deviceSN}/{command}/{shellVar.Key}/{shellVar.Value}", login, logger);
                }
        }

        private async void BtnReboot_Click(object sender, EventArgs e)
        {
            await CheckFinalURL();
            SendWebServiceRequest($"{httpPrefix}{finalHost}/dev/sys/wsdevice/{deviceSN}/Reboot", login, logger);
        }
        private void Logger_TextChanged(object? sender, EventArgs e)
        {
            // set the current caret position to the end
            logger.SelectionStart = logger.Text.Length;
            // scroll it automatically
            logger.ScrollToCaret();
        }

        private async void NfcPlotBtn_Click(object sender, EventArgs e)
        {
            await CheckFinalURL();
            Form b = new NFCPlot($"{httpPrefix}{finalHost}", deviceSN, userTextBox.Text, pwTextBox.Text);
            b.Show();
        }

    }
}
