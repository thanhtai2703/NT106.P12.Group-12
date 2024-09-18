using System.IO;
using System.Xml.Schema;
namespace Picture_viewer
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);  // Attach KeyDown event handler
            this.KeyPreview = true;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        private List<string> _currentPaths = new List<string>();
        private int currentImageIndex = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Multiselect = true;
                ofd.Filter = "(*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    _currentPaths = ofd.FileNames.ToList();
                    flowLayoutPanel1.Controls.Clear();

                    foreach (string imageFile in _currentPaths)
                    {
                        PictureBox thumbnail = new PictureBox
                        {
                            Image = Image.FromFile(imageFile).GetThumbnailImage(50, 50, null, IntPtr.Zero),
                            Width = 50,
                            Height = 50,
                            Padding = new Padding(5),
                            Cursor = Cursors.Hand,
                            Tag = imageFile
                        };

                        thumbnail.Click += Thumbnail_Click;
                        flowLayoutPanel1.Controls.Add(thumbnail);
                    }
                    if (_currentPaths.Count > 0)
                    {
                        currentImageIndex = 0;
                        pictureBox1.Image = Image.FromFile(_currentPaths[currentImageIndex]);
                    }
                }
            }
        }
        private void Thumbnail_Click(object sender, EventArgs e)
        {
            PictureBox clickedThumbnail = sender as PictureBox;
            if (clickedThumbnail != null)
            {
                string selectedImagePath = clickedThumbnail.Tag as string;
                textBox1.Text = _currentPaths[currentImageIndex];
                if (!string.IsNullOrEmpty(selectedImagePath))
                {
                    currentImageIndex = _currentPaths.IndexOf(selectedImagePath);
                    pictureBox1.Image = Image.FromFile(_currentPaths[currentImageIndex]);
                }
            }
        }
        private void DisplayImage(string imagePath)
        {
            pictureBox1.Image = Image.FromFile(_currentPaths[currentImageIndex]);
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (_currentPaths.Count == 0) return;

            if (e.KeyCode == Keys.Right)
            {

                currentImageIndex = (currentImageIndex + 1) % _currentPaths.Count;
                textBox1.Text = _currentPaths[currentImageIndex];
                DisplayImage(_currentPaths[currentImageIndex]);
            }
            else if (e.KeyCode == Keys.Left)
            {
                currentImageIndex = (currentImageIndex - 1 + _currentPaths.Count) % _currentPaths.Count;
                textBox1.Text = _currentPaths[currentImageIndex];
                pictureBox1.Image = Image.FromFile(_currentPaths[currentImageIndex]);
            }
        }


        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void flowLayoutPanel1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
          
        }
    }
}
