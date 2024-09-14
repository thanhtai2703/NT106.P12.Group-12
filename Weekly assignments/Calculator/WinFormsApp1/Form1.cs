using System;
using System.Data;
namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        float number1;
        float number2;
        string operand;
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void num1_Click(object sender, EventArgs e)
        {

        }

        private void num0_Click(object sender, EventArgs e)
        {
            Result.Text = Result.Text + "0";
            textBox1.Text = textBox1.Text + "0";
        }

        private void num1_Click_1(object sender, EventArgs e)
        {
            Result.Text = Result.Text + "1";
            textBox1.Text = textBox1.Text + "1";
        }

        private void num2_Click(object sender, EventArgs e)
        {
            Result.Text = Result.Text + "2";
            textBox1.Text = textBox1.Text + "2";
        }

        private void num3_Click(object sender, EventArgs e)
        {
            Result.Text = Result.Text + "3";
            textBox1.Text = textBox1.Text + "3";
        }

        private void num4_Click(object sender, EventArgs e)
        {
            Result.Text = Result.Text + "4";
            textBox1.Text = textBox1.Text + "4";
        }

        private void num5_Click(object sender, EventArgs e)
        {
            Result.Text = Result.Text + "5";
            textBox1.Text = textBox1.Text + "5";
        }

        private void num6_Click(object sender, EventArgs e)
        {
            Result.Text = Result.Text + "6";
            textBox1.Text = textBox1.Text + "6";
        }

        private void num7_Click(object sender, EventArgs e)
        {
            Result.Text = Result.Text + "7";
            textBox1.Text = textBox1.Text + "7";
        }

        private void num8_Click(object sender, EventArgs e)
        {
            Result.Text = Result.Text + "8";
            textBox1.Text = textBox1.Text + "8";
        }

        private void num9_Click(object sender, EventArgs e)
        {
            Result.Text = Result.Text + "9";
            textBox1.Text = textBox1.Text + "9";
        }

        private void dot_Click(object sender, EventArgs e)
        {

        }

        private void add_Click(object sender, EventArgs e)
        {
            if (Result.Text != "")
            {
                operand = "+";
                number1 = float.Parse(Result.Text);
                textBox1.Text = Result.Text + "+";
                Result.Clear();
            }
        }

        private void sub_Click(object sender, EventArgs e)
        {
            if (Result.Text != "")
            {
                operand = "-";
                number1 = float.Parse(Result.Text);
                textBox1.Text = Result.Text + "-";
                Result.Clear();
            }
        }

        private void mul_Click(object sender, EventArgs e)
        {
            if (Result.Text != "")
            {
                operand = "*";
                number1 = float.Parse(Result.Text);
                textBox1.Text = Result.Text + "*";
                Result.Clear();
            }
        }

        private void div_Click(object sender, EventArgs e)
        {
            if (Result.Text != "")
            {
                operand = "/";
                number1 = float.Parse(Result.Text);
                textBox1.Text = Result.Text + "/";
                Result.Clear();
            }
        }

        private void clear_Click(object sender, EventArgs e)
        {
            Result.Clear();
        }

        private void equal_Click(object sender, EventArgs e)
        {
            if (operand == "+")
            {
                number2 = number1 + float.Parse(Result.Text);
                Result.Text = number2.ToString();
            }
            if (operand == "-")
            {
                number2 = number1 - float.Parse(Result.Text);
                Result.Text = number2.ToString();
            }
            if (operand == "*")
            {
                number2 = number1 * float.Parse(Result.Text);
                Result.Text = number2.ToString();
            }
            if (operand == "/")
            {

                if (float.Parse(Result.Text) == 0)
                {
                    Result.Text = "Math error !!";
                }
                else
                {
                    number2 = number1 / float.Parse(Result.Text);

                    Result.Text = number2.ToString();
                }
                
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Result.Clear();
            textBox1.Clear();
        }
    }
}