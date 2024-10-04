namespace Client
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            richTextBox1 = new RichTextBox();
            sendMsgTextBox = new RichTextBox();
            Send_btn = new Button();
            textBox3 = new TextBox();
            btnConnect = new Button();
            txtServer = new Label();
            txtUser = new Label();
            txtFriend = new Label();
            progressBar1 = new ProgressBar();
            choosefile = new Button();
            SuspendLayout();
            // 
            // textBox1
            // 
            textBox1.Location = new Point(125, 29);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(125, 27);
            textBox1.TabIndex = 0;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(328, 29);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(125, 27);
            textBox2.TabIndex = 1;
            // 
            // richTextBox1
            // 
            richTextBox1.Location = new Point(12, 89);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(776, 194);
            richTextBox1.TabIndex = 2;
            richTextBox1.Text = "";
            // 
            // sendMsgTextBox
            // 
            sendMsgTextBox.Location = new Point(12, 318);
            sendMsgTextBox.Name = "sendMsgTextBox";
            sendMsgTextBox.Size = new Size(464, 120);
            sendMsgTextBox.TabIndex = 3;
            sendMsgTextBox.Text = "";
            // 
            // Send_btn
            // 
            Send_btn.Location = new Point(523, 321);
            Send_btn.Name = "Send_btn";
            Send_btn.Size = new Size(148, 117);
            Send_btn.TabIndex = 4;
            Send_btn.Text = "Send";
            Send_btn.UseVisualStyleBackColor = true;
            Send_btn.Click += button1_Click;
            // 
            // textBox3
            // 
            textBox3.Location = new Point(546, 30);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(125, 27);
            textBox3.TabIndex = 5;
            // 
            // btnConnect
            // 
            btnConnect.Location = new Point(694, 30);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(94, 29);
            btnConnect.TabIndex = 6;
            btnConnect.Text = "Connect";
            btnConnect.UseVisualStyleBackColor = true;
            btnConnect.Click += button2_Click;
            // 
            // txtServer
            // 
            txtServer.AutoSize = true;
            txtServer.Font = new Font("Tahoma", 7.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtServer.Location = new Point(12, 36);
            txtServer.Name = "txtServer";
            txtServer.Size = new Size(95, 16);
            txtServer.TabIndex = 7;
            txtServer.Text = "Server Address";
            // 
            // txtUser
            // 
            txtUser.AutoSize = true;
            txtUser.Font = new Font("Tahoma", 7.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtUser.Location = new Point(256, 36);
            txtUser.Name = "txtUser";
            txtUser.Size = new Size(66, 16);
            txtUser.TabIndex = 8;
            txtUser.Text = "UserName";
            // 
            // txtFriend
            // 
            txtFriend.AutoSize = true;
            txtFriend.Font = new Font("Tahoma", 7.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtFriend.Location = new Point(482, 35);
            txtFriend.Name = "txtFriend";
            txtFriend.Size = new Size(43, 16);
            txtFriend.TabIndex = 9;
            txtFriend.Text = "Friend";
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(609, 66);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(168, 17);
            progressBar1.TabIndex = 10;
            // 
            // choosefile
            // 
            choosefile.Location = new Point(13, 289);
            choosefile.Name = "choosefile";
            choosefile.Size = new Size(94, 29);
            choosefile.TabIndex = 11;
            choosefile.Text = "ChooseFile";
            choosefile.UseVisualStyleBackColor = true;
            choosefile.Click += openToolStripMenuItem_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(choosefile);
            Controls.Add(progressBar1);
            Controls.Add(txtFriend);
            Controls.Add(txtUser);
            Controls.Add(txtServer);
            Controls.Add(btnConnect);
            Controls.Add(textBox3);
            Controls.Add(Send_btn);
            Controls.Add(sendMsgTextBox);
            Controls.Add(richTextBox1);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            Name = "Form1";
            Text = "Client";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBox1;
        private TextBox textBox2;
        private RichTextBox richTextBox1;
        private RichTextBox sendMsgTextBox;
        private Button Send_btn;
        private TextBox textBox3;
        private Button btnConnect;
        private Label txtServer;
        private Label txtUser;
        private Label txtFriend;
        private ProgressBar progressBar1;
        private Button choosefile;
    }
}
