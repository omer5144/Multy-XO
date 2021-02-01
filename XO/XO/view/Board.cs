using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using XO.communication;
using XO.model;

namespace XO.view
{
    public partial class Board : Form
    {
        private const int dim = 3;
        private const int size = 50;
        private Player player;
        private Game game;
        private Communication com;
        public Board(Player player, Communication com)
        {
            this.player = player;
            this.com = com;
            InitializeComponent();
        }

        private void Board_Load(object sender, EventArgs e)
        {
            game = new Game(dim, size, ButtonClick, this.Controls, true, player, Player.X);
            this.Size = new Size(size * dim * dim + 17, size * dim * dim + 40);
            this.Text = "Player: " + player;
            if(player == Player.O)
            {
                new Thread(() =>
                {
                    try
                    {
                        string text = com.Recv();
                        if (text.Equals(""))
                            throw new Exception();
                        string[] click = text.Split(',');
                        this.BeginInvoke(new Action(() =>
                        {
                            game.Press(int.Parse(click[0]), int.Parse(click[1]), int.Parse(click[2]), int.Parse(click[3]));
                        }));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("recv communication error");
                        Application.Exit();
                    }
                }).Start();
            }
            
        }

        private void ButtonClick(object sender, EventArgs e)
        {
            Button b = sender as Button;
            string[] location = b.Name.Split(',');
            State state = game.Press(int.Parse(location[0]), int.Parse(location[1]), int.Parse(location[2]), int.Parse(location[3]));
            if(state != State.Illegal)
            {
                com.Send(b.Name);
                if (state == State.Legal)
                {
                    new Thread(() =>
                    {
                        try
                        {
                            string text = com.Recv();
                            if (text.Equals(""))
                                throw new Exception();
                            string[] click = text.Split(',');
                            this.BeginInvoke(new Action(() =>
                            {
                                if(game.Press(int.Parse(click[0]), int.Parse(click[1]), int.Parse(click[2]), int.Parse(click[3])) == State.End)
                                {
                                    this.Hide();
                                    Win win = new Win(game.Solve() ,com);
                                    win.Show();
                                }
                            }));
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("recv communication error");
                            Application.Exit();
                        }
                    }).Start();

                }
                else
                {
                    this.Hide();
                    Win win = new Win(game.Solve(), com);
                    win.Show();

                }
            }
        }

        private void Board_FormClosed(object sender, FormClosedEventArgs e)
        {
            com.Close();
            Application.Exit();
        }
    }
}
