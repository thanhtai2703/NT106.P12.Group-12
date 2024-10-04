using System;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Windows.Forms;

namespace Client
{
    public partial class ColorChoosing : Form
    {
        public ColorChoosing()
        {
            Program.colorChoosing = this;
            InitializeComponent();

            //Thiết lập giá trị mặc định cho tbColor
            tbColor.Text = "Chưa được chọn";

        }

        private void button1_Paint(object sender, PaintEventArgs e)
        {
            // Tạo GraphicsPath cho button bo góc
            GraphicsPath path = new GraphicsPath();
            int radius = 20; // Độ bo tròn của góc

            // Tạo một hình chữ nhật bo góc
            path.AddArc(new Rectangle(0, 0, radius, radius), 180, 90); // Góc trên trái
            path.AddArc(new Rectangle(returnBtn.Width - radius, 0, radius, radius), 270, 90); // Góc trên phải
            path.AddArc(new Rectangle(returnBtn.Width - radius, returnBtn.Height - radius, radius, radius), 0, 90); // Góc dưới phải
            path.AddArc(new Rectangle(0, returnBtn.Height - radius, radius, radius), 90, 90); // Góc dưới trái

            path.CloseAllFigures();

            // Áp dụng vùng bo góc cho nút
            returnBtn.Region = new Region(path);
            connect_button.Region = new Region(path);
        }
        private void connect_button_Click(object sender, EventArgs e)
        {
            switch (tbColor.Text)
            {
                //Nếu chọn màu đỏ 
                case "Đỏ":
                    //Gán tên người chơi là Red 
                    ConnectionOptions.PlayerName = "Đỏ" + " (" + ConnectionOptions.Room + ")";
                    Close();
                    DialogResult = DialogResult.OK;
                    break;
                 //Nếu chọn màu xanh
                case "Xanh":
                    //Gán tên người chơi là Blue
                    ConnectionOptions.PlayerName = "Xanh" + " (" + ConnectionOptions.Room + ")";
                    Close();
                    //Gắn cho DialogResult kết quả OK 
                    DialogResult = DialogResult.OK;
                    break;
                case "Chưa chọn":
                    //Nếu chưa chọn màu thì hiển thị thông báo yêu cầu chọn màu 
                    MessageBox.Show("Chọn màu!");
                    break;
            }

        }
        private void returnBtn_Click(object sender, EventArgs e)
        {
            //Khi nhấn vào nút trở lại thì gắn cho DialogResult kết quả Cancel 
            DialogResult = DialogResult.Cancel;
            Close();
        }

        //Xử lý sự kiện khi nhấn nút chọn màu đỏ
        private void chooseRedPlayerBtn_Click(object sender, EventArgs e)
        {
            //Cho tbColor hiển thị chữ "Red"
            tbColor.Text = "Đỏ";
        }
        //Xử lý sự kiện khi nhấn nút chọn màu đỏ
        private void chooseBluePlayerBtn_Click(object sender, EventArgs e)
        {
            //Cho tbColor hiển thị chữ "Red"
            tbColor.Text = "Xanh";
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
    }
}
