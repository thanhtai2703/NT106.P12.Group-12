using System;
using System.Windows.Forms;

namespace Client
{
    public partial class MainMenu : Form
    {
        public MainMenu()
        {
            InitializeComponent();
        }
        //Xử lý sự kiện khi nhấn vào nút Một người chơi á
        private void StartSingleplayerGameBtn_Click(object sender, EventArgs e)
        {
            //Thiết lập chế độ chơi là 1 người chơi 
            Gamemodes.Singleplayer = true;
            Gamemodes.Multiplayer = false;
            //Ẩn cái form MainMenu
            Hide();
            //Tạo đối tượng game mới và hiển thị dưới dạng hộp thoại 
            var game = new Game();
            game.ShowDialog();
            //Hiển thị lại form MainMenu sau khi chơi xong 
            Show();
        }
        //Xử lý sự kiện khi nhấn vào nút nhiều người chơi
        private void StartMultiplayerGameBtn_Click(object sender, EventArgs e)
        {
            //Thiết lập chế độ chơi là nhiều người chơi
            Gamemodes.Singleplayer = false;
            Gamemodes.Multiplayer = true;
            //Tạo đối tượng game mới và hiển thị dưới dạng hộp thoại 
            Hide();
            var game = new Game();
            game.ShowDialog();
            //Hiển thị lại form MainMenu sau khi chơi xong 
            Show();
        }
        //Xử lý khi bấm nút Thoát 
        private void QuitBtn_Click(object sender, EventArgs e)
        {
            // Hiển thị hộp thoại xác nhận và nếu người dùng chọn "Có", thoát ứng dụng
            if (MessageBox.Show("Bạn có muốn thoát?", "Thông báo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                Application.Exit();
        }
    }
}

