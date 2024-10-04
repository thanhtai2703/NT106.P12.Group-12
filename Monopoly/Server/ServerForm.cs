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
            Text = "Server. State: off";
            btnTurnOff.Text = "Close";
        }
        private void btnTurnOff_Click(object sender, EventArgs e)
        {
            switch (Text)
            {
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
                case "Server. State: on":
                    switch (MessageBox.Show("Turn off the server?" + Environment.NewLine + "Current game will be closed.", 
                                "Turning off", MessageBoxButtons.YesNo))
                    {
                        case DialogResult.Yes:
                            serverObject.Disconnect();
                            break;
                        case DialogResult.No:
                            break;
                    }
                    break;
            }
        }
        private void btnTurnOn_Click(object sender, EventArgs e)
        {
            try
            {
                btnTurnOn.Enabled = false;
                btnTurnOff.Enabled = true;
                serverObject = new ServerObject();
                listenThread = new Thread(serverObject.Listen_Client);
                listenThread.IsBackground = true;
                listenThread.Start();
                Text = "Server. State: on";
                btnTurnOff.Text = "Turn off";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi bật máy chủ: " + ex.Message);
                serverObject?.Disconnect();
            }
        }

        private void ServerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            serverObject?.Disconnect();
        }
    }
}