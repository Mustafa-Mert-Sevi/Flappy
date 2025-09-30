using flappy;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace flappy
{
    public partial class Form1 : Form
    {
        private Bird bird = null!;
        private List<Pipe> pipes = null!;
        private System.Windows.Forms.Timer gameTimer = null!;
        private int score;
        private int pipeSpawnCounter;
        private int pipeSpawnInterval = 70;
        private bool gameOver;
        private Random random = null!;

        public Form1()
        {
            InitializeComponent();
            SetupForm();
            InitializeGame();
        }

        private void SetupForm()
        {
            this.ClientSize = new Size(800, 600);
            this.Text = "Flappy Bird";
            this.DoubleBuffered = true;
            this.KeyDown += Form1_KeyDown;
            this.Paint += Form1_Paint;
        }

        private void InitializeGame()
        {
            bird = new Bird(100, 250, 40, 40);
            pipes = new List<Pipe>();
            score = 0;
            pipeSpawnCounter = 60; // Start spawning pipes sooner
            gameOver = false;
            random = new Random();

            if (gameTimer != null)
            {
                gameTimer.Stop();
                gameTimer.Dispose();
            }

            gameTimer = new System.Windows.Forms.Timer();
            gameTimer.Interval = 20;
            gameTimer.Tick += GameTimer_Tick;
            gameTimer.Start();
        }

        private void GameTimer_Tick(object? sender, EventArgs e)
        {
            if (gameOver) return;

            // Update bird
            bird.Update();

            // Check if bird hits ground or ceiling
            if (bird.Y + bird.Height > this.ClientSize.Height || bird.Y < 0)
            {
                EndGame();
                return;
            }

            // Update pipes
            for (int i = pipes.Count - 1; i >= 0; i--)
            {
                pipes[i].Update();

                // Check collision
                if (bird.GetBounds().IntersectsWith(pipes[i].GetTopBounds()) ||
                    bird.GetBounds().IntersectsWith(pipes[i].GetBottomBounds()))
                {
                    EndGame();
                    return;
                }

                // Check score
                if (!pipes[i].Scored && bird.X > pipes[i].X + pipes[i].Width)
                {
                    pipes[i].Scored = true;
                    score++;
                }

                // Remove off-screen pipes
                if (pipes[i].IsOffScreen())
                {
                    pipes.RemoveAt(i);
                }
            }

            // Spawn new pipes
            pipeSpawnCounter++;
            if (pipeSpawnCounter >= pipeSpawnInterval)
            {
                SpawnPipe();
                pipeSpawnCounter = 0;
            }

            this.Invalidate();
        }

        private void SpawnPipe()
        {
            int gap = 150;
            int pipeWidth = 60;
            int minHeight = 100;
            int maxHeight = this.ClientSize.Height - gap - 100;
            int topHeight = random.Next(minHeight, maxHeight);

            Pipe newPipe = new Pipe(this.ClientSize.Width, topHeight, gap, pipeWidth, 5, this.ClientSize.Height);
            pipes.Add(newPipe);
        }

        private void Form1_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                if (gameOver)
                {
                    RestartGame();
                }
                else
                {
                    bird.Jump();
                }
            }
        }

        private void Form1_Paint(object? sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            // Draw background
            g.Clear(Color.SkyBlue);

            // Draw ground
            g.FillRectangle(Brushes.SaddleBrown, 0, this.ClientSize.Height - 50, this.ClientSize.Width, 50);

            // Draw pipes
            foreach (Pipe pipe in pipes)
            {
                pipe.Draw(g);
            }

            // Draw bird
            bird.Draw(g);

            // Draw score (sadece oyun devam ederken göster)
            if (!gameOver)
            {
                g.DrawString($"Score: {score}", new Font("Arial", 20, FontStyle.Bold), Brushes.White, 10, 10);
            }

            // Draw game over message
            if (gameOver)
            {
                // 1. Önce overlay'ý çiz
                using (SolidBrush overlay = new SolidBrush(Color.FromArgb(200, 0, 0, 0)))
                {
                    g.FillRectangle(overlay, 0, 0, this.ClientSize.Width, this.ClientSize.Height);
                }

                // 2. Sonra metinleri overlay'ýn üstüne çiz
                string gameOverText = "GAME OVER";
                Font gameOverFont = new Font("Arial", 36, FontStyle.Bold);
                SizeF gameOverSize = g.MeasureString(gameOverText, gameOverFont);
                float gameOverX = (this.ClientSize.Width - gameOverSize.Width) / 2;
                float gameOverY = (this.ClientSize.Height - gameOverSize.Height) / 2 - 40;

                g.DrawString(gameOverText, gameOverFont, Brushes.Red, gameOverX, gameOverY);

                string scoreText = $"Score: {score}";
                Font scoreFont = new Font("Arial", 24, FontStyle.Bold);
                SizeF scoreSize = g.MeasureString(scoreText, scoreFont);
                float scoreX = (this.ClientSize.Width - scoreSize.Width) / 2;
                float scoreY = gameOverY + gameOverSize.Height + 10;

                g.DrawString(scoreText, scoreFont, Brushes.White, scoreX, scoreY);

                string restartText = "Press SPACE to restart";
                Font restartFont = new Font("Arial", 16, FontStyle.Regular);
                SizeF restartSize = g.MeasureString(restartText, restartFont);
                float restartX = (this.ClientSize.Width - restartSize.Width) / 2;
                float restartY = scoreY + scoreSize.Height + 15;

                g.DrawString(restartText, restartFont, Brushes.Yellow, restartX, restartY);
            }
        }

        private void EndGame()
        {
            gameOver = true;
        }

        private void RestartGame()
        {
            pipes.Clear();
            InitializeGame();
        }
    }
}