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

        public Lobby()
        {
            InitializeComponent();
        }
        public void DisplayConnectedPlayer(int num, string name)
        {
            switch (num)
            {
                case 1:
                    name1.Text = name;
                    break;
                case 2:
                    name2.Text = name;
                    break;
            } 
                
        }

        private void Start_Click(object sender, EventArgs e)
        {
            Game.SendMessageToServer("Bắt đầu" + ":" + ConnectionOptions.PlayerName+";"+0);
        }
    }
}
