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
    internal class RoomInfo
    {
        private static RoomInfo _instance;
        public string Room { get; set; } // id phòng
        public string PlayerName { get; set; } // Tên mặc định của quân cờ người chơi chọn ( red, blue ) -> liên quan tới logic trò chơi
        public string UserName { get; set; } // Tên người chơi nhập vào textbox, dùng để gửi lên server, hiển thị lên form server
        public string choosenPlayer { get; set; }//đánh dấu tên người chơi chọn quân cờ để hiển thị trong form colorchoosing
        public bool NameRedIsTaken { get; set; } //đánh dấu quân cờ nào đã được chọn
        public bool NameBlueIsTaken { get; set; }
        private RoomInfo() { }
        public static RoomInfo Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new RoomInfo();
                return _instance;
            }
        }
    }
    internal class ConnectionOptions
    {
        private static ConnectionOptions _instance;
        public string IP { get; set; }
        public int Port { get; set; }
        public bool Started { get; set; } = false; //Kiểm tra trạng thái phòng chơi
        public bool Connect { get; set; } = false; // kiểm tra tình trạng kết nối
        private ConnectionOptions() { }
        public static ConnectionOptions Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ConnectionOptions();
                return _instance;
            }
        }
    }
}