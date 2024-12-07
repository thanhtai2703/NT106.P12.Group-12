using System;
using System.Windows.Forms;

namespace Client
{
    internal static class Program
    {
        public static ColorChoosing colorChoosing;
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainMenu());
        }
    }
    internal static class Gamemodes
    {
        public static bool Singleplayer { get; set; }
        public static bool Multiplayer { get; set; }
        public static bool Create { get; set; }
    }

    internal static class ConnectionOptions
    {
        public static string IP { get; set; }
        public static int Port { get; set; }
        public static string Room { get; set; }
        public static string PlayerName { get; set; } //Màu sắc quân cờ của người chơi
        public static string UserName { get; set; } //Tên người chơi khi nhập vào textbox
        public static string BlueUserName { get; set; } // biến dùng để hiển thị người chơi xanh khi người chơi xanh đã được chọn trong form colorchoosing
        public static string RedUserName {  get; set; } // 
        public static bool NameRedIsTaken { get; set; } //đánh dấu quân cờ nào đã được chọn
        public static bool NameBlueIsTaken { get; set; }
        public static bool Started { get; set; } = false; //Kiểm tra trạng thái phòng chơi
        public static bool Connect { get; set; } = false; // kiểm tra tình trạng kết nối
    }
}