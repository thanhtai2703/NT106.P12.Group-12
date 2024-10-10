using System;
using System.Data.Odbc;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace LMCB_TestForm
{
    public partial class Form1 : Form
    {

        private TcpClient tcpClient;
        private StreamReader sReader;
        private StreamWriter sWriter;
        private Thread clientThread;
        private int serverPort = 8000;
        private bool stopTcpClient = true;
        public const int BufferSize = 4096;
        public const int FileBufferSize = 3072;
        private string SaveFileName = string.Empty;
        private MemoryStream fileSaveMemoryStream;
        public enum
            MessageType
        {
            Text,
            FileEof,
            FilePart,
        }
        public Form1()
        {
            InitializeComponent();

        }

        [STAThread]
        private void ClientRecv()
        {
            try
            {
                NetworkStream networkStream = tcpClient.GetStream();
                byte[] readBuffers = new byte[BufferSize];
                networkStream.BeginRead(readBuffers, 0, BufferSize, new AsyncCallback(OnReceive), readBuffers);
            }
            catch (SocketException sockEx)
            {
                tcpClient.Close();
                MessageBox.Show("Socket error: " + sockEx.Message);
            }
        }
        private void OnReceive(IAsyncResult ar)
        {
            try
            {
                NetworkStream networkStream = tcpClient.GetStream();
                byte[] readBuffers = (byte[])ar.AsyncState;

                int bytesRead = networkStream.EndRead(ar);

                if (bytesRead > 0)
                {
                    string headerAndMessage = Encoding.UTF8.GetString(readBuffers, 0, bytesRead).Replace("\0", string.Empty);
                    string[] arrPayload = headerAndMessage.Split(';');
                    if (arrPayload.Length >= 3)
                    {
                        string senderUsername = arrPayload[0];
                        MessageType msgType = (MessageType)Enum.Parse(typeof(MessageType), arrPayload[1], true);

                        if (msgType == MessageType.Text)
                        {
                            string content = arrPayload[2];
                            string formattedMsg = $"{senderUsername}: {content}\n";
                            UpdateChatHistoryThreadSafe(formattedMsg);
                        }
                        else if (msgType == MessageType.FilePart)
                        {
                            HandleFilePart(arrPayload[2]);
                        }
                        else if (msgType == MessageType.FileEof)
                        {
                            HandleFileEof(arrPayload[2]);
                        }
                    }

                    networkStream.BeginRead(readBuffers, 0, BufferSize, new AsyncCallback(OnReceive), readBuffers);
                }
                else
                {
                    tcpClient.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void HandleFilePart(string filePartData)
        {
            byte[] filePart = Encoding.UTF8.GetBytes(filePartData.Replace("\0", string.Empty));
            fileSaveMemoryStream?.Write(filePart, 0, filePart.Length);
        }

        private void HandleFileEof(string finalFilePartData)
        {
            byte[] finalFilePart = Encoding.UTF8.GetBytes(finalFilePartData.Replace("\0", string.Empty));

            if (fileSaveMemoryStream != null)
            {
                fileSaveMemoryStream.Write(finalFilePart, 0, finalFilePart.Length);
                using (FileStream fs = File.OpenWrite(SaveFileName))
                {
                    fileSaveMemoryStream.WriteTo(fs);
                }
            }
            else
            {
                using (FileStream fs = File.OpenWrite(SaveFileName))
                {
                    fs.Write(finalFilePart, 0, finalFilePart.Length);
                }
            }

            fileSaveMemoryStream = null;
            SaveFileName = null;
        }

        private delegate void SafeCallDelegate(string text);

        private delegate void SaveFileConfirmDialogDelegate(DialogResult result);
        private void UpdateChatHistoryThreadSafe(string text)
        {
            if (richTextBox1.InvokeRequired)
            {
                var d = new SafeCallDelegate(UpdateChatHistoryThreadSafe);
                richTextBox1.Invoke(d, new object[] {text});
            }
            else
            {
                richTextBox1.Text += text;
            }
        }
      

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                NetworkStream ns = tcpClient.GetStream();
                string allInOneMsg = $"{textBox3.Text};{MessageType.Text.ToString()};{sendMsgTextBox.Text}";
                byte[] sendingBytes = Encoding.UTF8.GetBytes(allInOneMsg);
                ns.Write(sendingBytes, 0, sendingBytes.Length);
                sendMsgTextBox.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Form1_Close(object sender, FormClosingEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                stopTcpClient = false;

                this.tcpClient = new TcpClient();
                this.tcpClient.Connect(new IPEndPoint(IPAddress.Parse(textBox2.Text), serverPort));
                this.sWriter = new StreamWriter(tcpClient.GetStream())
                {
                    AutoFlush = true
                };
                sWriter.WriteLine(this.textBox1.Text);
                clientThread = new Thread(this.ClientRecv);
                clientThread.Start();
                MessageBox.Show("Connected");
            }
            catch (SocketException sockEx)
            {
                MessageBox.Show(sockEx.Message, "Network error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fileContent = string.Empty;
            string filePath = string.Empty;
            byte[] sendingBuffer = null;
            try
            {
                NetworkStream networkStream = tcpClient.GetStream();
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.InitialDirectory = "c:\\";
                    openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                    openFileDialog.FilterIndex = 2;
                    openFileDialog.RestoreDirectory = true;

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {

                        byte[] headerBytes = Encoding.UTF8.GetBytes($"{textBox3.Text};{MessageType.FilePart};");
                        using (Stream fileStream = openFileDialog.OpenFile())
                        {
                            int NoOfPackets = Convert.ToInt32
                                (Math.Ceiling(Convert.ToDouble(fileStream.Length) / Convert.ToDouble(FileBufferSize)));

                            progressBar1.Maximum = NoOfPackets;
                            int TotalLength = (int) fileStream.Length, CurrentPacketLength, counter = 0;
                            for (int i = 0; i < NoOfPackets; i++)
                            {
                                if (TotalLength > FileBufferSize)
                                {
                                    CurrentPacketLength = FileBufferSize;
                                    TotalLength = TotalLength - CurrentPacketLength;
                                }
                                else
                                {
                                    CurrentPacketLength = TotalLength;
                                    headerBytes = Encoding.UTF8.GetBytes($"{textBox3.Text};{MessageType.FileEof};");

                                }

                                sendingBuffer = new byte[CurrentPacketLength];
                                fileStream.Read(sendingBuffer, 0, CurrentPacketLength);

                                byte[] sendingBytes = headerBytes.Concat(sendingBuffer).ToArray();
                                networkStream.Write(sendingBytes, 0, (int) sendingBytes.Length);
                                if (progressBar1.Value >= progressBar1.Maximum)
                                    progressBar1.Value = progressBar1.Minimum;
                                progressBar1.PerformStep();
                            }


                        }


                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
