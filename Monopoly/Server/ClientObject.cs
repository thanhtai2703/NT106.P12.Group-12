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
                                    Program.f.tbLog.Text += "[" + DateTime.Now + "] " + arraypayload[3] + " đã kết nối vào phòng " +arraypayload[2]+ Environment.NewLine;
                                    UpdateToFile("[" + DateTime.Now + "] " + arraypayload[3] + " đã kết nối vào phòng " + arraypayload[2]);
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
                                Program.f.tbLog.Text += "[" + DateTime.Now + "] " + "Phòng " + arraypayload[2] + " đã bắt đầu" + Environment.NewLine;
                                UpdateToFile("[" + DateTime.Now + "] " + "Phòng " + arraypayload[2] + " đã bắt đầu");
                            });
                            break;
                        case "Rời":
                            //Sự kiện Ngắt kết nối khỏi phòng chơi
                            server.SendMessageToOpponentClient(message, Id);
                            Program.f.tbLog.Invoke((MethodInvoker)delegate
                            {
                                Program.f.tbLog.Text += "[" + DateTime.Now + "] " + "Phòng " + arraypayload[1] + " đã kết thúc" + Environment.NewLine;
                                UpdateToFile("[" + DateTime.Now + "] " + "Phòng " + arraypayload[1] + " đã kết thúc");
                            });
                            server.RemoveConnection(this.Id);
                            RemoveRoom(Convert.ToInt32(arraypayload[1]));
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
                                Program.f.tbLog.Text += "[" + DateTime.Now + "] Phòng" + arraypayload[2] + ":Đến lượt của người tiếp theo" + Environment.NewLine;
                                UpdateToFile("[" + DateTime.Now + "] Phòng" + arraypayload[2] + ": Đến lượt của người tiếp theo");
                            });
                            break;
                        case "Nhắn":
                            //Sự kiện Chat
                            Program.f.tbLog.Invoke((MethodInvoker)delegate
                            {
                                Program.f.tbLog.Text += "[" + DateTime.Now + "] Phòng " + arraypayload[1] +":" + arraypayload[2] +"đã gửi tin nhắn : "+ arraypayload[3] + Environment.NewLine;
                                UpdateToFile("[" + DateTime.Now + "] Phòng" + arraypayload[1] + ":" + arraypayload[2] + "đã gửi tin nhắn : " + arraypayload[3]);
                            });
                            server.SendMessageToEveryone(message, Id);
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
                        case "Quân đỏ đã được chọn":
                            {
                                //Khi quân cờ đỏ được chọn, gửi  thông tin đến đối thủ để vô hiệu hóa nút chọn.
                                server.SendMessageToOpponentClient(message, Id);
                                Program.f.tbLog.Invoke((MethodInvoker)delegate
                                {
                                    Program.f.tbLog.Text += "[" + DateTime.Now + "] " + message + Environment.NewLine;
                                });
                                break;
                            }
                        case "Quân xanh đã được chọn":
                            {
                                //tương tự quân cờ đỏ.
                                server.SendMessageToOpponentClient(message, Id);
                                Program.f.tbLog.Invoke((MethodInvoker)delegate
                                {
                                    Program.f.tbLog.Text += "[" + DateTime.Now + "] " + message + Environment.NewLine;
                                });
                                break;
                            }
                        case "Tạo phòng":
                            //bool isCreated = false;
                             Program.f.tbLog.Invoke((MethodInvoker)delegate
                                {
                                    Program.f.tbLog.Text += "[" + DateTime.Now + "] " + message + Environment.NewLine;
                                });
                            //khi một người chơi mới vào ( chưa chọn quân cờ).
                            for (int i = 0; i < room.Count; i++)//Duyệt các phòng hiện tại
                            {
                                if (room[i].roomId == Convert.ToInt32(arraypayload[1])) //Kiểm tra phòng có tồn tại không, Nếu có:
                                {
                                    server.SendMessageToSender("Phòng đã tồn tại" + ";" + arraypayload[1], Id);
                                    server.RemoveConnection(this.Id);
                                    Program.f.tbLog.Invoke((MethodInvoker)delegate
                                    {
                                        Program.f.tbLog.Text += "[" + DateTime.Now + "] " + "Phòng" + arraypayload[1] + "đã tồn tại" + Environment.NewLine;
                                    });
                                    break;
                                }
                            }
                                break;
                        case "Tham gia":
                            bool Found = false;
                            for (int i = 0; i < room.Count; i++)//Duyệt các phòng hiện tại
                            {
                                if (room[i].roomId == Convert.ToInt32(arraypayload[1])) //Kiểm tra phòng có tồn tại không, Nếu có:
                                {
                                    Found = true;
                                    if (room[i].roomTaken.Blue && room[i].roomTaken.Red)
                                    {
                                        server.SendMessageToSender("Phòng đã đủ người chơi" + ";" + room[i].roomId + ";" + room[i].roomPlayer.Name1 + ";" + room[i].roomPlayer.Name2, Id);
                                        server.RemoveConnection(this.Id);
                                        break;
                                    }
                                    //Kiểm tra quân đỏ đã được chọn chưa và gửi thông tin lại người gửi để thông báo
                                    if (room[i].roomTaken.Red) server.SendMessageToSender("Quân đỏ đã được chọn" + ";" + room[i].roomId + ";" + room[i].roomPlayer.Name1, Id);
                                    //Kiểm tra quân xanh đã được chọn chưa và gửi thông tin lại người gửi để thông báo
                                    if (room[i].roomTaken.Blue) server.SendMessageToSender("Quân xanh đã được chọn" + ";" + room[i].roomId + ";" + room[i].roomPlayer.Name2, Id);
                                    break;
                                }

                            }
                            if (!Found)
                                server.SendMessageToSender("Không tìm thấy phòng" + ";" + arraypayload[1],Id);
                            break;
                        case "Thắng":
                            Program.f.tbLog.Invoke((MethodInvoker)delegate
                            {
                                Program.f.tbLog.Text += "[" + DateTime.Now + "] " + "Người chơi " + arraypayload[1] + "đã dành chiến thắng ở phòng " + arraypayload[2] + Environment.NewLine;
                                UpdateToFile("[" + DateTime.Now + "] " + message);
                                Program.f.tbLog.Text += "[" + DateTime.Now + "] " + "Phòng " + arraypayload[2] + " đã kết thúc " + Environment.NewLine;
                                UpdateToFile("[" + DateTime.Now + "] " + message);
                            });
                            RemoveRoom(Convert.ToInt32(arraypayload[2]));
                            server.RemoveConnection(this.Id);
                            break;
                        case "Thua":
                            Program.f.tbLog.Invoke((MethodInvoker)delegate
                            {
                                Program.f.tbLog.Text += "[" + DateTime.Now + "] " + "Người chơi " + arraypayload[1] + "đã thua ở phòng " + arraypayload[2] + Environment.NewLine;
                                UpdateToFile("[" + DateTime.Now + "] " + message);
                                Program.f.tbLog.Text += "[" + DateTime.Now + "] " + "Phòng " + arraypayload[2] + " đã kết thúc " + Environment.NewLine;
                                UpdateToFile("[" + DateTime.Now + "] " + message);
                            });
                            RemoveRoom(Convert.ToInt32(arraypayload[2]));
                            server.RemoveConnection(this.Id);
                            break;

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



