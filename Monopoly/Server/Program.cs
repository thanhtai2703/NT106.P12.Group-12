﻿using System;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
namespace Server
{
    internal static class Program
    {
        public static ServerForm f;
        [STAThread]
        static void Main()
        {
            
                    // Lấy đường dẫn đến thư mục bin của ứng dụng
                    string binDirectory = AppDomain.CurrentDomain.BaseDirectory;

                    // Tạo đường dẫn tới thư mục bạn muốn tạo trong thư mục bin
                    string folderPath = Path.Combine(binDirectory, "Data");

                    // Kiểm tra xem thư mục đã tồn tại chưa, nếu chưa thì tạo mới
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ServerForm());
        }
        //Lớp taken chứa trạng thái hiện tại của các quân cờ (được chọn,chưa được chọn)
        internal class Taken
        {
            //public static bool Red { get; set; }
            //public static bool Blue { get; set; }
            public bool Red { get; set; }
            public bool Blue { get; set; }
        }
        //lớp Player chứa tên người chơi, dùng để cập nhật cho client
        internal class Player
        {
            public string Name1 { get; set; }
            public string Name2 { get; set; }
        }
        //Lớp phòng dùng để biểu thị cho 1 phòng chơi trong sercvcer
        internal class Room
        {
            public int roomId;
            public Taken roomTaken = new Taken();
            public Player roomPlayer = new Player();
        }

    }
 
        

}
