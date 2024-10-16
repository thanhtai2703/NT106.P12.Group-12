using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;

namespace Server
{
    public partial class ServerForm : Form
    {
        private static ServerObject serverObject;
        private static Thread listenThread;
        public ServerForm()
        {
            InitializeComponent();
            Program.f = this;
            Text = "Server. State: off"; // Chuỗi thể hiện trạng thái hiện tại của server
            btnTurnOff.Text = "Close";
        }
        private void btnTurnOff_Click(object sender, EventArgs e)
        {
            switch (Text)
            {
                //Trường hợp server chưa được bật, hiển thị form đóng.
                case "Server. State: off":
                    switch (MessageBox.Show("Close server form?", 
                                "Closing", MessageBoxButtons.YesNo))
                    {
                        case DialogResult.Yes:
                            Close();
                            break;
                        case DialogResult.No:
                            break;
                    }
                    break;
                 //Trường hợp server đang bật, hiển thị form yêu cầu đưa ra lựa chọn.
                case "Server. State: on":
                    switch (MessageBox.Show("Turn off the server?" + Environment.NewLine + "Current game will be closed.", 
                                "Turning off", MessageBoxButtons.YesNo))
                    {
                        case DialogResult.Yes: // chọn yes
                            serverObject.Disconnect(); // ngắt kết nối server.
                            break;
                        case DialogResult.No://chọn No, quay lại.
                            break;
                    }
                    break;
            }
        }
        //Sự kiện khi bật nút ON
        private void btnTurnOn_Click(object sender, EventArgs e)
        {
            try
            {
                // vô hiệu hóa nút turn on
                btnTurnOn.Enabled = false;
                //Cho phép sử dụng nút turn off
                btnTurnOff.Enabled = true;
                serverObject = new ServerObject();
                //Tạo một luồng lắng nghe kết nối từ client
                listenThread = new Thread(serverObject.Listen_Client);
                listenThread.IsBackground = true;
                listenThread.Start();
                //Cập nhật chuỗi điều kiển thành "Server. State: on"
                Text = "Server. State: on";
                btnTurnOff.Text = "Turn off";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi bật máy chủ: " + ex.Message);
                serverObject?.Disconnect();
            }
        }
        //Sự kiện đóng bảng server
        private void ServerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            serverObject?.Disconnect();
        }
    }
}