using System;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace Client
{
    public partial class MainMenu : Form
    {
        public MainMenu()
        {
            InitializeComponent();
        }
        //Xử lý sự kiện khi nhấn vào nút Một người chơi á

        private void button1_Paint(object sender, PaintEventArgs e)
        {
            // Tạo GraphicsPath cho button bo góc
            GraphicsPath path = new GraphicsPath();
            int radius = 20; // Độ bo tròn của góc

            // Tạo một hình chữ nhật bo góc
            path.AddArc(new Rectangle(0, 0, radius, radius), 180, 90); // Góc trên trái
            path.AddArc(new Rectangle(quitBtn.Width - radius, 0, radius, radius), 270, 90); // Góc trên phải
            path.AddArc(new Rectangle(quitBtn.Width - radius, quitBtn.Height - radius, radius, radius), 0, 90); // Góc dưới phải
            path.AddArc(new Rectangle(0, quitBtn.Height - radius, radius, radius), 90, 90); // Góc dưới trái
            path.CloseAllFigures();
            // Áp dụng vùng bo góc cho nút
            quitBtn.Region = new Region(path);
            startSingleplayerGameBtn.Region = new Region(path);
            startMultiplayerGameBtn.Region = new Region(path);
            JoinBtn.Region = new Region(path);
        }


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
            //Show();
        }
        //Xử lý sự kiện khi nhấn vào nút nhiều người chơi
        private void StartMultiplayerGameBtn_Click(object sender, EventArgs e)
        {
            //Thiết lập chế độ chơi là nhiều người chơi
            Gamemodes.Singleplayer = false;
            Gamemodes.Multiplayer = true;
            Gamemodes.Create = true;
            //Tạo đối tượng game mới và hiển thị dưới dạng hộp thoại 
            Hide();
           // var lobby = new Lobby();
            //lobby.Show();
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

       

        private void Button_MouseEnter(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            btn.FlatAppearance.BorderSize = 2;
            btn.FlatAppearance.BorderColor = Color.LightBlue;
        }

        // Khi con trỏ chuột rời khỏi Button
        private void Button_MouseLeave(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            btn.FlatAppearance.BorderSize = 0;
        }

        private void JoinBtn_Click(object sender, EventArgs e)
        {
            //Thiết lập chế độ chơi là nhiều người chơi
            Gamemodes.Singleplayer = false;
            Gamemodes.Multiplayer = true;
            Gamemodes.Create = false;
            //Tạo đối tượng game mới và hiển thị dưới dạng hộp thoại 
            Hide();
            // var lobby = new Lobby();
            //lobby.Show();
            var game = new Game();
            game.ShowDialog();
            //Hiển thị lại form MainMenu sau khi chơi xong 
            Show();
        }
    }
}

