using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ggame
{
    public partial class Form1 : Form
    {
        private const int GridSize = 8;
        private const int CellSize = 80;
        private int[,] board = new int[GridSize, GridSize]; // 0: 空，1: 黑棋，2: 白棋
        private bool isBlackTurn = true;
        private bool isSinglePlayer = true; // 标识是否是单人模式
        private enum Difficulty { Easy, Normal, Hard }
        private Difficulty currentDifficulty = Difficulty.Easy; // 当前难度
        private Point? lastMove = null;


        public Form1()
        {
            InitializeComponent();
            InitializeBoard();
            this.panel1.Paint += new PaintEventHandler(Panel1_Paint);
            this.panel1.MouseClick += new MouseEventHandler(Panel1_MouseClick);
            UpdateStatus();
            // 設置窗體的邊框樣式為固定
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
        }

        // 初始化棋盘
        private void InitializeBoard()
        {
            board[3, 3] = 2;
            board[4, 4] = 2;
            board[3, 4] = 1;
            board[4, 3] = 1;
        }

        // 绘制棋盘和棋子
        private void Panel1_Paint(object sender, PaintEventArgs e)
        {
            DrawBoard(e.Graphics);

            // 绘制有效移动指示器
            List<Point> validMoves = GetValidMoves(isBlackTurn ? 1 : 2);
            foreach (Point move in validMoves)
            {
                int centerX = move.X * CellSize + CellSize / 2;
                int centerY = move.Y * CellSize + CellSize / 2;
                int radius = CellSize / 6;

                e.Graphics.FillEllipse(Brushes.Green, centerX - radius, centerY - radius, radius * 2, radius * 2);
            }
            // 绘制最新一步的红点
            if (lastMove.HasValue)
            {
                int lastMoveX = lastMove.Value.X * CellSize + CellSize / 2;
                int lastMoveY = lastMove.Value.Y * CellSize + CellSize / 2;
                int redDotRadius = CellSize / 8;

                e.Graphics.FillEllipse(Brushes.Red, lastMoveX - redDotRadius, lastMoveY - redDotRadius, redDotRadius * 2, redDotRadius * 2);
            }
        }

        // 绘制棋盘和棋子
        private void DrawBoard(Graphics g)
        {
            Pen pen = new Pen(Color.Black);
            Brush backgroundBrush = new SolidBrush(Color.LightGreen);

            // Draw grid background
            for (int y = 0; y < GridSize; y++)
            {
                for (int x = 0; x < GridSize; x++)
                {
                    g.FillRectangle(backgroundBrush, x * CellSize, y * CellSize, CellSize, CellSize);
                }
            }

            // 绘制网格线
            for (int i = 0; i <= GridSize; i++)
            {
                g.DrawLine(pen, i * CellSize, 0, i * CellSize, GridSize * CellSize);
                g.DrawLine(pen, 0, i * CellSize, GridSize * CellSize, i * CellSize);
            }

            // 绘制棋子
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

        // 处理鼠标点击事件
        private void Panel1_MouseClick(object sender, MouseEventArgs e)
        {
            int x = e.X / CellSize;
            int y = e.Y / CellSize;

            if (x >= 0 && x < GridSize && y >= 0 && y < GridSize && board[y, x] == 0)
            {
                int currentPlayer = isBlackTurn ? 1 : 2;
                if (IsValidMove(x, y, currentPlayer))
                {
                    MakeMove(x, y, currentPlayer);
                    this.panel1.Invalidate();
                    UpdateStatus();

                    if (!HasValidMove(1) && !HasValidMove(2))
                    {
                        DialogResult result = MessageBox.Show(GetScores() + "游戲結束！\n 是否重新開始？", "確認", MessageBoxButtons.YesNo);
                        if (result == DialogResult.Yes)
                        {
                            ResetBoard();
                        }
                    }
                    if (!HasValidMove(2))
                    {
                        isBlackTurn = false;
                        UpdateStatus();
                        MessageBox.Show("没有有效的移动，回合跳过！");
                    }
                    else if (isSinglePlayer && !isBlackTurn)
                    {
                        // 电脑回合
                        ComputerMove();
                    }
                }
            }
        }

        // 电脑回合逻辑
        private void ComputerMove()
        {
            List<Point> validMoves = GetValidMoves(2);
            if (validMoves.Count > 0)
            {
                Random rand = new Random();
                Point move = validMoves[rand.Next(validMoves.Count)];
                MakeMove(move.X, move.Y, 2);
                this.panel1.Invalidate();
                UpdateStatus();

                if (!HasValidMove(1) && !HasValidMove(2))
                {
                    DialogResult result = MessageBox.Show(GetScores() + "游戲結束 ！是否重新開始？", "", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        ResetBoard();
                    }
                }
            }
            else if (!HasValidMove(1))
            {
                MessageBox.Show("没有有效的移动，游戏结束！");
            }
            else
            {
                isBlackTurn = true;
                UpdateStatus();
                MessageBox.Show("没有有效的移动，回合跳过！");
            }
        }

        // 执行落子操作
        private void MakeMove(int x, int y, int player)
        {
            board[y, x] = player;
            FlipPieces(x, y, player);
            isBlackTurn = !isBlackTurn;
            lastMove = new Point(x, y); // 保存最新一步的位置
        }

        // 检查移动是否合法
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

        // 翻转对手的棋子
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

        // 检查是否有合法移动
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

        // 获取分数
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

            return $"黑棋: {blackCount}, 白棋: {whiteCount}";
        }

        // 更新状态显示
        private void UpdateStatus()
        {
            string currentPlayer = isBlackTurn ? "黑棋" : "白棋";
            label1.Text = $"回合: {currentPlayer}\n{GetScores()}";
        }

        // 获取有效移动位置列表
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

        public void setModeToPVP()//玩家對戰模式
        {
            isSinglePlayer = false;
        }
        public void setModeToPVE()//人機對戰模式
        {
            isSinglePlayer = true;
        }

        private void Back_Click(object sender, EventArgs e)//返回按鈕
        {
            this.Close();
        }

        private void ResetBoard()//重設棋盤
        {
            board = new int[GridSize, GridSize];
            InitializeBoard();
            isBlackTurn = true;
            UpdateStatus();
            this.panel1.Invalidate(); // 重新绘制棋盘
        }
    }
}
