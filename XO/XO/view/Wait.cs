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
using System.Threading;

namespace XO.view
{
    public partial class Wait : Form
    {
        private Player player;
        private Communication com;
        private bool connected;
        private bool success;
        public Wait(Player player)
        {
            this.connected = true;
            this.player = player;
            this.success = true;

            InitializeComponent();
        }

        private void Wait_Load(object sender, EventArgs e)
        {
            this.Text = "Player: " + player;
            if(player == Player.X)
            {
                ip.Enabled = false;
            }
            
        }

        private void Start_Click(object sender, EventArgs e)
        {
            if(ip.Enabled != false)
            {
                if (!ip.Text.Contains("."))
                    return;

                string[] numbers = ip.Text.Split('.');
                if (numbers.Length != 4)
                    return;

                for(int i=0;i<numbers.Length;i++)
                {
                    try
                    {
                        int.Parse(numbers[i]);
                    }
                    catch(Exception ex)
                    {
                        return;
                    }
                }
            }

            if (player == Player.X)
                com = new Server(int.Parse(port.Value.ToString()));
            else
                com = new Client(ip.Text, int.Parse(port.Value.ToString()));

            Button b = sender as Button;
            b.Enabled = false;
            ip.Enabled = false;
            port.Enabled = false;

            new Thread(() =>
            {
                try
                {
                    connected = false;
                    com.Open();
                    if (!success)
                        throw new Exception();
                    connected = true;
                    this.BeginInvoke(new Action(() =>
                    {
                        this.Hide();
                    }));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("open communication error");
                    Application.Exit();
                }
            }).Start();
        }

        private void Wait_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible == false)
            {
                if(connected)
                {
                    Board board = new Board(player, com);
                    board.Show();
                }
            }
        }

        private void Wait_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(player == Player.X)
            {
                if (!connected)
                {
                    Client client = new Client("127.0.0.1", int.Parse(port.Value.ToString()));
                    client.Open();
                    client.Close();
                    success = false;
                    com.Close();
                    Application.Exit();
                }
            }
        }
    }
}
