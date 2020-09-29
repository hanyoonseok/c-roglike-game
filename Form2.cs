using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Final
{
    public partial class Form2 : Form
    {
        string text1;
        string text2;
        public Form2(string text1, string text2)
        {
            InitializeComponent();
            this.text1 = text1;
            this.text2 = text2;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            label2.Text = text1;
            label3.Text = text2;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
