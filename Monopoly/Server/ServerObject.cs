using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Server
{
    public class ServerObject
    {
        private Socket serverSocket;
        private readonly List<ClientObject> clients = new List<ClientObject>();
        
        //thêm kết nối
        protected internal void AddConnection(ClientObject clientObject)
        {
            clients.Add(clientObject);
        }
        //xóa kết nối
        protected internal void RemoveConnection(string id)
        {
            var client = clients.Find(c => c.Id == id);
            if (client != null)
                clients.Remove(client);
        }
        //lắng nghe từ client
        protected internal void Listen_Client()
        {
            try
            {
                //Tạo kết nối với sever
                serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                serverSocket.Bind(new IPEndPoint(IPAddress.Any, 8888));
                serverSocket.Listen(100);
                Program.f.tbLog.Invoke((MethodInvoker)delegate
                {
                    Program.f.tbLog.Text += "[" + DateTime.Now + "] " 
                                            + "Server đã được bật, chờ đợi người chơi vào..." 
                                            + Environment.NewLine;
                });
                while (true)
                {
                    //Tiếp nhận các client 
                    Socket handler = serverSocket.Accept();
                    //Tạo một clientObject mới
                    ClientObject clientObject = new ClientObject(handler, this);
                    // Tạo luồng riêng cho ClientObject
                    Thread clientThread = new Thread(new ThreadStart(clientObject.Process));
                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                Program.f.tbLog.Invoke((MethodInvoker)delegate
                {
                    Program.f.tbLog.Text += "[" + DateTime.Now + "] " + 
                                            ex.Message + 
                                            Environment.NewLine;
                });
            }
        }
        //gửi tin nhắn đến đối thủ
        protected internal void SendMessageToOpponentClient(string message, string id)
        {
            foreach (var client in clients.Where(c => c.Id != id))
            {
                byte[] data = Encoding.Unicode.GetBytes(message);
                client.Client.Send(data);
            }
        }
        //gửi tin nhắn đến chính người gửi, dùng trong trường hợp cần cập nhật thông tin
        protected internal void SendMessageToSender(string message, string id)
        {
            foreach (var client in clients.Where(c => c.Id == id))
            {
                byte[] data = Encoding.Unicode.GetBytes(message);
                client.Client.Send(data);
            }
        }
        //gửi tin đến mọi người
        protected internal void SendMessageToEveryone(string message, string id)
        {
            foreach (var client in clients)
            {
                byte[] data = Encoding.Unicode.GetBytes(message);
                client.Client.Send(data);
            }
        }
        //Ngắt kết nối sever
        protected internal void Disconnect()
        {
            serverSocket.Close();
            foreach (var client in clients)
                client.Client.Close();
            Environment.Exit(0);
        }
    }
}