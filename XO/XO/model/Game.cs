using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.Control;

namespace XO.model
{
    public class Game
    {
        private int dim;
        private int size;
        private Player player;
        private Player current;
        private Matrix<Player> wins;
        private Matrix<Matrix<Button>> matrix;
        private bool multy;

        public Game(int dim, int size, EventHandler click, ControlCollection controls, bool multy, Player player, Player current)
        {
            this.dim = dim;
            this.size = size;
            this.player = player;
            this.current = current;
            this.wins = new Matrix<Player>(dim, dim, Player.None);
            this.matrix = new Matrix<Matrix<Button>>(dim, dim, null);
            this.multy = multy;

            for (int i = 0; i < dim; i++)
            {
                for (int j = 0; j < dim; j++)
                {
                    Matrix<Button> innerMatrix = new Matrix<Button>(dim, dim, null);
                    for (int ii = 0; ii < dim; ii++)
                    {
                        for (int jj = 0; jj < dim; jj++)
                        {
                            Button b = new Button();
                            b.Name = i + "," + j + "," + ii + "," + jj;
                            b.Text = "";
                            b.Size = new Size(size, size);
                            b.Location = new Point(j * dim * size + jj * size, i * dim * size + ii * size);
                            if ((i + j) % 2 == 0)
                                b.BackColor = Color.LightGray;
                            else
                                b.BackColor = Color.White;
                            if(multy)
                            {
                                if (this.player == Player.X)
                                    b.Enabled = true;
                                else
                                    b.Enabled = false;
                            }
                            b.Click += click;
                            b.Font = new Font("", 24, FontStyle.Bold);
                            innerMatrix.Set(ii, jj, b);
                            controls.Add(b);
                        }
                    }
                    matrix.Set(i, j, innerMatrix);
                }
            }
        }

        public Button GetButton(int i, int j, int ii, int jj)
        {
            return matrix.Get(i, j).Get(ii, jj);
        }
    
        public State Press(int i, int j, int ii, int jj)
        {
            Button b = this.GetButton(i, j, ii, jj);

            if (!b.Text.Equals(""))
                return State.Illegal;

            b.Text = current.ToString();
            if (current == Player.X)
            {
                if (wins.Get(i, j) == Player.None)
                    b.BackColor = Color.Red;
                current = Player.O;
            }
            else
            {
                if (wins.Get(i,j)==Player.None)
                    b.BackColor = Color.Blue;
                current = Player.X;
            }

            Enable(matrix, ii, jj);

            if (wins.Get(i, j) == Player.None)
            {
                Matrix<Button> buttons = matrix.Get(i,j);
                Color c;
                Player win = Solve(ButtonsToPlayers(buttons));
                wins.Set(i, j, win);
                if (win != Player.None)
                {
                    if (win == Player.O)
                    {
                        c = Color.LightBlue;
                    }
                    else
                    {
                        c = Color.LightCoral;
                    }
                    for (int x = 0; x < dim; x++)
                    {
                        for (int y = 0; y < dim; y++)
                        {
                            buttons.Get(x, y).BackColor = c;

                        }
                    }
                }
                Player player = Solve(wins);
                if (player != Player.None)
                {
                    return State.End;
                }
            }

            return State.Legal;
        }

        private Player Solve(Matrix<Player> board)
        {
            Player player;
            for (int i = 0; i < dim; i++)
            {
                player = board.Get(i, 0);
                if (player != Player.None)
                {
                    for (int j = 0; j < dim; j++)
                    {
                        if (player != (board.Get(i, j)))
                        {
                            goto not1;
                        }
                    }
                    return player;
                not1:;
                }
            }

            for (int j = 0; j < dim; j++)
            {
                player = board.Get(0, j);
                if (player != Player.None)
                {
                    for (int i = 0; i < dim; i++)
                    {
                        if (player != board.Get(i, j))
                        {
                            goto not2;
                        }
                    }
                    return player;
                not2:;
                }
            }

            player = board.Get(0, 0);
            if (player != Player.None)
            {
                for (int i = 0; i < dim; i++)
                {
                    if (player != board.Get(i, i))
                    {
                        goto not3;
                    }
                }
                return player;
            }

        not3:

            player = board.Get(0, dim - 1);
            if (player != Player.None)
            {
                for (int i = 0; i < dim; i++)
                {
                    if (player != board.Get(i, dim - 1 - i))
                    {
                        goto not4;
                    }
                }
                return player;
            }

        not4:
            return Player.None;

        }
    
        private Matrix<Player> ButtonsToPlayers(Matrix<Button> buttons)
        {
            Matrix<Player> players = new Matrix<Player>(buttons.GetRows(), buttons.GetCols(), Player.None);

            for (int i = 0; i < players.GetRows(); i++)
            {
                for (int j = 0; j < players.GetCols(); j++)
                {
                    string text = buttons.Get(i, j).Text;
                    Player player;
                    if (text.Equals("X"))
                        player = Player.X;
                    else if (text.Equals("O"))
                        player = Player.O;
                    else
                        player = Player.None;
                    players.Set(i, j, player);
                }
            }
            return players;
        }
    
        private void Enable(Matrix<Matrix<Button>> board, int x, int y)
        {
            for (int i = 0; i < board.GetRows(); i++)
            {
                for (int j = 0; j < board.GetCols(); j++)
                {
                    Matrix<Button> buttons = board.Get(i, j);
                    if (multy && player!=current)
                    {
                        for (int ii = 0; ii < buttons.GetRows(); ii++)
                        {
                            for (int jj = 0; jj < buttons.GetCols(); jj++)
                            {
                                buttons.Get(ii, jj).Enabled = false;
                            }
                        }
                    }
                    else
                    {
                        if(i == x && j == y)
                        {
                            for (int ii = 0; ii < buttons.GetRows(); ii++)
                            {
                                for (int jj = 0; jj < buttons.GetCols(); jj++)
                                {
                                    buttons.Get(ii, jj).Enabled = true;
                                }
                            }
                        }
                        else
                        {
                            for (int ii = 0; ii < buttons.GetRows(); ii++)
                            {
                                for (int jj = 0; jj < buttons.GetCols(); jj++)
                                {
                                    buttons.Get(ii, jj).Enabled = false;
                                }
                            }
                        }
                    }
                }
            }  
        }
    
        public Player Solve()
        {
            return Solve(wins);
        }
    }
}
