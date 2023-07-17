using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MDIPAINT
{
    public partial class CanvasSizeForm : Form
    {
        MainForm mainForm;
        public CanvasSizeForm(MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            textBox1.Text = $"{((DocumentForm)mainForm.ActiveMdiChild).HeightImage}";
            textBox2.Text = $"{ ((DocumentForm)mainForm.ActiveMdiChild).WidhtImage}";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != "")
                ((DocumentForm)mainForm.ActiveMdiChild).WidhtImage = int.Parse(textBox2.Text);
            else
                textBox2.Text = $"{((DocumentForm)mainForm.ActiveMdiChild).WidhtImage}";

            if (textBox1.Text != "")
                ((DocumentForm)mainForm.ActiveMdiChild).HeightImage = int.Parse(textBox1.Text);
            else
                textBox1.Text = $"{((DocumentForm)mainForm.ActiveMdiChild).HeightImage}";
        }


        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(textBox2.Text, out int weight) || textBox2.Text == "")
            {
                if (weight <= 0 && textBox2.Text != "")
                {
                    MessageBox.Show("Вы ввели отрицательное число, введите положительное!");
                    textBox2.Clear();
                    textBox2.Text = $"{((DocumentForm)mainForm.ActiveMdiChild).WidhtImage}";
                }
            }
            else
            {
                MessageBox.Show("Вы ввели слишком больше число, либо вы ввели символ, вводите цифры!");
                textBox2.Clear();
                textBox2.Text = $"{((DocumentForm)mainForm.ActiveMdiChild).WidhtImage}";
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(textBox1.Text, out int height) || textBox1.Text == "")
            {
                if (height <= 0 && textBox1.Text != "")
                {
                    MessageBox.Show("Вы ввели отрицательное число, введите положительное!");
                    textBox1.Clear();
                    textBox1.Text = $"{((DocumentForm)mainForm.ActiveMdiChild).HeightImage}";
                }
            }
            else
            {
                MessageBox.Show("Вы ввели слишком больше число, либо вы ввели символ, вводите цифры!");
                textBox1.Clear();
                textBox1.Text = $"{((DocumentForm)mainForm.ActiveMdiChild).HeightImage}";
            }
        }
    }
}
