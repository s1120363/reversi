using System;
using System.Drawing;
using System.Reflection.Emit;
using System.Windows.Forms;

namespace ggame
{
    public partial class Form1 : Form
    {
        private const int GridSize = 8;
        private const int CellSize = 80;
        private int[,] board = new int[GridSize, GridSize]; // 0: empty, 1: black, 2: white
        private bool isBlackTurn = true;

        public Form1()
        {
            InitializeComponent();
            InitializeBoard();
            this.panel1.Paint += new PaintEventHandler(Panel1_Paint);
            this.panel1.MouseClick += new MouseEventHandler(Panel1_MouseClick);
            UpdateStatus();
        }

        private void InitializeBoard()
        {
            board[3, 3] = 2;
            board[4, 4] = 2;
            board[3, 4] = 1;
            board[4, 3] = 1;
        }

        private void Panel1_Paint(object sender, PaintEventArgs e)
        {
            DrawBoard(e.Graphics);

            // Draw valid move indicators
            List<Point> validMoves = GetValidMoves(isBlackTurn ? 1 : 2);
            foreach (Point move in validMoves)
            {
                int centerX = move.X * CellSize + CellSize / 2;
                int centerY = move.Y * CellSize + CellSize / 2;
                int radius = CellSize / 6;

                e.Graphics.FillEllipse(Brushes.Green, centerX - radius, centerY - radius, radius * 2, radius * 2);
            }
        }

        private void DrawBoard(Graphics g)
        {
            Pen pen = new Pen(Color.Black);

            // Draw grid lines
            for (int i = 0; i <= GridSize; i++)
            {
                g.DrawLine(pen, i * CellSize, 0, i * CellSize, GridSize * CellSize);
                g.DrawLine(pen, 0, i * CellSize, GridSize * CellSize, i * CellSize);
            }

            // Draw pieces
            for (int y = 0; y < GridSize; y++)
            {
                for (int x = 0; x < GridSize; x++)
                {
                    if (board[y, x] == 1)
                    {
                        g.FillEllipse(Brushes.Black, x * CellSize + 5, y * CellSize + 5, CellSize - 10, CellSize - 10);
                    }
                    else if (board[y, x] == 2)
                    {
                        g.FillEllipse(Brushes.White, x * CellSize + 5, y * CellSize + 5, CellSize - 10, CellSize - 10);
                    }
                }
            }
        }

        private void Panel1_MouseClick(object sender, MouseEventArgs e)
        {
            int x = e.X / CellSize;
            int y = e.Y / CellSize;

            if (x >= 0 && x < GridSize && y >= 0 && y < GridSize && board[y, x] == 0)
            {
                int currentPlayer = isBlackTurn ? 1 : 2;
                if (IsValidMove(x, y, currentPlayer))
                {
                    board[y, x] = currentPlayer;
                    FlipPieces(x, y, currentPlayer);
                    isBlackTurn = !isBlackTurn;
                    this.panel1.Invalidate();
                    UpdateStatus();

                    if (!HasValidMove(1) && !HasValidMove(2))
                    {
                        MessageBox.Show("Game Over!\n" + GetScores());
                    }
                    else if (!HasValidMove(isBlackTurn ? 1 : 2))
                    {
                        isBlackTurn = !isBlackTurn;
                        UpdateStatus();
                        MessageBox.Show("No valid moves, turn skipped!");
                    }
                }
            }
        }

        private bool IsValidMove(int x, int y, int player)
        {
            bool valid = false;
            int opponent = player == 1 ? 2 : 1;

            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    if (dx == 0 && dy == 0) continue;

                    int nx = x + dx, ny = y + dy;
                    bool hasOpponentBetween = false;

                    while (nx >= 0 && nx < GridSize && ny >= 0 && ny < GridSize && board[ny, nx] == opponent)
                    {
                        hasOpponentBetween = true;
                        nx += dx;
                        ny += dy;
                    }

                    if (hasOpponentBetween && nx >= 0 && nx < GridSize && ny >= 0 && ny < GridSize && board[ny, nx] == player)
                    {
                        valid = true;
                    }
                }
            }

            return valid;
        }

        private void FlipPieces(int x, int y, int player)
        {
            int opponent = player == 1 ? 2 : 1;

            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    if (dx == 0 && dy == 0) continue;

                    int nx = x + dx, ny = y + dy;
                    bool hasOpponentBetween = false;

                    while (nx >= 0 && nx < GridSize && ny >= 0 && ny < GridSize && board[ny, nx] == opponent)
                    {
                        hasOpponentBetween = true;
                        nx += dx;
                        ny += dy;
                    }

                    if (hasOpponentBetween && nx >= 0 && nx < GridSize && ny >= 0 && ny < GridSize && board[ny, nx] == player)
                    {
                        nx = x + dx;
                        ny = y + dy;

                        while (board[ny, nx] == opponent)
                        {
                            board[ny, nx] = player;
                            nx += dx;
                            ny += dy;
                        }
                    }
                }
            }
        }

        private bool HasValidMove(int player)
        {
            for (int y = 0; y < GridSize; y++)
            {
                for (int x = 0; x < GridSize; x++)
                {
                    if (board[y, x] == 0 && IsValidMove(x, y, player))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private string GetScores()
        {
            int blackCount = 0;
            int whiteCount = 0;

            for (int y = 0; y < GridSize; y++)
            {
                for (int x = 0; x < GridSize; x++)
                {
                    if (board[y, x] == 1)
                    {
                        blackCount++;
                    }
                    else if (board[y, x] == 2)
                    {
                        whiteCount++;
                    }
                }
            }

            return $"Black: {blackCount}, White: {whiteCount}";
        }

        private void UpdateStatus()
        {
            string currentPlayer = isBlackTurn ? "Black" : "White";
            label1.Text = $"Turn: {currentPlayer}\n{GetScores()}";
        }

        private List<Point> GetValidMoves(int player)
        {
            List<Point> validMoves = new List<Point>();

            for (int y = 0; y < GridSize; y++)
            {
                for (int x = 0; x < GridSize; x++)
                {
                    if (board[y, x] == 0 && IsValidMove(x, y, player))
                    {
                        validMoves.Add(new Point(x, y));
                    }
                }
            }

            return validMoves;
        }
    }
}