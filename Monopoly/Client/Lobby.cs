using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;

//using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class Lobby : Form
    {
        public Lobby lobby;
        public List<Label> PlayerName = new List<Label>();
        public List<PictureBox> PlayerIcon = new List<PictureBox>();
        public int connectedPlayer = 0;
        public Lobby()
        {
            InitializeComponent();
            lobby = this;
           // btnStart.Visible = false;

            PlayerName.Add(label1);
            PlayerName.Add(label2);
            //PlayerName.Add(labelP3);
           // PlayerName.Add(labelP4);

            PlayerIcon.Add(pictureBox1);
            PlayerIcon.Add(pictureBox2);
            //PlayerIcon.Add(pictureBoxP3);
            //PlayerIcon.Add(pictureBoxP4);
        }
        public void DisplayConnectedPlayer(int num, string name)
        {
            //connectedPlayer++;

            switch (num)
            {
                case 1:
                    label1.Text = name;
                    break;
                case 2:
                    label2.Text = name;
                    break;
                default:
                    break;
            }
        }

        private void Start_Click(object sender, EventArgs e)
        {
            Game.SendMessageToServer("Bắt đầu" + ":" + ConnectionOptions.PlayerName+";"+0);
        }
    }
}
