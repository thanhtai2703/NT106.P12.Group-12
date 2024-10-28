using System;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Client
{
    public partial class ColorChoosing : Form
    {
        public ColorChoosing()
        {
            Program.colorChoosing = this;
            InitializeComponent();
            txtName.MaxLength = 12;
            if (ConnectionOptions.NameRedIsTaken)
            {
                chooseRedPlayerBtn.Enabled = false;
                label3.Text = ConnectionOptions.RedUserName;
            }
            if (ConnectionOptions.NameBlueIsTaken)
            {
                chooseBluePlayerBtn.Enabled = false;
                label4.Text = ConnectionOptions.BlueUserName;
            }  
            //Thiết lập giá trị mặc định cho tbColor
            tbColor.Text = "Not selected yet.";

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
            if (Regex.IsMatch(txtName.Text, @"[^\p{L}\p{N}\s]"))
            {
                MessageBox.Show("Please enter a name without special characters.");
            }
            else if(txtName.Text == "")
            {
                MessageBox.Show("Please enter the player’s name.");
            }    
            else
            {
                switch (tbColor.Text)
                {
                    //Nếu chọn màu đỏ 
                    case "Red":
                        //Gán tên người chơi là Red 
                        ConnectionOptions.UserName = txtName.Text;
                        ConnectionOptions.PlayerName = "Red" + ";" + ConnectionOptions.Room;
                        //Close();
                        this.Hide();
                        DialogResult = DialogResult.OK;
                        break;
                    //Nếu chọn màu xanh
                    case "Blue":
                        //Gán tên người chơi là Blue
                        ConnectionOptions.UserName = txtName.Text;
                        ConnectionOptions.PlayerName = "Blue" + ";" + ConnectionOptions.Room;
                        //Close();
                        this.Hide();
                        //Gắn cho DialogResult kết quả OK 
                        DialogResult = DialogResult.OK;
                        break;
                    case "Not selected yet.":
                        //Nếu chưa chọn màu thì hiển thị thông báo yêu cầu chọn màu 
                        MessageBox.Show("Choose a color!");
                        break;
                }
            }

        }
       
        private void returnBtn_Click(object sender, EventArgs e)
        {
            //Khi nhấn vào nút trở lại thì gắn cho DialogResult kết quả Cancel 
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        //Xử lý sự kiện khi nhấn nút chọn màu đỏ
        private void chooseRedPlayerBtn_Click(object sender, EventArgs e)
        {
            //Cho tbColor hiển thị chữ "Red"
            tbColor.Text = "Red";
        }
        //Xử lý sự kiện khi nhấn nút chọn màu đỏ
        private void chooseBluePlayerBtn_Click(object sender, EventArgs e)
        {
            //Cho tbColor hiển thị chữ "Red"
            tbColor.Text = "Blue";
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
