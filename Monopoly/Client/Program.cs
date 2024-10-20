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
        public static string PlayerName { get; set; }
        public static string UserName { get; set; }
        public static string BlueUserName { get; set; }
        public static string RedUserName {  get; set; }
        public static bool NameRedIsTaken { get; set; }
        public static bool NameBlueIsTaken { get; set; }
    }
}