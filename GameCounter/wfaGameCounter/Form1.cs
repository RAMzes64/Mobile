using libCore;
using System;
using System.Windows.Forms;

namespace wfaGameCounter
{
    public partial class Form1 : Form
    {
        private readonly Game game;
        private readonly System.Windows.Forms.Timer timer = new();

        public Form1()
        {
            InitializeComponent();

            game = new Game(new GameSettings
            {
                Difficulty = DifficultyLevel.UpTo60,
                TimeLimitSeconds = 60,
                AllowAdd = true,
                AllowSubtract = true,
                AllowMultiply = true,
                AllowDivide = false
            });

            game.ChangeQuestion += UpdateView;
            game.ChangeStat += UpdateView;
            game.ChangeTimer += UpdateView;
            game.GameOver += OnGameOver;

            timer.Interval = 1000;
            timer.Tick += (_, _) => game.Tick();
        }

        private void Game_ChangeStat()
        {
            laCountCorrect.Text = $"Верно  = {game.CountCorrect}";
            laCountIncorrect.Text = $"Неверно  = {game.CountIncorrect}";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            game.Restart();
            timer.Start();
            UpdateView();
        }
        private void buYes_Click(object sender, EventArgs e)
        {
            game.Answer(true);
       
        }

        private void buNo_Click(object sender, EventArgs e)
        {
            game.Answer(false);
          
        }

        private void BtnRestart_Click(object sender, EventArgs e)
        {
            game.Restart();
            timer.Start();
            buYes.Enabled = true;
            buNo.Enabled = true;
            UpdateView();
        }

        private void UpdateView()
        {
            laQuestion.Text = game.QuestionLine;
            laCountCorrect.Text = $"Верно: {game.CountCorrect}";
            laCountIncorrect.Text = $"Неверно: {game.CountIncorrect}";
            laCoins.Text = $"Монетки: {game.Coins}";
            laTimer.Text = game.RemainingSeconds > 0
                ? $"Время: {game.RemainingSeconds} сек."
                : "Время: выключено";
            laStatus.Text = game.StatusLine;
        }

        private void OnGameOver()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(OnGameOver));
                return;
            }

            timer.Stop();
            buYes.Enabled = false;
            buNo.Enabled = false;
            laStatus.Text = "Игра завершена";
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click_1(object sender, EventArgs e)
        {

        }
    }
}
