namespace Client
{
    partial class Connection
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label labelIp;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Connection));
            this.roomTb = new System.Windows.Forms.TextBox();
            this.btnChooseColor = new System.Windows.Forms.Button();
            this.returnBtn = new System.Windows.Forms.Button();
            this.ip_textbox = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            labelIp = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = System.Drawing.Color.Transparent;
            label1.Font = new System.Drawing.Font("Elephant", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label1.Location = new System.Drawing.Point(164, 139);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(72, 25);
            label1.TabIndex = 20;
            label1.Text = "Phòng";
            // 
            // labelIp
            // 
            labelIp.AutoSize = true;
            labelIp.BackColor = System.Drawing.Color.Transparent;
            labelIp.Font = new System.Drawing.Font("Elephant", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            labelIp.Location = new System.Drawing.Point(164, 204);
            labelIp.Name = "labelIp";
            labelIp.Size = new System.Drawing.Size(37, 25);
            labelIp.TabIndex = 22;
            labelIp.Text = "IP";
            // 
            // roomTb
            // 
            this.roomTb.BackColor = System.Drawing.Color.White;
            this.roomTb.ForeColor = System.Drawing.Color.Black;
            this.roomTb.Location = new System.Drawing.Point(169, 172);
            this.roomTb.MaxLength = 15;
            this.roomTb.Multiline = true;
            this.roomTb.Name = "roomTb";
            this.roomTb.Size = new System.Drawing.Size(282, 29);
            this.roomTb.TabIndex = 14;
            // 
            // btnChooseColor
            // 
            this.btnChooseColor.BackColor = System.Drawing.Color.White;
            this.btnChooseColor.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnChooseColor.FlatAppearance.BorderSize = 0;
            this.btnChooseColor.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnChooseColor.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnChooseColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChooseColor.Font = new System.Drawing.Font("Elephant", 10.2F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChooseColor.ForeColor = System.Drawing.Color.Black;
            this.btnChooseColor.Location = new System.Drawing.Point(320, 288);
            this.btnChooseColor.Name = "btnChooseColor";
            this.btnChooseColor.Size = new System.Drawing.Size(147, 45);
            this.btnChooseColor.TabIndex = 13;
            this.btnChooseColor.Text = "Connect";
            this.btnChooseColor.UseVisualStyleBackColor = false;
            this.btnChooseColor.Click += new System.EventHandler(this.btnConnect_Click);
            this.btnChooseColor.Paint += new System.Windows.Forms.PaintEventHandler(this.button1_Paint);
            this.btnChooseColor.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.btnChooseColor.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // returnBtn
            // 
            this.returnBtn.BackColor = System.Drawing.Color.White;
            this.returnBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.returnBtn.FlatAppearance.BorderSize = 0;
            this.returnBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.returnBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.returnBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.returnBtn.Font = new System.Drawing.Font("Elephant", 10.8F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.returnBtn.ForeColor = System.Drawing.Color.Black;
            this.returnBtn.Location = new System.Drawing.Point(144, 288);
            this.returnBtn.Name = "returnBtn";
            this.returnBtn.Size = new System.Drawing.Size(146, 45);
            this.returnBtn.TabIndex = 19;
            this.returnBtn.Text = "Back";
            this.returnBtn.UseVisualStyleBackColor = false;
            this.returnBtn.Click += new System.EventHandler(this.returnBtn_Click);
            this.returnBtn.Paint += new System.Windows.Forms.PaintEventHandler(this.button1_Paint);
            this.returnBtn.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.returnBtn.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // ip_textbox
            // 
            this.ip_textbox.BackColor = System.Drawing.Color.White;
            this.ip_textbox.ForeColor = System.Drawing.Color.Black;
            this.ip_textbox.Location = new System.Drawing.Point(169, 237);
            this.ip_textbox.MaxLength = 15;
            this.ip_textbox.Multiline = true;
            this.ip_textbox.Name = "ip_textbox";
            this.ip_textbox.Size = new System.Drawing.Size(282, 29);
            this.ip_textbox.TabIndex = 21;
            this.ip_textbox.Text = "127.0.0.1";
            // 
            // Connection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::Client.Properties.Resources.real_money_monopoly_background_free_vector;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(587, 398);
            this.Controls.Add(labelIp);
            this.Controls.Add(this.ip_textbox);
            this.Controls.Add(label1);
            this.Controls.Add(this.returnBtn);
            this.Controls.Add(this.roomTb);
            this.Controls.Add(this.btnChooseColor);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Connection";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Connection";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox roomTb;
        private System.Windows.Forms.Button btnChooseColor;
        private System.Windows.Forms.Button returnBtn;
        private System.Windows.Forms.TextBox ip_textbox;
    }
}