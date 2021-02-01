using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XO.view
{
    public partial class Start : Form
    {
        public Start()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Wait wait = new Wait(model.Player.X);
            this.Hide();
            wait.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Wait wait = new Wait(model.Player.O);
            this.Hide();
            wait.Show();
        }
    }
}
