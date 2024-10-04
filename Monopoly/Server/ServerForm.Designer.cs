namespace Server
{
    partial class ServerForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServerForm));
            this.tbLog = new System.Windows.Forms.TextBox();
            this.tsServerControl = new System.Windows.Forms.ToolStrip();
            this.btnTurnOn = new System.Windows.Forms.ToolStripButton();
            this.btnTurnOff = new System.Windows.Forms.ToolStripButton();
            this.tsServerControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbLog
            // 
            this.tbLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbLog.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.tbLog.Location = new System.Drawing.Point(0, 32);
            this.tbLog.Multiline = true;
            this.tbLog.Name = "tbLog";
            this.tbLog.ReadOnly = true;
            this.tbLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbLog.Size = new System.Drawing.Size(675, 392);
            this.tbLog.TabIndex = 0;
            // 
            // tsServerControl
            // 
            this.tsServerControl.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.tsServerControl.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tsServerControl.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsServerControl.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.tsServerControl.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnTurnOn,
            this.btnTurnOff});
            this.tsServerControl.Location = new System.Drawing.Point(0, 0);
            this.tsServerControl.Name = "tsServerControl";
            this.tsServerControl.Size = new System.Drawing.Size(675, 32);
            this.tsServerControl.TabIndex = 5;
            this.tsServerControl.Text = "toolStrip1";
            // 
            // btnTurnOn
            // 
            this.btnTurnOn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnTurnOn.Image = ((System.Drawing.Image)(resources.GetObject("btnTurnOn.Image")));
            this.btnTurnOn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnTurnOn.Name = "btnTurnOn";
            this.btnTurnOn.Size = new System.Drawing.Size(82, 29);
            this.btnTurnOn.Text = "Turn on";
            this.btnTurnOn.Click += new System.EventHandler(this.btnTurnOn_Click);
            // 
            // btnTurnOff
            // 
            this.btnTurnOff.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnTurnOff.AutoToolTip = false;
            this.btnTurnOff.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnTurnOff.Image = ((System.Drawing.Image)(resources.GetObject("btnTurnOff.Image")));
            this.btnTurnOff.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnTurnOff.Name = "btnTurnOff";
            this.btnTurnOff.Size = new System.Drawing.Size(146, 29);
            this.btnTurnOff.Text = "Close / Turn off";
            this.btnTurnOff.Click += new System.EventHandler(this.btnTurnOff_Click);
            // 
            // ServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(675, 424);
            this.ControlBox = false;
            this.Controls.Add(this.tbLog);
            this.Controls.Add(this.tsServerControl);
            this.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "ServerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Server. State: on / Server. State: off";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ServerForm_FormClosed);
            this.tsServerControl.ResumeLayout(false);
            this.tsServerControl.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        public System.Windows.Forms.TextBox tbLog;
        private System.Windows.Forms.ToolStrip tsServerControl;
        private System.Windows.Forms.ToolStripButton btnTurnOn;
        private System.Windows.Forms.ToolStripButton btnTurnOff;
    }
}

