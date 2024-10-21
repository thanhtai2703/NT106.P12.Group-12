﻿namespace Client
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Connection));
            this.roomTb = new System.Windows.Forms.TextBox();
            this.btnChooseColor = new System.Windows.Forms.Button();
            this.returnBtn = new System.Windows.Forms.Button();
            label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = System.Drawing.Color.Transparent;
            label1.Font = new System.Drawing.Font("Elephant", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label1.Location = new System.Drawing.Point(164, 141);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(87, 30);
            label1.TabIndex = 20;
            label1.Text = "Phòng";
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
            this.btnChooseColor.Location = new System.Drawing.Point(321, 221);
            this.btnChooseColor.Name = "btnChooseColor";
            this.btnChooseColor.Size = new System.Drawing.Size(147, 45);
            this.btnChooseColor.TabIndex = 13;
            this.btnChooseColor.Text = "Next";
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
            this.returnBtn.Location = new System.Drawing.Point(169, 221);
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
            // Connection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::Client.Properties.Resources.real_money_monopoly_background_free_vector;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(587, 398);
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
    }
}