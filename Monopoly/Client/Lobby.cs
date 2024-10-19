using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;


//using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

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
                    if (name1.InvokeRequired)
                    {
                        // Sử dụng Invoke để đảm bảo thay đổi được thực hiện trên UI thread
                        name1.Invoke(new Action(() => name1.Text = name));
                    }
                    else
                    {
                        name1.Text = name;
                    }
                    break;
                case 2:
                    if (name2.InvokeRequired)
                    {
                        // Sử dụng Invoke để đảm bảo thay đổi được thực hiện trên UI thread
                        name2.Invoke(new Action(() => name2.Text = name));
                    }
                    else
                    {
                        name2.Text = name;
                    }
                    break;
            } 
                
        }

        private void Start_Click(object sender, EventArgs e)
        {
        }
    }
}
