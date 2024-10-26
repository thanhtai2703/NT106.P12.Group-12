namespace Client
{
    partial class ColorChoosing
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ColorChoosing));
            this.chooseBluePlayerBtn = new System.Windows.Forms.Button();
            this.chooseRedPlayerBtn = new System.Windows.Forms.Button();
            this.returnBtn = new System.Windows.Forms.Button();
            this.connect_button = new System.Windows.Forms.Button();
            this.tbColor = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // chooseBluePlayerBtn
            // 
            this.chooseBluePlayerBtn.BackColor = System.Drawing.Color.Transparent;
            this.chooseBluePlayerBtn.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("chooseBluePlayerBtn.BackgroundImage")));
            this.chooseBluePlayerBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.chooseBluePlayerBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chooseBluePlayerBtn.FlatAppearance.BorderSize = 0;
            this.chooseBluePlayerBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.chooseBluePlayerBtn.Location = new System.Drawing.Point(412, 95);
            this.chooseBluePlayerBtn.Name = "chooseBluePlayerBtn";
            this.chooseBluePlayerBtn.Size = new System.Drawing.Size(150, 172);
            this.chooseBluePlayerBtn.TabIndex = 30;
            this.chooseBluePlayerBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.chooseBluePlayerBtn.UseVisualStyleBackColor = false;
            this.chooseBluePlayerBtn.Click += new System.EventHandler(this.chooseBluePlayerBtn_Click);
            // 
            // chooseRedPlayerBtn
            // 
            this.chooseRedPlayerBtn.BackColor = System.Drawing.Color.Transparent;
            this.chooseRedPlayerBtn.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("chooseRedPlayerBtn.BackgroundImage")));
            this.chooseRedPlayerBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.chooseRedPlayerBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chooseRedPlayerBtn.FlatAppearance.BorderSize = 0;
            this.chooseRedPlayerBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.chooseRedPlayerBtn.Location = new System.Drawing.Point(12, 95);
            this.chooseRedPlayerBtn.Name = "chooseRedPlayerBtn";
            this.chooseRedPlayerBtn.Size = new System.Drawing.Size(157, 172);
            this.chooseRedPlayerBtn.TabIndex = 29;
            this.chooseRedPlayerBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.chooseRedPlayerBtn.UseVisualStyleBackColor = false;
            this.chooseRedPlayerBtn.Click += new System.EventHandler(this.chooseRedPlayerBtn_Click);
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
            this.returnBtn.Location = new System.Drawing.Point(186, 323);
            this.returnBtn.Name = "returnBtn";
            this.returnBtn.Size = new System.Drawing.Size(124, 33);
            this.returnBtn.TabIndex = 28;
            this.returnBtn.Text = "Exit";
            this.returnBtn.UseVisualStyleBackColor = false;
            this.returnBtn.Click += new System.EventHandler(this.returnBtn_Click);
            this.returnBtn.Paint += new System.Windows.Forms.PaintEventHandler(this.button1_Paint);
            this.returnBtn.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.returnBtn.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // connect_button
            // 
            this.connect_button.BackColor = System.Drawing.Color.White;
            this.connect_button.Cursor = System.Windows.Forms.Cursors.Hand;
            this.connect_button.FlatAppearance.BorderSize = 0;
            this.connect_button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.connect_button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.connect_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.connect_button.Font = new System.Drawing.Font("Elephant", 10.8F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.connect_button.ForeColor = System.Drawing.Color.Black;
            this.connect_button.Location = new System.Drawing.Point(316, 323);
            this.connect_button.Name = "connect_button";
            this.connect_button.Size = new System.Drawing.Size(124, 33);
            this.connect_button.TabIndex = 24;
            this.connect_button.Text = "Connect";
            this.connect_button.UseVisualStyleBackColor = false;
            this.connect_button.Click += new System.EventHandler(this.connect_button_Click);
            this.connect_button.Paint += new System.Windows.Forms.PaintEventHandler(this.button1_Paint);
            this.connect_button.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.connect_button.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // tbColor
            // 
            this.tbColor.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbColor.Enabled = false;
            this.tbColor.Location = new System.Drawing.Point(216, 196);
            this.tbColor.Multiline = true;
            this.tbColor.Name = "tbColor";
            this.tbColor.Size = new System.Drawing.Size(151, 22);
            this.tbColor.TabIndex = 31;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Elephant", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(226, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 21);
            this.label1.TabIndex = 32;
            this.label1.Text = "Choose Color";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(212, 97);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(118, 19);
            this.label2.TabIndex = 34;
            this.label2.Text = "Nhập tên của bạn";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(216, 123);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(151, 27);
            this.txtName.TabIndex = 35;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.IndianRed;
            this.label3.Location = new System.Drawing.Point(30, 279);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 20);
            this.label3.TabIndex = 36;
            this.label3.Text = "chưa chọn....";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.PaleTurquoise;
            this.label4.Location = new System.Drawing.Point(439, 275);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(94, 20);
            this.label4.TabIndex = 37;
            this.label4.Text = "Chưa chọn.....";
            // 
            // ColorChoosing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::Client.Properties.Resources.monopoly_board_games_element_background_free_vector;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(583, 394);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbColor);
            this.Controls.Add(this.chooseBluePlayerBtn);
            this.Controls.Add(this.chooseRedPlayerBtn);
            this.Controls.Add(this.returnBtn);
            this.Controls.Add(this.connect_button);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ColorChoosing";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Color choosing";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Button chooseBluePlayerBtn;
        public System.Windows.Forms.Button chooseRedPlayerBtn;
        private System.Windows.Forms.Button returnBtn;
        private System.Windows.Forms.Button connect_button;
        private System.Windows.Forms.TextBox tbColor;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}