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
    public partial class StarForm : Form
    {
        MainForm mainForm;

        public StarForm(MainForm mainForm)
        {
            InitializeComponent();
            textBox1.Text = $"{DocumentForm.cntStarBeams}";
            textBox2.Text = $"{DocumentForm.radiiRatio}";
            this.mainForm = mainForm;
        }
        // кол-во лучей
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(textBox1.Text, out int newCntStarBeams) || textBox1.Text == "")
            {
                if ((newCntStarBeams <= 3 || newCntStarBeams >= 10) && textBox1.Text != "")
                {
                    MessageBox.Show("Можно вводить лишь целые положительные числа от 4 до 9 ! Вы ввели неположительное целое число!");
                    textBox1.Clear();
                }
            }
            else
            {
                MessageBox.Show("Вы ввели слишком больше число, либо вы ввели некорректный символ!");
                textBox1.Clear();
                textBox1.Text = $"{DocumentForm.cntStarBeams}";
            }
        }
        // отношение внутреннего и внешнего радиуса
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (float.TryParse(textBox2.Text, out float newRadiiRatio) || textBox2.Text == "")
            {
                if ((newRadiiRatio < 0 || newRadiiRatio > 1) && textBox2.Text != "")
                {
                    MessageBox.Show("Можно вводить лишь числа от 0 до 1 (Если число не целое, то нужно вводить его через запятую)! Вы ввели неккоректное число!");
                    textBox2.Clear();
                    textBox2.Text = $"{DocumentForm.radiiRatio}";
                }
            }
            else
            {
                MessageBox.Show("Вы ввели слишком большое число, либо вы ввели некорректный символ!");
                textBox2.Clear();
                textBox2.Text = $"{DocumentForm.radiiRatio}";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            mainForm.tools = Tools.Star;
            int newCntStartBeams = DocumentForm.cntStarBeams;
            if (textBox1.Text != "")
                newCntStartBeams = int.Parse(textBox1.Text);
            DocumentForm.cntStarBeams = newCntStartBeams;

            float radiiRatio = DocumentForm.radiiRatio;
            if (textBox2.Text != "")
                radiiRatio = float.Parse(textBox2.Text);
            DocumentForm.radiiRatio = radiiRatio;
        }
    }
}
