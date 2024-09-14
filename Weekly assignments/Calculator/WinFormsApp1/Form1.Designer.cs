namespace WinFormsApp1
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
            Result = new TextBox();
            num0 = new Button();
            dot = new Button();
            add = new Button();
            equal = new Button();
            clear = new Button();
            button6 = new Button();
            num1 = new Button();
            num2 = new Button();
            num3 = new Button();
            sub = new Button();
            num4 = new Button();
            num5 = new Button();
            num6 = new Button();
            mul = new Button();
            num7 = new Button();
            num8 = new Button();
            num9 = new Button();
            div = new Button();
            textBox1 = new TextBox();
            SuspendLayout();
            // 
            // Result
            // 
            Result.Font = new Font("Segoe UI Symbol", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            Result.ForeColor = SystemColors.WindowText;
            Result.Location = new Point(246, 62);
            Result.Multiline = true;
            Result.Name = "Result";
            Result.Size = new Size(154, 55);
            Result.TabIndex = 0;
            Result.TextChanged += textBox1_TextChanged;
            // 
            // num0
            // 
            num0.Location = new Point(12, 351);
            num0.Name = "num0";
            num0.Size = new Size(148, 65);
            num0.TabIndex = 1;
            num0.Text = "0";
            num0.UseVisualStyleBackColor = true;
            num0.Click += num0_Click;
            // 
            // dot
            // 
            dot.Location = new Point(166, 348);
            dot.Name = "dot";
            dot.Size = new Size(74, 70);
            dot.TabIndex = 2;
            dot.Text = ".";
            dot.UseVisualStyleBackColor = true;
            dot.Click += dot_Click;
            // 
            // add
            // 
            add.Location = new Point(246, 348);
            add.Name = "add";
            add.Size = new Size(74, 70);
            add.TabIndex = 3;
            add.Text = "+";
            add.UseVisualStyleBackColor = true;
            add.Click += add_Click;
            // 
            // equal
            // 
            equal.Location = new Point(326, 272);
            equal.Name = "equal";
            equal.Size = new Size(74, 149);
            equal.TabIndex = 4;
            equal.Text = "=";
            equal.UseVisualStyleBackColor = true;
            equal.Click += equal_Click;
            // 
            // clear
            // 
            clear.Location = new Point(326, 199);
            clear.Name = "clear";
            clear.Size = new Size(74, 70);
            clear.TabIndex = 5;
            clear.Text = "C";
            clear.UseVisualStyleBackColor = true;
            clear.Click += clear_Click;
            // 
            // button6
            // 
            button6.Location = new Point(326, 123);
            button6.Name = "button6";
            button6.Size = new Size(74, 70);
            button6.TabIndex = 6;
            button6.Text = "CE";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // num1
            // 
            num1.Location = new Point(12, 272);
            num1.Name = "num1";
            num1.Size = new Size(74, 70);
            num1.TabIndex = 7;
            num1.Text = "1";
            num1.UseVisualStyleBackColor = true;
            num1.Click += num1_Click_1;
            // 
            // num2
            // 
            num2.Location = new Point(86, 272);
            num2.Name = "num2";
            num2.Size = new Size(74, 70);
            num2.TabIndex = 8;
            num2.Text = "2";
            num2.UseVisualStyleBackColor = true;
            num2.Click += num2_Click;
            // 
            // num3
            // 
            num3.Location = new Point(166, 272);
            num3.Name = "num3";
            num3.Size = new Size(74, 70);
            num3.TabIndex = 9;
            num3.Text = "3";
            num3.UseVisualStyleBackColor = true;
            num3.Click += num3_Click;
            // 
            // sub
            // 
            sub.Location = new Point(246, 272);
            sub.Name = "sub";
            sub.Size = new Size(74, 70);
            sub.TabIndex = 10;
            sub.Text = "-";
            sub.UseVisualStyleBackColor = true;
            sub.Click += sub_Click;
            // 
            // num4
            // 
            num4.Location = new Point(12, 199);
            num4.Name = "num4";
            num4.Size = new Size(74, 70);
            num4.TabIndex = 11;
            num4.Text = "4";
            num4.UseVisualStyleBackColor = true;
            num4.Click += num4_Click;
            // 
            // num5
            // 
            num5.Location = new Point(86, 199);
            num5.Name = "num5";
            num5.Size = new Size(74, 70);
            num5.TabIndex = 12;
            num5.Text = "5";
            num5.UseVisualStyleBackColor = true;
            num5.Click += num5_Click;
            // 
            // num6
            // 
            num6.Location = new Point(166, 199);
            num6.Name = "num6";
            num6.Size = new Size(74, 70);
            num6.TabIndex = 13;
            num6.Text = "6";
            num6.UseVisualStyleBackColor = true;
            num6.Click += num6_Click;
            // 
            // mul
            // 
            mul.Location = new Point(246, 199);
            mul.Name = "mul";
            mul.Size = new Size(74, 70);
            mul.TabIndex = 14;
            mul.Text = "*";
            mul.UseVisualStyleBackColor = true;
            mul.Click += mul_Click;
            // 
            // num7
            // 
            num7.Location = new Point(12, 123);
            num7.Name = "num7";
            num7.Size = new Size(74, 70);
            num7.TabIndex = 15;
            num7.Text = "7";
            num7.UseVisualStyleBackColor = true;
            num7.Click += num7_Click;
            // 
            // num8
            // 
            num8.Location = new Point(86, 123);
            num8.Name = "num8";
            num8.Size = new Size(74, 70);
            num8.TabIndex = 16;
            num8.Text = "8";
            num8.UseVisualStyleBackColor = true;
            num8.Click += num8_Click;
            // 
            // num9
            // 
            num9.Location = new Point(166, 123);
            num9.Name = "num9";
            num9.Size = new Size(74, 70);
            num9.TabIndex = 17;
            num9.Text = "9";
            num9.UseVisualStyleBackColor = true;
            num9.Click += num9_Click;
            // 
            // div
            // 
            div.Location = new Point(246, 123);
            div.Name = "div";
            div.Size = new Size(74, 70);
            div.TabIndex = 18;
            div.Text = "/";
            div.UseVisualStyleBackColor = true;
            div.Click += div_Click;
            // 
            // textBox1
            // 
            textBox1.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            textBox1.Location = new Point(12, 7);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(388, 49);
            textBox1.TabIndex = 19;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(422, 433);
            Controls.Add(textBox1);
            Controls.Add(div);
            Controls.Add(num9);
            Controls.Add(num8);
            Controls.Add(num7);
            Controls.Add(mul);
            Controls.Add(num6);
            Controls.Add(num5);
            Controls.Add(num4);
            Controls.Add(sub);
            Controls.Add(num3);
            Controls.Add(num2);
            Controls.Add(num1);
            Controls.Add(button6);
            Controls.Add(clear);
            Controls.Add(equal);
            Controls.Add(add);
            Controls.Add(dot);
            Controls.Add(num0);
            Controls.Add(Result);
            Name = "Form1";
            Text = "Calculator";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox Result;
        private Button num0;
        private Button dot;
        private Button add;
        private Button equal;
        private Button clear;
        private Button button6;
        private Button num1;
        private Button num2;
        private Button num3;
        private Button sub;
        private Button num4;
        private Button num5;
        private Button num6;
        private Button mul;
        private Button num7;
        private Button num8;
        private Button num9;
        private Button div;
        private TextBox textBox1;
    }
}
