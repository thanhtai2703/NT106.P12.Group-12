using System;
using System.Windows.Forms;
using System.IO;
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
    }
 
        

}
