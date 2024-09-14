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
        string temp="";
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void num1_Click(object sender, EventArgs e)
        {

        }

        private void num0_Click(object sender, EventArgs e)
        {
            //Result.Text = Result.Text + "0";
            textBox1.Text = textBox1.Text + "0";
            temp = temp + "0";
        }

        private void num1_Click_1(object sender, EventArgs e)
        {
            //Result.Text = Result.Text + "1";
            textBox1.Text = textBox1.Text + "1";
            temp = temp + "1";
        }

        private void num2_Click(object sender, EventArgs e)
        {
            //Result.Text = Result.Text + "2";
            textBox1.Text = textBox1.Text + "2";
            temp = temp + "2";
        }

        private void num3_Click(object sender, EventArgs e)
        {
            //Result.Text = Result.Text + "3";
            textBox1.Text = textBox1.Text + "3";
            temp = temp + "3";
        }

        private void num4_Click(object sender, EventArgs e)
        {
            //Result.Text = Result.Text + "4";
            textBox1.Text = textBox1.Text + "4";
            temp = temp + "4";
        }

        private void num5_Click(object sender, EventArgs e)
        {
            //Result.Text = Result.Text + "5";
            textBox1.Text = textBox1.Text + "5";
            temp = temp + "5";
        }

        private void num6_Click(object sender, EventArgs e)
        {
            //Result.Text = Result.Text + "6";
            textBox1.Text = textBox1.Text + "6";
            temp = temp + "6";
        }

        private void num7_Click(object sender, EventArgs e)
        {
            //Result.Text = Result.Text + "7";
            textBox1.Text = textBox1.Text + "7";
            temp = temp + "7";
        }

        private void num8_Click(object sender, EventArgs e)
        {
            //Result.Text = Result.Text + "8";
            textBox1.Text = textBox1.Text + "8";
            temp = temp + "8";
        }

        private void num9_Click(object sender, EventArgs e)
        {
           // Result.Text = Result.Text + "9";
            textBox1.Text = textBox1.Text + "9";
            temp = temp + "9";
        }

        private void dot_Click(object sender, EventArgs e)
        {

        }

        private void add_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                operand = "+";
                number1 = float.Parse(temp);
                textBox1.Text = temp + "+";
                temp = "";
                //Result.Clear();
            }
            
        }

        private void sub_Click(object sender, EventArgs e)
        {

            if (textBox1.Text != "")
            {
                operand = "-";
                number1 = float.Parse(temp);
                textBox1.Text = temp + "-";
                temp = "";
                // Result.Clear();
            }
           
        }

        private void mul_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                operand = "*";
                number1 = float.Parse(temp);
                textBox1.Text = temp + "*";
                // Result.Clear();
                temp = "";
            }
            
        }

        private void div_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                operand = "/";
                number1 = float.Parse(temp);
                textBox1.Text = temp + "/";
                // Result.Clear();
                temp = "";
            }
            
        }

        private void clear_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            Result.Clear();
            temp = "";
        }

        private void equal_Click(object sender, EventArgs e)
        {
            if (operand == "+")
            {
                number2 = number1 + float.Parse(temp);
                Result.Text = number2.ToString();
            }
            if (operand == "-")
            {
                number2 = number1 - float.Parse(temp);
                Result.Text = number2.ToString();
            }
            if (operand == "*")
            {
                number2 = number1 * float.Parse(temp);
                Result.Text = number2.ToString();
            }
            if (operand == "/")
            {

                if (float.Parse(temp) == 0)
                {
                    Result.Text = "Math error !!";
                }
                else
                {
                    number2 = number1 / float.Parse(temp);

                    Result.Text = number2.ToString();
                }
                
            }
            textBox1.Clear();
            temp = "";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Result.Clear();
            
        }
    }
}