using System;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
namespace Server
{
    internal static class Program
    {
        public static ServerForm f;
        [STAThread]
        static void Main(){
         
            Application.Run(new ServerForm());
        }
        //Lớp taken chứa trạng thái hiện tại của các quân cờ (được chọn,chưa được chọn)
        internal class Taken{
            public bool Red { get; set; }
            public bool Blue { get; set; }
        }
        //lớp Player chứa tên người chơi, dùng để cập nhật cho client
        internal class Player{
            public string Name1 { get; set; }
            public string Name2 { get; set; }
        }
        //Lớp phòng dùng để biểu thị cho 1 phòng chơi trong server
        internal class Room{
            public ClientObject[] players = new ClientObject[2]; //lưu lại client của người chơi để xử lí
            public int roomId;
            public Taken roomTaken = new Taken();
            public Player roomPlayer = new Player();
        }

    }
 
        

}
