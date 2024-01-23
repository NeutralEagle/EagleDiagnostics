using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace EagleDiagnostics
{
    public partial class WSSender : Form
    {
        public static string finalHost = "";
        public static string finalUrl = "";
        public static string deviceSN = "";
        public static string msSN = "";
        public static string httpPrefix = "https://";
        static string login;

        Dictionary<string, string> shellVarValuePairs = new Dictionary<string, string>();
        public WSSender()
        {
            InitializeComponent();
            logger.TextChanged += logger_TextChanged;
        }

        private bool checkSN(string input)
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
            string url = await GetRedirectedUrlAsync(initialUrl, login);
            if (url != null)
            {
                finalHost = GetHostFromUrl(url);
                finalUrl = $"{url}dev/sys/wsdevice/{deviceSN}/getvar";
            }
            else { MessageBox.Show("Unable to connect, please check MS SN and login info"); }


        }
        private async void button1_Click(object sender, EventArgs e)
        {
            if (checkSN(msTextBox.Text) == false)
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
                    CreateTable(shellVars, panel, logger);
                }
                else
                {
                    // Handle the case when no ShellVars were found
                }
            }
            else
            {
                // Handle the case when the redirection URL is not available
            }
        }

        static async Task CreateTable(List<string> shellVars, Control panel, Control logger)
        {
            int y = 20;
            int x = 10;
            foreach (string shellVar in shellVars)
            {
                Label label = new Label()
                {
                    Text = shellVar,
                    Location = new Point(x, y)
                };
                string url = $"{httpPrefix}{finalHost}/dev/sys/wsdevice/{deviceSN}/get/{shellVar}";
                TextBox textBox = new TextBox()
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
        public string GetHostFromUrl(string url)
        {
            try
            {
                Uri uri = new Uri(url);
                return uri.Host + ":" + uri.Port;
            }
            catch (UriFormatException ex)
            {
                throw new Exception($"Failed to extract host from URL: {ex.Message}");
            }
        }
        public static string SendWebServiceRequest(string url, string accessToken, Control logger)
        {
            using (HttpClient client = new HttpClient())
            {
                // Set the Authorization header
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", accessToken);
                logger.Text += "Sending request: " + url + "\r\n";

                HttpResponseMessage response = client.GetAsync(url).Result; // Synchronous call
                logger.Text += response.StatusCode + "\r\n";

                if (response.IsSuccessStatusCode)
                {
                    string xmlContent = response.Content.ReadAsStringAsync().Result; // Synchronous read of response content
                    logger.Text += xmlContent;
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(xmlContent);

                    XmlNode valueNode = xmlDoc.SelectSingleNode("//LL[@value]");
                    if (valueNode != null && valueNode.Attributes["value"] != null)
                    {
                        return valueNode.Attributes["value"].Value.Trim();
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
        }

        static async Task<string> GetRedirectedUrlAsync(string initialUrl, string accessToken)
        {
            using (HttpClient client = new HttpClient(new HttpClientHandler { AllowAutoRedirect = false }))
            {

                // Set the Authorization header
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", accessToken);

                HttpResponseMessage response = await client.GetAsync(initialUrl);

                if (response.StatusCode == HttpStatusCode.RedirectKeepVerb)
                {
                    // Retrieve the redirection URL from the Location header
                    string finalUrl = response.Headers.Location.OriginalString;
                    return finalUrl;
                }

                return null; // Return null if there is no redirection
            }
        }

        static async Task<string> GetXmlDataAsync(string url, string accessToken)
        {
            using (HttpClient client = new HttpClient())
            {
                // Set the Authorization header
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", accessToken);

                string xmlContent = await client.GetStringAsync(url);

                return xmlContent;
            }
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
            List<string> shellVars = new List<string>();

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlData);

                XmlNodeList llNodes = xmlDoc.GetElementsByTagName("LL");

                foreach (XmlNode llNode in llNodes)
                {
                    if (llNode.Attributes["value"] != null)
                    {
                        string[] varNames = llNode.Attributes["value"].Value.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

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

                    // Find the corresponding TextBox control
                    Control textBoxControl = panel.Controls
                        .OfType<TextBox>()
                        .FirstOrDefault(textBox => textBox.Location.X == label.Location.X + label.Width + 20 && textBox.Location.Y == label.Location.Y);

                    if (textBoxControl is TextBox textBox)
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

        private void btnReboot_Click(object sender, EventArgs e)
        {
            CheckFinalURL();
            SendWebServiceRequest($"{httpPrefix}{finalHost}/dev/sys/wsdevice/{deviceSN}/Reboot", login, logger);
        }
        private void logger_TextChanged(object sender, EventArgs e)
        {
            // set the current caret position to the end
            logger.SelectionStart = logger.Text.Length;
            // scroll it automatically
            logger.ScrollToCaret();
        }

        private void nfcPlotBtn_Click(object sender, EventArgs e)
        {
            CheckFinalURL();
            Form b = new NFCPlot($"{httpPrefix}{finalHost}", deviceSN, userTextBox.Text, pwTextBox.Text);
            b.Show();
        }
    }
}
