using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Explorer
{
    public partial class Form1 : Form
    {
        private string currentDirectory;
        private TreeNode currentTreeNode;
        private string clipboardFile; // Stores the full path of the copied or cut file
        private bool isCutOperation;  // True if cut operation, false if copy operation
        public Form1()
        {
            InitializeComponent();
            LoadDrives();
        }
        private void LoadDrives()
        {
            foreach (var drive in DriveInfo.GetDrives())
            {
                TreeNode node = new TreeNode(drive.Name);
                node.Tag = drive;
                node.Nodes.Add("...");
                treeView1.Nodes.Add(node);
            }
        }

        private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Nodes[0].Text == "...")
            {
                e.Node.Nodes.Clear();
                LoadDirectoriesAndFiles(e.Node);
            }
        }
        private void LoadDirectoriesAndFiles(TreeNode node)
        {
            DirectoryInfo dir = new DirectoryInfo(node.FullPath);
            try
            {
                // Load subdirectories into the TreeView
                foreach (var directory in dir.GetDirectories())
                {
                    TreeNode dirNode = new TreeNode(directory.Name);
                    dirNode.Nodes.Add("...");
                    dirNode.Tag = directory;
                    node.Nodes.Add(dirNode);
                }

                // Load files into the ListView
                listView1.Items.Clear();
                foreach (var file in dir.GetFiles())
                {
                    ListViewItem item = new ListViewItem(file.Name);
                    item.Tag = file.FullName;  // Store the full file path in the Tag property
                    item.SubItems.Add(file.Length.ToString());  // Add file size (optional)
                    listView1.Items.Add(item);
                }
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Access Denied!");
            }
        }
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            currentTreeNode = e.Node;
            DirectoryInfo dir = new DirectoryInfo(e.Node.FullPath);
            currentDirectory = dir.FullName;
            LoadDirectoriesAndFiles(e.Node); // Load files and subfolders into ListView
        }
        private void InitializeContextMenu()
        {
            ContextMenuStrip contextMenu = new ContextMenuStrip();

            ToolStripMenuItem copyItem = new ToolStripMenuItem("Copy");
            copyItem.Click += CopyItem_Click;
            contextMenu.Items.Add(copyItem);

            ToolStripMenuItem cutItem = new ToolStripMenuItem("Cut");
            cutItem.Click += CutItem_Click;
            contextMenu.Items.Add(cutItem);

            ToolStripMenuItem pasteItem = new ToolStripMenuItem("Paste");
            pasteItem.Click += PasteItem_Click;
            contextMenu.Items.Add(pasteItem);

            ToolStripMenuItem deleteItem = new ToolStripMenuItem("Delete");
            deleteItem.Click += DeleteItem_Click;
            contextMenu.Items.Add(deleteItem);

            listView1.ContextMenuStrip = contextMenu;  // Attach context menu to the ListView
        }

        private void CopyItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                clipboardFile = listView1.SelectedItems[0].Tag.ToString();  // Full file path
                isCutOperation = false;  // It's a copy operation
            }
        }

        private void CutItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                clipboardFile = listView1.SelectedItems[0].Tag.ToString();  // Full file path
                isCutOperation = true;  // It's a cut operation
            }
        }

        private void PasteItem_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(clipboardFile) && !string.IsNullOrEmpty(currentDirectory))
            {
                string destinationPath = Path.Combine(currentDirectory, Path.GetFileName(clipboardFile));

                // Check if the destination file already exists
                if (File.Exists(destinationPath))
                {
                    MessageBox.Show("File already exists in the destination folder.");
                    return;
                }

                try
                {
                    if (isCutOperation)
                    {
                        File.Move(clipboardFile, destinationPath);  // Move file (cut operation)
                        isCutOperation = false;
                    }
                    else
                    {
                        File.Copy(clipboardFile, destinationPath);  // Copy file (copy operation)
                    }

                    clipboardFile = null;  // Clear the clipboard
                    LoadDirectoriesAndFiles(currentTreeNode);  // Refresh the view
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error while pasting the file: {ex.Message}");
                }
            }
        }

        private void DeleteItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                var fileToDelete = listView1.SelectedItems[0].Tag.ToString();  // Get the full file path
                try
                {
                    File.Delete(fileToDelete);
                    MessageBox.Show("File deleted successfully.");
                    LoadDirectoriesAndFiles(currentTreeNode);  // Refresh the view after delete
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting the file: {ex.Message}");
                }
            }
        }
        private void CreateNewFolder()
        {
            if (!string.IsNullOrEmpty(currentDirectory))
            {
                string newFolderPath = Path.Combine(currentDirectory, "New Folder");
                Directory.CreateDirectory(newFolderPath);
                LoadDirectoriesAndFiles(currentTreeNode);  // Refresh view after folder creation
            }
        }
        private async void CopyLargeFileAsync(string sourceFile, string destFile)
        {
            byte[] buffer = new byte[1024 * 1024]; // 1MB buffer
            using (FileStream sourceStream = new FileStream(sourceFile, FileMode.Open, FileAccess.Read))
            using (FileStream destStream = new FileStream(destFile, FileMode.Create, FileAccess.Write))
            {
                int bytesRead;
                while ((bytesRead = await sourceStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    await destStream.WriteAsync(buffer, 0, bytesRead);
                }
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }

}
