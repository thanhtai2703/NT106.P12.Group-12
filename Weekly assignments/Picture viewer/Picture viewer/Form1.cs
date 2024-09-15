using System.IO;
using System.Drawing;
using System.Data;


namespace Picture_viewer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        public string _currentDirectory = "";
        private void button1_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    string[] files = Directory.GetFiles(dlg.SelectedPath, "*.jpg");

                    _currentDirectory = dlg.SelectedPath;
                    textBox1.Text = _currentDirectory;
                    foreach (string file in files)
                    {
                        listBox1.Items.Add(Path.GetFileName(file));
                    }
                }
            }

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string directory = Path.Combine(_currentDirectory, listBox1.Text).ToString();
            pictureBox1.Image = Image.FromFile(directory);
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
          
        }
    }
}
