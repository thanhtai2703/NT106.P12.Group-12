using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static Server.Program;

namespace Server
{
    public class ClientObject
    {
        protected internal Socket Client;
        private readonly ServerObject server;
        private string userName; // tên người gửi thông điệp từ client.
       // private int currentTurn;
        private StreamWriter write;
        static List<Room> room = new List<Room>(); // khời tạo 1 danh sách phòng chơi trên server để quản lý các phòng chơi hiện có.
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
            Id = Guid.NewGuid().ToString(); //khởi tạo ID cho client
            Client = socket;
            server = serverObject;
            serverObject.AddConnection(this);//Thêm kết nối client vào server
        }
        protected internal string Id { get; }
        //Xử lý tin nhắn gửi lên từ client
        public void Process()
        {
            try
            {
                while (true)
                {
                    string message = GetMessage(); //Tạo biến tạm để xử lý tin nhắn
                    string []arraypayload = message.Split(';'); // tách chuỗi tin nhắn theo dấu ";"
                    switch (arraypayload[0]) // đọc thông điệp control message
                    {
                        case "Kết nối":
                            bool isFound = false; // tạo biến kiểm tra phòng đã tồn tại chưa
                            for (int i = 0; i < room.Count; i++) //duyệt danh sách phòng hiện có để tìm kiếm
                            {
                                if (room[i].roomId == Convert.ToInt32(arraypayload[2])) // nếu phòng đã tồn tại
                                {
                                    isFound = true; // Tìm thấy
                                    if (arraypayload[1] == "Đỏ") // cập nhật người chơi chọn quân cờ đỏ.
                                    {
                                        room[i].roomTaken.Red = true; //Biến takenRed đánh dấu thể hiện quân đỏ đã được chọn.
                                        room[i].roomPlayer.Name1 = arraypayload[3]; // gán tên người chơi.
                                    }
                                    if (arraypayload[1] == "Xanh") // cập nhật người chơi chọn quân xanh.
                                    {
                                        room[i].roomTaken.Blue = true; //tương tự
                                        room[i].roomPlayer.Name2 = arraypayload[3];//tương tự
                                    }
                                    if (room[i].roomTaken.Red == true && room[i].roomTaken.Blue == true) // kiểm tra nếu cả 2 quân cờ đã được chọn, tức là phòng đã đủ người
                                    {
                                        //Gửi thông điệp đến tất cả client trong phòng.
                                        server.SendMessageToEveryone("Cập nhật" + ";" + room[i].roomId + ";" + room[i].roomPlayer.Name1 + ";" + room[i].roomPlayer.Name2 + ";", Id);
                                    }
                                    break;
                                }
                                //if (room[i].roomTaken.Red == true && room[i].roomTaken.Blue == true)
                                //{
                                //    server.SendMessageToEveryone("Cập nhật" + ";" + room[i].roomId +";" + room[i].roomPlayer.Name1 + ";" + room[i].roomPlayer.Name2 + ";", Id);//cập nhật + phòng + teen1 + tên 2
                                //}
                            }
                            //Trường hợp phòng chưa tồn tại
                            if(!isFound)
                            {
                                Room a = new Room(); // tạo một đối tượng room mới
                                //Tiến hành gán các giá trị
                                a.roomId = Convert.ToInt32(arraypayload[2]); 
                                if (arraypayload[1] == "Đỏ")
                                {
                                    a.roomTaken.Blue = false;
                                    a.roomTaken.Red = true;
                                    a.roomPlayer.Name1 = arraypayload[3];
                                    a.roomPlayer.Name2 = "";
                                }
                                if (arraypayload[1] == "Xanh")
                                {
                                    a.roomTaken.Red = false;
                                    a.roomTaken.Blue = true;
                                    a.roomPlayer.Name2 = arraypayload[3];
                                    a.roomPlayer.Name1 = "";
                                }
                                room.Add(a); //Thêm phòng mới vào danh sách phòng hiện tại
                            }    
                                userName = arraypayload[1];
                                Program.f.tbLog.Invoke((MethodInvoker)delegate
                                {
                                    Program.f.tbLog.Text += "[" + DateTime.Now + "] " + userName + " đã kết nối" + Environment.NewLine;
                                    UpdateToFile("[" + DateTime.Now + "] " + userName + " đã kết nối");
                                });
                                //if (Taken.Red == true && Taken.Blue == true)
                                //{
                                //    server.SendMessageToEveryone("Cập nhật" + ";" + Player.Name1 + ";" + Player.Name2 + ";", Id);
                                //}
                                server.SendMessageToOpponentClient(message, Id);
                                //server.SendMessageToSender(message,Id);
                            
                            break;
                        case "Bắt đầu":
                            //Gửi thông điệp bắt đầu đến client, bắt đầu trò chơi
                            server.SendMessageToEveryone(message, Id);
                            Program.f.tbLog.Invoke((MethodInvoker)delegate
                            {
                                Program.f.tbLog.Text += "[" + DateTime.Now + "] " + message + Environment.NewLine;
                                UpdateToFile("[" + DateTime.Now + "] " + message);
                            });
                            break;
                        case "Rời":
                            //Sự kiện Ngắt kết nối khỏi phòng chơi
                            server.SendMessageToOpponentClient(message, Id);
                            Program.f.tbLog.Invoke((MethodInvoker)delegate
                            {
                                Program.f.tbLog.Text += "[" + DateTime.Now + "] " + message + Environment.NewLine;
                                UpdateToFile("[" + DateTime.Now + "] " + message);
                            });
                            server.RemoveConnection(this.Id);
                            RemoveRoom(Convert.ToInt32(arraypayload[2]));
                            break;
                        case "Thoát":
                            server.RemoveConnection(this.Id);
                            break;
                        case "Kết quả":
                            //Cập nhật thông tin lượt đi của người chơi vừa kết thúc lượt, gửi qua client đối thủ.
                            server.SendMessageToOpponentClient(message, Id);
                            Program.f.tbLog.Invoke((MethodInvoker)delegate
                            {
                                string tempMessage = message;
                                string nextPlayerId = arraypayload[3];
                                //tempMessage = tempMessage.Replace("Kết quả lượt đi của Đỏ", "");
                                //string[] data = arraypayload[4].Split('~');
                                //int STT = Convert.ToInt32(data[1]);
                                //string vitri = FindNameByNumber(STT);
                                //Program.f.tbLog.Text += "[" + DateTime.Now + "] " + userName
                                //                    + "\tVị trí: " + vitri
                                //                    + "\tTiền: " + data[2] + Environment.NewLine;
                                //UpdateToFile("[" + DateTime.Now + "] " + userName
                                //                    + "\tVị trí: " + vitri
                                //                    + "\tTiền: " + data[2]);
                                Program.f.tbLog.Text += "[" + DateTime.Now + "] " + "Đến lượt của người tiếp theo" + Environment.NewLine;
                                UpdateToFile("[" + DateTime.Now + "] " + "Đến lượt của người tiếp theo");
                            });
                            break;
                        case "Nhắn":
                            //Sự kiện Chat
                            Program.f.tbLog.Invoke((MethodInvoker)delegate
                            {
                                Program.f.tbLog.Text += "[" + DateTime.Now + "] " + userName + message + Environment.NewLine;
                                UpdateToFile("[" + DateTime.Now + "] " + userName + message);
                            });
                            server.SendMessageToEveryone(message+";"+userName, Id);
                            break;
                        case "Thuê":
                            //Sự kiện Thuê nhà trong trò chơi
                            Program.f.tbLog.Invoke((MethodInvoker)delegate
                            {
                                Program.f.tbLog.Text += "[" + DateTime.Now + "] " + message + Environment.NewLine;
                                UpdateToFile("[" + DateTime.Now + "] " + message);
                            });
                            server.SendMessageToOpponentClient(message, Id);
                            break;
                        case "Red pawn is already selected":
                            {
                                //Khi quân cờ đỏ được chọn, gửi  thông tin đến đối thủ để vô hiệu hóa nút chọn.
                                server.SendMessageToOpponentClient(message, Id);
                                Program.f.tbLog.Invoke((MethodInvoker)delegate
                                {
                                    Program.f.tbLog.Text += "[" + DateTime.Now + "] " + message + Environment.NewLine;
                                });
                                break;
                            }
                        case "Blue pawn is already selected":
                            {
                                //tương tự quân cờ đỏ.
                                server.SendMessageToOpponentClient(message, Id);
                                Program.f.tbLog.Invoke((MethodInvoker)delegate
                                {
                                    Program.f.tbLog.Text += "[" + DateTime.Now + "] " + message + Environment.NewLine;
                                });
                                break;
                            }
                        case "Người chơi mới đã vào":
                             Program.f.tbLog.Invoke((MethodInvoker)delegate
                                {
                                    Program.f.tbLog.Text += "[" + DateTime.Now + "] " + message + Environment.NewLine;
                                });
                            //khi một người chơi mới vào ( chưa chọn quân cờ).
                            for (int i = 0; i < room.Count; i++)//Duyệt các phòng hiện tại
                            {
                                if (room[i].roomId == Convert.ToInt32(arraypayload[1])) //Kiểm tra phòng có tồn tại không, Nếu có:
                                {
                                    if (room[i].roomTaken.Blue && room[i].roomTaken.Red)
                                    {
                                        server.SendMessageToSender("Phòng đã đủ người chơi" + ";" + room[i].roomId + ";"+ room[i].roomPlayer.Name1+";"+ room[i].roomPlayer.Name2, Id);
                                        break;
                                    }
                                    //Kiểm tra quân đỏ đã được chọn chưa và gửi thông tin lại người gửi để thông báo
                                    if (room[i].roomTaken.Red) server.SendMessageToSender("Red pawn is already selected" + ";" + room[i].roomId + ";" + room[i].roomPlayer.Name1, Id);
                                    //Kiểm tra quân xanh đã được chọn chưa và gửi thông tin lại người gửi để thông báo
                                    if (room[i].roomTaken.Blue) server.SendMessageToSender("Blue pawn is already selected" + ";" + room[i].roomId+";"+ room[i].roomPlayer.Name2, Id);
                                    break;
                                }
                            }
                                break;
                    }

                    //    if (message.Contains("Cả 2 người chơi đã kết nối: "))
                    //{
                    //    server.SendMessageToEveryone(message, Id);
                    //    Program.f.tbLog.Invoke((MethodInvoker)delegate
                    //    {
                    //        Program.f.tbLog.Text += "[" + DateTime.Now + "] " + message + Environment.NewLine;
                    //        UpdateToFile("[" + DateTime.Now + "] " + message);
                    //    });
                    //}
                    //Nhận được thông điệp đỏ đã kết nối
                    //else if (Regex.IsMatch(message, @"Đỏ\s*\(\s*(\d+)\s*\)") 
                    //    && !message.Contains(" đã rời") 
                    //    && !message.Contains("thắng") 
                    //    && !message.Contains("thua"))
                    //{
                    //    userName = message;
                    //    Program.f.tbLog.Invoke((MethodInvoker)delegate
                    //    {
                    //        Program.f.tbLog.Text += "[" + DateTime.Now + "] " + userName + " đã kết nối" + Environment.NewLine;
                    //        UpdateToFile("[" + DateTime.Now + "] " + userName + " đã kết nối");
                    //    });
                    //    server.SendMessageToOpponentClient(userName + " đã kết nối", Id);
                    //}
                    ////Nhân được thông điệp xanh đã kết nối
                    //else if (Regex.IsMatch(message, @"Xanh\s*\(\s*(\d+)\s*\)") 
                    //    && !message.Contains(" đã rời")
                    //    && !message.Contains("thắng")
                    //    && !message.Contains("thua"))
                    //{
                    //    userName = message;
                    //    Program.f.tbLog.Invoke((MethodInvoker)delegate
                    //    {
                    //        Program.f.tbLog.Text += "[" + DateTime.Now + "] " + userName + " đã kết nối" + Environment.NewLine;
                    //        UpdateToFile("[" + DateTime.Now + "] " + userName + " đã kết nối");
                    //    });
                    //    server.SendMessageToOpponentClient(userName + " đã kết nối", Id);
                    //}
                    //Nhận được thông điệp rời đi của client
                    //else if (message.Contains(" đã rời"))
                    //{
                    //    if (!message.Contains(" đã rời."))
                    //        server.SendMessageToOpponentClient(message, Id);
                    //    Program.f.tbLog.Invoke((MethodInvoker)delegate
                    //    {
                    //        Program.f.tbLog.Text += "[" + DateTime.Now + "] " + message + Environment.NewLine;
                    //        UpdateToFile("[" + DateTime.Now + "] " + message);
                    //    });
                    //    server.RemoveConnection(this.Id);
                    //    break;
                    //}
                    ////Nhận được tin nhắn của client và procast đến tất cả client còn lại
                    //else if (message.Contains(" nhắn: "))
                    //{
                    //    Program.f.tbLog.Invoke((MethodInvoker)delegate
                    //    {
                    //        Program.f.tbLog.Text += "[" + DateTime.Now + "] " + userName + message + Environment.NewLine;
                    //        UpdateToFile("[" + DateTime.Now + "] " + userName + message);
                    //    });
                    //    server.SendMessageToEveryone(userName + message, Id);
                    //}
                    //Nhận được thông điệp kết thúc lượt và các thông số lượt đi trước của đỏ
                    //else if (message.Contains("Kết quả lượt đi của Đỏ"))
                    //{   
                    //    server.SendMessageToOpponentClient(message, Id);
                    //    Program.f.tbLog.Invoke((MethodInvoker)delegate
                    //    {
                    //        string tempMessage = message;
                    //        tempMessage = tempMessage.Replace("Kết quả lượt đi của Đỏ", "");
                    //        string[] data = tempMessage.Split('~');
                    //        int STT = Convert.ToInt32(data[1]);
                    //        string vitri = FindNameByNumber(STT);
                    //        Program.f.tbLog.Text += "[" + DateTime.Now + "] " + userName
                    //                            + "\tVị trí: " + vitri
                    //                            + "\tTiền: " + data[2] + Environment.NewLine;
                    //        UpdateToFile("[" + DateTime.Now + "] " + userName
                    //                            + "\tVị trí: " + vitri
                    //                            + "\tTiền: " + data[2]);
                    //        Program.f.tbLog.Text += "[" + DateTime.Now + "] " + "Đến lượt của Xanh" + Environment.NewLine;
                    //        UpdateToFile("[" + DateTime.Now + "] " + "Đến lượt của Xanh");
                    //    });
                        
                    //}
                    ////Nhận được thông điệp kết thúc lượt và các thông số lượt đi trước của xanh
                    //else if (message.Contains("Kết quả lượt đi của Xanh"))
                    //{   
                    //    server.SendMessageToOpponentClient(message, Id);
                    //    Program.f.tbLog.Invoke((MethodInvoker)delegate
                    //    {
                    //        string tempMessage = message;
                    //        tempMessage = tempMessage.Replace("Kết quả lượt đi của Xanh", "");
                    //        string[] data = tempMessage.Split('~');
                    //        int STT = Convert.ToInt32(data[1]);
                    //        string vitri = FindNameByNumber(STT);
                    //        Program.f.tbLog.Text += "[" + DateTime.Now + "] " + userName
                    //                             + "\tVị trí: " + vitri
                    //                             + "\tTiền: " + data[2] + Environment.NewLine;
                    //        UpdateToFile("[" + DateTime.Now + "] " + userName
                    //                            + "\tVị trí: " + vitri
                    //                            + "\tTiền: " + data[2]);
                    //        Program.f.tbLog.Text += "[" + DateTime.Now + "] " + "Đến lượt của Đỏ" + Environment.NewLine;
                    //        UpdateToFile("[" + DateTime.Now + "] " + "Đến lượt của Đỏ");
                    //    });
                        
                    //}
                    //Nhận được thông điệp người chơi chịu mức thuế từ đối phương và số tiền thuế
                    //else if (message.Contains("thuê"))
                    //{
                    //    Program.f.tbLog.Invoke((MethodInvoker)delegate
                    //    {
                    //        Program.f.tbLog.Text += "[" + DateTime.Now + "] " + message + Environment.NewLine;
                    //        UpdateToFile("[" + DateTime.Now + "] " + message);
                    //    });
                    //    server.SendMessageToOpponentClient(message, Id);
                    //}
                     if (message.Contains("thắng.") || message.Contains("thua.")) {
                        Program.f.tbLog.Invoke((MethodInvoker)delegate
                        {
                            Program.f.tbLog.Text += "[" + DateTime.Now + "] " + message + Environment.NewLine;
                            UpdateToFile("[" + DateTime.Now + "] " + message);
                        });
                        server.RemoveConnection(this.Id);
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
        //hàm phân tích tin nhắn
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
        //ghi thông tin vào file
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
        private void RemoveRoom(int deleteId)
        {
            int index = room.FindIndex(room => room.roomId == deleteId);
            if (index != -1)
            {
                room.RemoveAt(index);
            }
        }
    }
}



