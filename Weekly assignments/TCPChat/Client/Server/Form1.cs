using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Byte = System.Byte;
namespace Server
{
    public partial class Form1 : Form
    {
        private Thread listenThread;
        private TcpListener tcpListener;
        private bool stopChatServer = true;
        private readonly int _serverPort = 8888;
        private Dictionary<string, TcpClient> dict = new Dictionary<string, TcpClient>();
        private Thread replyThread;
        public const int BufferSize = 4096;
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
        public void Listen()
        {
            try
            {
                tcpListener = new TcpListener(new IPEndPoint(IPAddress.Parse(textBox1.Text), _serverPort));
                tcpListener.Start();

                while (!stopChatServer)
                {
                    //Application.DoEvents();
                    TcpClient _client = tcpListener.AcceptTcpClient();

                    StreamReader sr = new StreamReader(_client.GetStream());
                    StreamWriter sw = new StreamWriter(_client.GetStream());
                    sw.AutoFlush = true;
                    string username = sr.ReadLine();
                    if (string.IsNullOrEmpty(username))
                    {
                        sw.WriteLine("Please pick a username");
                        _client.Close();
                    }
                    else
                    {
                        if (!dict.ContainsKey(username))
                        {
                            Thread clientThread = new Thread(() => this.ClientRecv(username, _client));
                            dict.Add(username, _client);
                            clientThread.Start();
                        }
                        else
                        {
                            sw.WriteLine("Username already exist, pick another one");
                            _client.Close();
                        }
                    }

                }
            }
            catch (SocketException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void ClientRecv(string username, TcpClient tcpClient)
        {
            NetworkStream thisUserNetworkStream = tcpClient.GetStream();
            try
            {
                while (!stopChatServer)
                {
                    Application.DoEvents();

                    byte[] forwardBuffer = new byte[BufferSize];

                    while (thisUserNetworkStream.DataAvailable)
                    {
                        thisUserNetworkStream.Read(forwardBuffer, 0, BufferSize);
                    }

                    string headerAndMessage = Encoding.UTF8.GetString(forwardBuffer);
                    string[] arrPayload = headerAndMessage.Split(';');
                    if (arrPayload.Length >= 3)
                    {
                        string friendUsername = arrPayload[0];
                        MessageType msgType = (MessageType)Enum.Parse(typeof(MessageType), arrPayload[1], true);
                        if (msgType == MessageType.Text)
                        {
                            string content = arrPayload[2].Replace("\0", string.Empty);
                            string forwardHeaderAndMessage = $"{username};{MessageType.Text};{content}";
                            if (dict.TryGetValue(friendUsername, out TcpClient friendTcpClient))
                            {
                                StreamWriter sw = new StreamWriter(friendTcpClient.GetStream());
                                sw.WriteLine(forwardHeaderAndMessage);
                                sw.AutoFlush = true;
                            }

                            StreamWriter sw2 = new StreamWriter(tcpClient.GetStream());
                            sw2.WriteLine(forwardHeaderAndMessage);
                            sw2.AutoFlush = true;
                            UpdateChatHistoryThreadSafe(forwardHeaderAndMessage);
                        }
                        else if (msgType == MessageType.FilePart || msgType == MessageType.FileEof)
                        {
                            //string content = arrPayload[2].Replace("\0", string.Empty);
                            if (dict.TryGetValue(friendUsername, out TcpClient friendTcpClient))
                            {
                                //                                StreamWriter sw = new StreamWriter(friendTcpClient.GetStream());
                                NetworkStream ns = friendTcpClient.GetStream();
                                byte[] forwardBytes = Encoding.UTF8.GetBytes(headerAndMessage);
                                ns.Write(forwardBytes, 0, forwardBytes.Length);

                            }
                            //
                        }

                    }
                }
            }
            catch (SocketException sockEx)
            {
                tcpClient.Close();
                //                sr.Close();

            }

        }
        private delegate void SafeCallDelegate(string text);

        private void UpdateChatHistoryThreadSafe(string text)
        {
            if (richTextBox1.InvokeRequired)
            {
                var d = new SafeCallDelegate(UpdateChatHistoryThreadSafe);
                richTextBox1.Invoke(d, new object[] { text });
            }
            else
            {
                richTextBox1.Text += text;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (stopChatServer)
            {
                stopChatServer = false;
                listenThread = new Thread(this.Listen);
                listenThread.Start();
                MessageBox.Show(@"Start listening for incoming connections");
                button1.Text = @"Stop";
            }
            else
            {
                stopChatServer = true;
                button1.Text = @"Start listening";
                tcpListener.Stop();
                listenThread = null;

            }
        }
    }
}
