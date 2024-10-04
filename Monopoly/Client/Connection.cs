using System;
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
        }
        //Khi nhấn nút Connect 
        private void btnConnect_Click(object sender, EventArgs e)
        {
            //Nhập phòng cho client
            if (Regex.IsMatch(roomTb.Text, @"^\d+$"))
            {
                ConnectionOptions.Room = roomTb.Text;
                //gán giá trị cổng của sever cho biến Port trong class ConnectionOptions
                ConnectionOptions.Port = 8888;
                //Convert.ToInt32(insertPort.Text);
                // gán địa chỉ IP của sever cho biến IP trong class ConnectionOptions
                ConnectionOptions.IP = "127.0.0.1";
                //insertIP.Text;
                //Gắn cho DialogResult kết quả OK 
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("Nhập tên phòng là số nguyên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                roomTb.Text = "";
            }
          
        }
        private void returnBtn_Click(object sender, EventArgs e)
        {
            //Khi nhấn vào nút trở lại thì gắn cho DialogResult kết quả Cancel 
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
