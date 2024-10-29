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
        private readonly List<ClientObject> clients = new List<ClientObject>();//tạo danh sách client quản lý client đang kết nối 

        //thêm kết nối
        protected internal void AddConnection(ClientObject clientObject)
        {
            //thêm 1 client vào danh sách clients hiện có
            clients.Add(clientObject);
        }
        //xóa kết nối
        protected internal void RemoveConnection(string id)
        {
            if (clients == null) return; // Kiểm tra null cho danh sách clients
                var client = clients.Find(c => c.Id == id); //tìm kiến id của client trong danh sách
                if (client != null)
                {

                    clients.Remove(client); // Xóa client khỏi danh sách
                }
        }
        //lắng nghe từ client
        protected internal void Listen_Client()
        {
            try
            {
                //Tạo kết nối với sever
                serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                serverSocket.Bind(new IPEndPoint(IPAddress.Any, 11000));
                serverSocket.Listen(20);
                Program.f.tbLog.Invoke((MethodInvoker)delegate
                {
                    Program.f.tbLog.Text += "[" + DateTime.Now + "] " 
                                            + "Server is turn on, waiting for connection......." 
                                            + Environment.NewLine;
                });
                while (true) //lặp liên tục để tiếp nhận yêu cầu từ client
                {
                    Socket handler = serverSocket.Accept();//Tạo một kết nối clientObject mới
                    ClientObject clientObject = new ClientObject(handler, this); //constructor khởi tạo của ClienObject
                    Thread clientThread = new Thread(new ThreadStart(clientObject.Process));// Tạo luồng riêng cho ClientObject để nhận tin nhắn
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