﻿using System;
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
        static List<Room> room = new List<Room>(); // khời tạo 1 danh sách phòng chơi trên server để quản lý các phòng chơi hiện có.
        DateTime now = DateTime.Now;
        protected internal string Id { get; }
        //constructer cung cấp ID riêng biệt cho từng client 
        public ClientObject(Socket socket, ServerObject serverObject)
        {
            Id = Guid.NewGuid().ToString(); //khởi tạo ID cho client
            Client = socket;
            server = serverObject;
            serverObject.AddConnection(this);//Thêm kết nối client vào server
        }
        //Xử lý tin nhắn gửi lên từ client
        public void Process()
        {
            try
            {
                while (true){
                    string message = GetMessage();
                    string []arraypayload = message.Split(';'); // tách chuỗi tin nhắn theo dấu ";"
                    switch (arraypayload[0]) // đọc thông điệp control message
                    {
                        #region xử lí tin nhắn
                        case "Connect": //Sau khi chọn tham gia(tạo phòng) thành công
                            bool isFound = false; // tạo biến kiểm tra phòng đã tồn tại chưa
                            for (int i = 0; i < room.Count; i++) //duyệt danh sách phòng hiện có để tìm kiếm
                            {
                                if (room[i].roomId == Convert.ToInt32(arraypayload[1])) //trong trường hợp tham gia -> phòng sẽ được tìm thấy 
                                {
                                    isFound = true; // Tìm thấy
                                    if (arraypayload[2] == "Red") // cập nhật người chơi chọn quân cờ đỏ.
                                    {
                                        room[i].roomTaken.Red = true; //Biến takenRed đánh dấu thể hiện quân đỏ đã được chọn.
                                        room[i].roomPlayer.Name1 = arraypayload[3]; // gán tên người chơi.
                                        room[i].players[0] = this;
                                    }
                                    if (arraypayload[2] == "Blue") // cập nhật người chơi chọn quân xanh.
                                    {
                                        room[i].roomTaken.Blue = true; //tương tự
                                        room[i].roomPlayer.Name2 = arraypayload[3];//tương tự
                                        room[i].players[1] = this;
                                    }
                                    if (room[i].roomTaken.Red == true && room[i].roomTaken.Blue == true) // kiểm tra nếu cả 2 quân cờ đã được chọn, tức là phòng đã đủ người
                                    {
                                        //Gửi thông điệp cập nhật cho client trong phòng
                                        server.SendMessageToEveryone("Update" + ";" + room[i].roomId + ";" + room[i].roomPlayer.Name1 + ";" + room[i].roomPlayer.Name2 + ";", Id, room[i].players);
                                    }
                                    server.SendMessageToOpponentClient(message, Id, room[i].players);
                                    break;
                                }
                            }
                            //Khi tạo phòng -> tạo phòng mới 
                            if (!isFound)
                            {
                                Room a = new Room(); // tạo một đối tượng room mới
                                //Tiến hành gán các giá trị
                                a.roomId = Convert.ToInt32(arraypayload[1]);
                                if (arraypayload[2] == "Red")
                                {
                                    a.roomTaken.Blue = false;
                                    a.roomTaken.Red = true;
                                    a.roomPlayer.Name1 = arraypayload[3];
                                    a.roomPlayer.Name2 = "";
                                    a.players[0] = this;
                                }
                                if (arraypayload[2] == "Blue")
                                {
                                    a.roomTaken.Red = false;
                                    a.roomTaken.Blue = true;
                                    a.roomPlayer.Name2 = arraypayload[3];
                                    a.roomPlayer.Name1 = "";
                                    a.players[1] = this;
                                }
                                room.Add(a); //Thêm phòng mới vào danh sách phòng hiện tại
                            }
                                Program.f.tbLog.Invoke((MethodInvoker)delegate
                                {
                                    Program.f.tbLog.Text += "[" + DateTime.Now + "] " + arraypayload[3] + " has connected to room : " +arraypayload[1]+ Environment.NewLine;
                                 
                                });
                            
                            break;
                        case "Start":
                            //Gửi thông điệp bắt đầu đến client, bắt đầu trò chơi
                            for (int i = 0; i < room.Count; i++)
                            {
                                if (room[i].roomId == Convert.ToInt32(arraypayload[1]))
                                {
                                    server.SendMessageToEveryone(message, Id, room[i].players);
                                    break;
                                }
                            }
                            Program.f.tbLog.Invoke((MethodInvoker)delegate
                            {
                                Program.f.tbLog.Text += "[" + DateTime.Now + "] " + "Room " + arraypayload[1] + " has started " + Environment.NewLine;
                              
                            });
                            break;
                        case "Exit":
                            //Sự kiện Ngắt kết nối khỏi phòng chơi
                            for(int i = 0; i < room.Count; i++){
                                if(room[i].roomId == Convert.ToInt32(arraypayload[1]))
                                {
                                    server.SendMessageToOpponentClient(message, Id, room[i].players);
                                    break;
                                } 
                            }
                            Program.f.tbLog.Invoke((MethodInvoker)delegate
                            {
                                Program.f.tbLog.Text += "[" + DateTime.Now + "] " + "Room " + arraypayload[1] + " has ended" + Environment.NewLine;
                            });
                            server.RemoveConnection(this.Id); //Xóa client khỏi danh sách
                            RemoveRoom(Convert.ToInt32(arraypayload[1])); // xóa phòng chơi khỏi danh sách
                            break;
                        case "Disconected":
                            server.RemoveConnection(this.Id);
                            break;
                        case "Exit lobby": // Thoát nhưng không xóa phòng ( trong trường hợp phòng chưa bắt đầu hay người chơi không phải chủ phòng)
                            for (int i = 0; i < room.Count; i++)
                            {
                                if (room[i].roomId == Convert.ToInt32(arraypayload[1]))
                                {
                                    server.SendMessageToOpponentClient(message, Id, room[i].players);
                                    switch (arraypayload[2])
                                    {
                                        case "Red":
                                            room[i].roomTaken.Red = false;
                                            break;
                                        case "Blue":
                                            room[i].roomTaken.Blue = false;
                                            break;
                                    }
                                    break;
                                }
                            }
                            server.RemoveConnection(this.Id); // xóa client khỏi danh sách
                            break;
                        case "Result":
                            for (int i = 0; i < room.Count; i++)
                            {
                                if (room[i].roomId == Convert.ToInt32(arraypayload[1]))
                                {
                                    server.SendMessageToOpponentClient(message, Id, room[i].players);
                                    break;
                                }
                            }
                            break;
                        case "Send":
                            //Sự kiện Chat
                            for (int i = 0; i < room.Count; i++)
                            {
                                if(room[i].roomId == Convert.ToInt32(arraypayload[1]))
                                {
                                   server.SendMessageToEveryone(message, Id, room[i].players);
                                   break;
                                }
                            }
                            Program.f.tbLog.Invoke((MethodInvoker)delegate
                            {
                                Program.f.tbLog.Text += "[" + DateTime.Now + "] room " + arraypayload[1] +":" + arraypayload[2] +" send a message " + Environment.NewLine;
                            });
                            break;
                        case "Rent":
                            //Sự kiện Thuê nhà trong trò chơi
                            for (int i = 0; i < room.Count; i++)
                            {
                                if (room[i].roomId == Convert.ToInt32(arraypayload[1]))
                                {
                                    server.SendMessageToOpponentClient(message, Id, room[i].players);
                                    break;
                                }
                            }
                            break;
                        case "Location":
                            for (int i = 0; i < room.Count; i++)
                            {
                                if (room[i].roomId == Convert.ToInt32(arraypayload[1]))
                                {
                                    server.SendMessageToOpponentClient(message, Id, room[i].players);
                                    break;
                                }
                            }
                            break;
                        case "Create":
                            //bool isCreated = false;
                             Program.f.tbLog.Invoke((MethodInvoker)delegate{
                                    Program.f.tbLog.Text += "[" + DateTime.Now + "] " + "Room " + arraypayload[1] + " is Created" + Environment.NewLine;
                                });
                            //khi một người chơi mới vào ( chưa chọn quân cờ ).
                            for (int i = 0; i < room.Count; i++)//Duyệt các phòng hiện tại
                            {
                                if (room[i].roomId == Convert.ToInt32(arraypayload[1])) //Kiểm tra phòng có tồn tại không, Nếu có:
                                {
                                    server.SendMessageToSender("Room already exists" + ";" + arraypayload[1], Id);
                                    server.RemoveConnection(this.Id);
                                    Program.f.tbLog.Invoke((MethodInvoker)delegate
                                    {
                                        Program.f.tbLog.Text += "[" + DateTime.Now + "] " + "Room " + arraypayload[1] + " already exists" + Environment.NewLine;
                                    });
                                    break;
                                }
                            }
                                break;
                        case "Join":
                            bool Found = false;
                            for (int i = 0; i < room.Count; i++)//Duyệt các phòng hiện tại
                            {
                                if (room[i].roomId == Convert.ToInt32(arraypayload[1])) //Kiểm tra phòng có tồn tại không, Nếu có:
                                {
                                    Found = true;
                                    //Nếu phòng cđã đủ 2 người chơi
                                    if (room[i].roomTaken.Blue && room[i].roomTaken.Red)
                                    {
                                        server.SendMessageToSender("Room is full" + ";" + room[i].roomId + ";" + room[i].roomPlayer.Name1 + ";" + room[i].roomPlayer.Name2, Id);
                                        server.RemoveConnection(this.Id);
                                        break;
                                    }
                                    //Kiểm tra quân cờ nào đã được chọn và gửi thông báo
                                    if (room[i].roomTaken.Red) server.SendMessageToSender("Red pawn already selected" + ";" + room[i].roomId + ";" + room[i].roomPlayer.Name1, Id);
                                    if (room[i].roomTaken.Blue) server.SendMessageToSender("Blue pawn already selected" + ";" + room[i].roomId + ";" + room[i].roomPlayer.Name2, Id);
                                    break;
                                }

                            }
                            if (!Found)
                            {
                                server.SendMessageToSender("Room not found" + ";" + arraypayload[1], Id);
                                server.RemoveConnection(this.Id);
                            }
                            break;
                        case "Win": //cập nhật người chơi thắng và xóa phòng chơi
                            Program.f.tbLog.Invoke((MethodInvoker)delegate
                            {
                                Program.f.tbLog.Text += "[" + DateTime.Now + "] " + "Player " + arraypayload[1] + " has won at room " + arraypayload[2] + Environment.NewLine;
                                Program.f.tbLog.Text += "[" + DateTime.Now + "] " + "Room " + arraypayload[2] + " has ended " + Environment.NewLine;
                            });
                            RemoveRoom(Convert.ToInt32(arraypayload[2]));
                            server.RemoveConnection(this.Id);
                            break;
                        case "Lose":
                            Program.f.tbLog.Invoke((MethodInvoker)delegate
                            {
                                Program.f.tbLog.Text += "[" + DateTime.Now + "] " + "Player " + arraypayload[1] + " has losed " + arraypayload[2] + Environment.NewLine;
                            });
                            RemoveRoom(Convert.ToInt32(arraypayload[2]));
                            server.RemoveConnection(this.Id);
                            break;
                            #endregion
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
        //Đóng kết nối 
        protected internal void Close()
        {
            Client.Shutdown(SocketShutdown.Both);
            Client.Close();
        }
        //Xóa phòng chơi, dùng để tái tạo lại danh sách phòng
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



