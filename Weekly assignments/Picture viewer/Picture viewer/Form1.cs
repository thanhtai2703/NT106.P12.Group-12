using System.IO;
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
        string _current = "";
        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "*.jpg|*.jpg|*.gif|*.ico";
                if (ofd.ShowDialog() == DialogResult.OK)
                {

                    _current = ofd.FileName;
                    listBox1.Items.Add(ofd.FileName);
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            pictureBox1.Image = Image.FromFile(listBox1.Text);
        }
    }
}
