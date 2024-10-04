using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Server
{
    public class ClientObject
    {
        protected internal Socket Client;
        private readonly ServerObject server;
        private string userName;
        private StreamWriter write;
        DateTime now = DateTime.Now;

        List<(string, int)> o = new List<(string, int)>
            {
                ("GO", 0),
                ("Phú Lâm", 1),
                ("Khí vận", 2),
                ("Nhà bè Phú Xuân", 3),
                ("Thuế lợi tức", 4),
                ("Bến xe Lục Tỉnh", 5),
                ("Thị Nghè", 6),
                ("Cơ hội", 7),
                ("Tân Định", 8),
                ("Bến Chương Dương", 9),
                ("Thăm tù", 10),
                ("Phan Đình Phùng", 11),
                ("Công ty điện lực", 12),
                ("Trịnh Minh Thế", 13),
                ("Lý Thái Tổ", 14),
                ("Bến xe Lam Chợ Lớn", 15),
                ("Đại lộ Hùng Vương", 16),
                ("Khí vận", 17),
                ("Gia Long", 18),
                ("Bến Bạch Đằng", 19),
                ("Sân bay", 20),
                ("Đường Công Lý", 21),
                ("Cơ hội", 22),
                ("Đại lộ thống nhất", 23),
                ("Đại lộ Cộng Hòa", 24),
                ("Bến xe An Đông", 25),
                ("Đại lộ Hồng Thập Tự", 26),
                ("Đại lộ Hai Bà Trưng", 27),
                ("Công ty thủy cục", 28),
                ("Xa lộ Biên Hòa", 29),
                ("VÔ TÙ", 30),
                ("Phan Thanh Giảm", 31),
                ("Lê Văn Duyệt", 32),
                ("Khí vận", 33),
                ("Nguyễn Thái Học", 34),
                ("Tân Kì Tân Quý", 35),
                ("Cơ hội", 36),
                ("Nha Trang", 37),
                ("Thuế lương bổng", 38),
                ("Cố Đô Huế", 39)
            };
       
        public string FindNameByNumber(int number)
            {
                foreach (var pair in o)
                {
                    if (pair.Item2 == number)
                    {
                        return pair.Item1; // Trả về tên khi tìm thấy số
                    }
                }
                // Trả về null nếu không tìm thấy
                return null; 
            }
        public ClientObject(Socket socket, ServerObject serverObject)
        {
            Id = Guid.NewGuid().ToString();
            Client = socket;
            server = serverObject;
            serverObject.AddConnection(this);
        }
        protected internal string Id { get; }
        public void Process()
        {
            try
            {
                while (true)
                {
                    string message = GetMessage();
                    string []arraypayload = message.Split(';');
                    // Nhận được thông điệp cả 2 người chơi đã kết nối
                    //if (Regex.IsMatch(message, @"Cả\s+2\s+người\s+chơi\s+đã\s+kết\s+nối:\s+\d+"))
                    if (message.Contains("Cả 2 người chơi đã kết nối: "))
                    {
                        server.SendMessageToEveryone(message, Id);
                        Program.f.tbLog.Invoke((MethodInvoker)delegate
                        {
                            Program.f.tbLog.Text += "[" + DateTime.Now + "] " + message + Environment.NewLine;
                            UpdateToFile("[" + DateTime.Now + "] " + message);
                        });
                    }
                    //Nhận được thông điệp đỏ đã kết nối
                    else if (Regex.IsMatch(message, @"Đỏ\s*\(\s*(\d+)\s*\)") 
                        && !message.Contains(" đã rời") 
                        && !message.Contains("thắng") 
                        && !message.Contains("thua"))
                    {
                        userName = message;
                        Program.f.tbLog.Invoke((MethodInvoker)delegate
                        {
                            Program.f.tbLog.Text += "[" + DateTime.Now + "] " + userName + " đã kết nối" + Environment.NewLine;
                            UpdateToFile("[" + DateTime.Now + "] " + userName + " đã kết nối");
                        });
                        server.SendMessageToOpponentClient(userName + " đã kết nối", Id);
                    }
                    //Nhân được thông điệp xanh đã kết nối
                    else if (Regex.IsMatch(message, @"Xanh\s*\(\s*(\d+)\s*\)") 
                        && !message.Contains(" đã rời")
                        && !message.Contains("thắng")
                        && !message.Contains("thua"))
                    {
                        userName = message;
                        Program.f.tbLog.Invoke((MethodInvoker)delegate
                        {
                            Program.f.tbLog.Text += "[" + DateTime.Now + "] " + userName + " đã kết nối" + Environment.NewLine;
                            UpdateToFile("[" + DateTime.Now + "] " + userName + " đã kết nối");
                        });
                        server.SendMessageToOpponentClient(userName + " đã kết nối", Id);
                    }
                    //Nhận được thông điệp rời đi của client
                    else if (message.Contains(" đã rời"))
                    {
                        if (!message.Contains(" đã rời."))
                            server.SendMessageToOpponentClient(message, Id);
                        Program.f.tbLog.Invoke((MethodInvoker)delegate
                        {
                            Program.f.tbLog.Text += "[" + DateTime.Now + "] " + message + Environment.NewLine;
                            UpdateToFile("[" + DateTime.Now + "] " + message);
                        });
                        server.RemoveConnection(this.Id);
                        break;
                    }
                    //Nhận được tin nhắn của client và procast đến tất cả client còn lại
                    else if (message.Contains(" nhắn: "))
                    {
                        Program.f.tbLog.Invoke((MethodInvoker)delegate
                        {
                            Program.f.tbLog.Text += "[" + DateTime.Now + "] " + userName + message + Environment.NewLine;
                            UpdateToFile("[" + DateTime.Now + "] " + userName + message);
                        });
                        server.SendMessageToEveryone(userName + message, Id);
                    }
                    //Nhận được thông điệp kết thúc lượt và các thông số lượt đi trước của đỏ
                    else if (message.Contains("Kết quả lượt đi của Đỏ"))
                    {   
                        server.SendMessageToOpponentClient(message, Id);
                        Program.f.tbLog.Invoke((MethodInvoker)delegate
                        {
                            string tempMessage = message;
                            tempMessage = tempMessage.Replace("Kết quả lượt đi của Đỏ", "");
                            string[] data = tempMessage.Split('~');
                            int STT = Convert.ToInt32(data[1]);
                            string vitri = FindNameByNumber(STT);
                            Program.f.tbLog.Text += "[" + DateTime.Now + "] " + userName
                                                + "\tVị trí: " + vitri
                                                + "\tTiền: " + data[2] + Environment.NewLine;
                            UpdateToFile("[" + DateTime.Now + "] " + userName
                                                + "\tVị trí: " + vitri
                                                + "\tTiền: " + data[2]);
                            Program.f.tbLog.Text += "[" + DateTime.Now + "] " + "Đến lượt của Xanh" + Environment.NewLine;
                            UpdateToFile("[" + DateTime.Now + "] " + "Đến lượt của Xanh");
                        });
                        
                    }
                    //Nhận được thông điệp kết thúc lượt và các thông số lượt đi trước của xanh
                    else if (message.Contains("Kết quả lượt đi của Xanh"))
                    {   
                        server.SendMessageToOpponentClient(message, Id);
                        Program.f.tbLog.Invoke((MethodInvoker)delegate
                        {
                            string tempMessage = message;
                            tempMessage = tempMessage.Replace("Kết quả lượt đi của Xanh", "");
                            string[] data = tempMessage.Split('~');
                            int STT = Convert.ToInt32(data[1]);
                            string vitri = FindNameByNumber(STT);
                            Program.f.tbLog.Text += "[" + DateTime.Now + "] " + userName
                                                 + "\tVị trí: " + vitri
                                                 + "\tTiền: " + data[2] + Environment.NewLine;
                            UpdateToFile("[" + DateTime.Now + "] " + userName
                                                + "\tVị trí: " + vitri
                                                + "\tTiền: " + data[2]);
                            Program.f.tbLog.Text += "[" + DateTime.Now + "] " + "Đến lượt của Đỏ" + Environment.NewLine;
                            UpdateToFile("[" + DateTime.Now + "] " + "Đến lượt của Đỏ");
                        });
                        
                    }
                    //Nhận được thông điệp người chơi chịu mức thuế từ đối phương và số tiền thuế
                    else if (message.Contains("thuê"))
                    {
                        Program.f.tbLog.Invoke((MethodInvoker)delegate
                        {
                            Program.f.tbLog.Text += "[" + DateTime.Now + "] " + message + Environment.NewLine;
                            UpdateToFile("[" + DateTime.Now + "] " + message);
                        });
                        server.SendMessageToOpponentClient(message, Id);
                    }
                    else if (message.Contains("thắng.") || message.Contains("thua.") || message == "Người chơi mới đã vào") {
                        Program.f.tbLog.Invoke((MethodInvoker)delegate
                        {
                            Program.f.tbLog.Text += "[" + DateTime.Now + "] " + message + Environment.NewLine;
                            UpdateToFile("[" + DateTime.Now + "] " + message);
                        });
                    }
                }
            }
            catch (Exception e)
            {
                Program.f.tbLog.Invoke((MethodInvoker)delegate
                {
                    Program.f.tbLog.Text += "[" + DateTime.Now + "] " + e.Message + Environment.NewLine;
                });
            }
        }
        private string GetMessage()
        {
            byte[] data = new byte[256];
            StringBuilder builder = new StringBuilder();
            do
            {
                int bytes = Client.Receive(data);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            } while (Client.Available > 0);

            return builder.ToString();
        }
        private void UpdateToFile(string data) 
        {
            if (write == null)
                write = new StreamWriter("Data\\" + userName + " " + $"Data_{now:yyyyMMdd_HHmmss}.txt");
            write.WriteLine(data);
            write.Flush();
        }
        protected internal void Close()
        {
            Client.Shutdown(SocketShutdown.Both);
            Client.Close();
        }
    }
}



