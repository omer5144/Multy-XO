using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XO.communication;
using XO.model;

namespace XO.view
{
    public partial class Win : Form
    {
        private Communication com;
        private Player player;
        public Win(Player win, Communication com)
        {
            this.com = com;
            this.player = win;
            InitializeComponent();
        }

        private void Win_Load(object sender, EventArgs e)
        {
            this.Text = "Player: " + player;

            if (player == Player.X)
            {
                this.BackColor = Color.Red;
                winner.Text = "X IS THE WINNER!";
            }
            else
            {
                this.BackColor = Color.Blue;
                winner.Text = "O IS THE WINNER!";

            }
        }

        private void Win_FormClosed(object sender, FormClosedEventArgs e)
        {
            com.Close();
            Application.Exit();
        }
    }
}
