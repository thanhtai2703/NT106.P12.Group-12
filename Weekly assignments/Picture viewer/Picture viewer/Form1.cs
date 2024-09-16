using System.IO;
using System.Xml.Schema;
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
        string _currentPath = "";
        private void button1_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    _currentPath = fbd.SelectedPath;
                    foreach(string file in Directory.GetFiles(_currentPath,"*jpg"))
                    {
                        listBox1.Items.Add(Path.GetFileName(file));
                    }
                    textBox1.Text = _currentPath;
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string name = listBox1.SelectedItem.ToString();
            string location = Path.Combine(_currentPath, name);
            pictureBox1.Image = Image.FromFile(location);
        }
    }
}
