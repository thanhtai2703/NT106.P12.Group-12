using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculator
{
    public partial class Form1 : Form
    {
        Double resultValue = 0;
        String operationPerformed = "";
        bool isOperationPerformed = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, EventArgs e)
        {
            if (textBox_Result.Text == "0" || (isOperationPerformed))
                textBox_Result.Clear();

            isOperationPerformed = false;
            Button button = (Button)sender;
            if (button.Text==".")
            {
                if(!textBox_Result.Text.Contains("."))
                    textBox_Result.Text = textBox_Result.Text + button.Text;

            }
            else
                textBox_Result.Text = textBox_Result.Text + button.Text;

        }

        private void operator_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            if (resultValue != 0)
            {
                btnEqual.PerformClick();
                operationPerformed = button.Text;
                isOperationPerformed = true;
            }
            else
            {
                operationPerformed = button.Text;
                resultValue = Double.Parse(textBox_Result.Text);
                isOperationPerformed = true;
            }
        }

        private void btnCE_Click(object sender, EventArgs e)
        {
            textBox_Result.Text = "0";
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            textBox_Result.Text = "0";
            resultValue = 0;
        }

        private void btnEqual_Click(object sender, EventArgs e)
        {
            switch ( operationPerformed)
            {
                case "+":
                    textBox_Result.Text = (resultValue + Double.Parse(textBox_Result.Text)).ToString(); break;
                case "-":
                    textBox_Result.Text = (resultValue - Double.Parse(textBox_Result.Text)).ToString(); break;
                case "*":
                    textBox_Result.Text = (resultValue * Double.Parse(textBox_Result.Text)).ToString(); break;
                case "/":
                    textBox_Result.Text = (resultValue / Double.Parse(textBox_Result.Text)).ToString(); break;
                default:
                    break;
            }
            resultValue = Double.Parse(textBox_Result.Text);
        }
    }
}
