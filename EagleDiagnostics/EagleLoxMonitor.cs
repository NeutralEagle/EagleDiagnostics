using System.Data;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Windows.Forms;


namespace EagleDiagnostics
{


    public partial class EagleLoxMonitor : Form
    {
        private const int Port = 7777;
        private bool refreshFlag = true;
        private byte[]? monitorFile;
        private static readonly string autoSaveFolder = Directory.GetCurrentDirectory() + "\\autosave\\";
        private readonly string autoSaveFile = "autosave";
        private readonly string autoSaveExtension = ".txt";
        private int autoSaveID;
        private bool loadFlag = false;
        private bool formClosing = false;
        int pps = 0;
        int ppsTotal = 0;
        private readonly string regexPattern = @"\0\u001f.*?\0\0\u0001";
        readonly Thread UDPthread;
        UdpClient? udpClient = null;
        public EagleLoxMonitor()
        {
            //Lines = GetLines();
            InitializeComponent();
            autoSaveTimer.Interval = 300000; //autosave every 5min
            autoSaveTimer.Start();
            ppsTimer.Interval = 1000;
            ppsTimer.Start();
            Show();

            UDPthread = new Thread(UDP_setup)
            {
                IsBackground = true
            };
            try
            {
                UDPthread.Start();
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        /*public List<DebugLine> Lines { get; set; }
        private List<DebugLine> GetLines()
        {
            var list = new List<DebugLine>();
            list.Add(new DebugLine()
            { 
                header = 
            }
        }*/

        public void UDP_setup()
        {

            try
            {
                udpClient = new UdpClient(Port);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while opening UDP socket: \n" + ex.Message);
                Invoke((MethodInvoker)(() => receiveButton.Enabled = true));
                return;
            }
            IPEndPoint iPEndPoint = new(IPAddress.Any, Port);

            while (!formClosing)
            {
                try
                {
                    if (!loadFlag) FillData(udpClient.Receive(ref iPEndPoint), true);
                }
                catch { }

            }



        }

        public void FillData(byte[] data, bool arr)
        {
            if (arr)
            {
                int indexOfNull;

                if (data[2] == '\xa0') return;//NYI - unknown message
                if (data[2] == '\xa4') return;//NYI - unknown message
                if (data[2] == '\x14') return;//NYI - unknown message
                if (data[2] == '\x9c') return;//NYI - unknown message

                data = data.Skip(2).Take(data.Length - 4).ToArray();

                if (data.Length >= 27)
                    if (data[24] == 0 && data[25] == 0 && data[26] == 1)
                    {
                        pps++;
                        short millisec = BitConverter.ToInt16(data, 8);
                        int time = BitConverter.ToInt32(data, 10);
                        IPAddress address = new(data.Skip(14).Take(4).ToArray());
                        string IP = address.ToString();
                        byte spacer = 32;
                        int headerEnd = Array.IndexOf(data, spacer, 27);
                        if (headerEnd != -1)
                        {

                            string str = Encoding.UTF8.GetString(data, headerEnd, data.Length - 3 - headerEnd);
                            string header = Encoding.UTF8.GetString(data, 27, headerEnd - 26);
                            str = Regex.Replace(str, regexPattern, "\r\n");
                            indexOfNull = str.IndexOf("\0\u001f\x001f");
                            /*
                            StringBuilder sb = new StringBuilder();
                            foreach (var a in data.Take(8).ToArray())
                            {
                                sb.Append(a.ToString().PadLeft(3, '0') + ", ");

                            }
                            string bytes0_8 = sb.ToString();*/
                            int messageNr = BitConverter.ToUInt16(data, 6);
                            /*
                            sb.Clear();
                            foreach (var b in data.Skip(17).Take(6).ToArray())
                            {
                                sb.Append(b.ToString().PadLeft(3, '0') + ", ");

                            }
                            string bytes18_23 = sb.ToString();
                            */

                            //Invoke((MethodInvoker)(() => mainListBox.Items.Add($"{bytes0_8}{messageNr.ToString().PadLeft(5, '0')}   {LoxTimeStampToDateTime(time)}.{millisec.ToString().PadLeft(3, '0')};    {IP}    {bytes18_23}        {header} {str}")));
                            if (formClosing)
                                return;
                            Invoke((MethodInvoker)(() => mainListBox.Items.Add($"{messageNr,5}  {LoxTimeStampToDateTime(time)}.{millisec.ToString().PadLeft(3, '0')};  {IP,-16}{header}{str}")));

                            if (refreshFlag == true) Invoke((MethodInvoker)(() => mainListBox.TopIndex = mainListBox.Items.Count - 1));

                        }
                    }



            }
            else
            {

                var stringFile = Encoding.UTF8.GetString(data).Split("\r\n");
                DisplayProgressBarMessageBox(stringFile);
                /*
                Invoke((MethodInvoker)(() => mainListBox.BeginUpdate()));
                foreach (var line in stringFile) { Invoke((MethodInvoker)(() => mainListBox.Items.Add(line))); }
                Invoke((MethodInvoker)(() => mainListBox.EndUpdate()));*/
            }
        }
        private static DateTime LoxTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dateTime = new(2009, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }
        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (refreshFlag != true)
            {
                refreshFlag = true;

            }
            else
            {
                refreshFlag = false;

            }
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control == true && e.KeyCode == Keys.C)
            {
                Clipboard.SetText(string.Join(Environment.NewLine, mainListBox.SelectedItems.OfType<string>()));


            }
            // TO FIX: Crashes
            /*
            if (e.Control == true && e.KeyCode == Keys.A)
            {
                Invoke((MethodInvoker)(() => mainListBox.BeginUpdate()));
                foreach (var item in mainListBox.Items)
                {
                    Invoke((MethodInvoker)(() => mainListBox.SelectedItems.Add(item)));
                }
                Invoke((MethodInvoker)(() => mainListBox.EndUpdate()));
            }*/
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
            string savepath = saveFileDialog1.FileName;
            if (savepath != "") File.WriteAllLines(savepath, mainListBox.Items.Cast<string>().ToArray());
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
            File.WriteAllLines(savepath, mainListBox.Items.Cast<string>().ToArray());
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => mainListBox.Items.Clear()));
            loadFlag = true;
            receiveButton.Enabled = true;
            Invoke((MethodInvoker)(() => mainListBox.BeginUpdate()));
            var a = openFileDialog1.ShowDialog();
            if (a == DialogResult.OK)
                try
                {
                    monitorFile = File.ReadAllBytes(openFileDialog1.FileName);

                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            if (monitorFile == null) return;
            if (monitorFile.Length < 30) return;
            if (monitorFile[1] < 30)
            {
                /*
                byte[] buffer;
                do
                {
                    buffer = monitorFile.SkipWhile(x => x != '\x1F').ToArray();
                    monitorFile = buffer;
                    buffer = buffer.TakeWhile(x => x != '\xF1').ToArray();
                    FillData(buffer, true);
                } while (buffer.Length > 26);*/
                byte[] startBytes = { 0x1F, 0xFA };
                byte[] endBytes = { 0x1F, 0x1F };

                bool insideMessage = false;
                MemoryStream messageStream = new();

                for (int i = 0; i < monitorFile.Length; i++)
                {
                    byte currentByte = monitorFile[i];

                    if (!insideMessage && currentByte == startBytes[0])
                    {
                        if (i + 1 < monitorFile.Length && monitorFile[i + 1] == startBytes[1])
                        {
                            insideMessage = true;
                            messageStream.WriteByte(currentByte);
                            messageStream.WriteByte(monitorFile[i + 1]);
                            i++;
                        }
                    }
                    else if (insideMessage)
                    {
                        messageStream.WriteByte(currentByte);

                        if (currentByte == endBytes[1] && messageStream.Length >= 2)
                        {
                            byte[] messageBytes = messageStream.ToArray();
                            FillData(messageBytes, true);

                            insideMessage = false;
                            messageStream = new MemoryStream();
                        }
                    }
                }
                Invoke((MethodInvoker)(() => mainListBox.EndUpdate()));
            }
            else
            {
                FillData(monitorFile, false);
            }
        }

        private void ReceiveButton_Click(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => mainListBox.Items.Clear()));
            loadFlag = false;
            receiveButton.Enabled = false;
            Thread UDPthread = new(UDP_setup);
            try
            {
                UDPthread.Start();
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        private void PpsTimer_Tick(object sender, EventArgs e)
        {
            ppsTotal++;
            ppsLabel.Text = (pps / ppsTotal).ToString();
            totalPacketLabel.Text = pps.ToString();

        }
        void EagleLoxMonitor_FormClosing(object sender, FormClosingEventArgs e)
        {
            udpClient?.Close();
            udpClient?.Dispose();
            formClosing = true;
        }

        void DisplayProgressBarMessageBox(string[] stringFile)
        {

            mainProgressBar.Maximum = stringFile.Length;
            // Perform the time-consuming operation
            // Your code here
            foreach (var line in stringFile)
            {
                Invoke((MethodInvoker)(() => mainListBox.Items.Add(line)));
                mainProgressBar.Value++;
            }
            mainProgressBar.Value = 0;

        }
    }
}
