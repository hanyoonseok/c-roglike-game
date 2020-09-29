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
    public partial class Form8 : Form
    {
        public Form8()
        {
            InitializeComponent();
        }

        private void Form8_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Application.Exit();
        }

        private void Form8_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
