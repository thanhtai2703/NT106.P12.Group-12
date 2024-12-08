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
        private void StartSingleplayerGameBtn_Click(object sender, EventArgs e)
        {
            Gamemodes.Singleplayer = true;
            Gamemodes.Multiplayer = false;
            Hide();
            var game = new Game();
            game.ShowDialog();
        }
        //Xử lý sự kiện khi nhấn vào nút CREATE
        private void CreateBtn_Click(object sender, EventArgs e)
        {
            //Thiết lập chế độ chơi là nhiều người chơi
            Gamemodes.Singleplayer = false;
            Gamemodes.Multiplayer = true;
            Gamemodes.Create = true;
            //Tạo đối tượng game mới và hiển thị dưới dạng hộp thoại 
            Hide();
            var game = new Game();
            game.ShowDialog();
        }
        //Ấn vào Join
        private void JoinBtn_Click(object sender, EventArgs e)
        {
            //Thiết lập chế độ chơi là nhiều người chơi
            Gamemodes.Singleplayer = false;
            Gamemodes.Multiplayer = true;
            Gamemodes.Create = false;
            Hide();
            var game = new Game();
            game.ShowDialog();
        }
        //Xử lý khi bấm nút Thoát 
        private void QuitBtn_Click(object sender, EventArgs e)
        {
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
            CreateBtn.Region = new Region(path);
            JoinBtn.Region = new Region(path);
        }
    }
}

