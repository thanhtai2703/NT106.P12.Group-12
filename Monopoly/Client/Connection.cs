using System;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Client
{
    public partial class Connection : Form
    {
        public Connection()
        {
            
            InitializeComponent();
            roomTb.MaxLength = 5;
        }
        //Khi nhấn nút Connect 
        private void btnConnect_Click(object sender, EventArgs e)
        {
            //Nhập phòng cho client
            if (Regex.IsMatch(roomTb.Text, @"^\d+$"))
            {
                ConnectionOptions.Room = roomTb.Text;
                //gán giá trị cổng của sever cho biến Port trong class ConnectionOptions
                ConnectionOptions.Port = 11000;
                //Convert.ToInt32(insertPort.Text);
                // gán địa chỉ IP của sever cho biến IP trong class ConnectionOptions
                ConnectionOptions.IP =ip_textbox.Text;
                //insertIP.Text;
                //Gắn cho DialogResult kết quả OK 
                DialogResult = DialogResult.OK;
                //Close();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Please enter integer digit", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                roomTb.Text = "";
            }
          
        }
        private void returnBtn_Click(object sender, EventArgs e)
        {
            //Khi nhấn vào nút trở lại thì gắn cho DialogResult kết quả Cancel 
            DialogResult = DialogResult.Cancel;
            this.Hide();
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
            path.AddArc(new Rectangle(returnBtn.Width - radius, 0, radius, radius), 270, 90); // Góc trên phải
            path.AddArc(new Rectangle(returnBtn.Width - radius, returnBtn.Height - radius, radius, radius), 0, 90); // Góc dưới phải
            path.AddArc(new Rectangle(0, returnBtn.Height - radius, radius, radius), 90, 90); // Góc dưới trái

            path.CloseAllFigures();

            // Áp dụng vùng bo góc cho nút
            returnBtn.Region = new Region(path);
            btnChooseColor.Region = new Region(path);
        }
    }
}
