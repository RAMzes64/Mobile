using System;
using System.Timers;
using libCore;

namespace cnsGameCounter
{
    internal class Program
    {
        private static Game game = null!;
        private static System.Timers.Timer? timer;

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.CursorVisible = false;

            var settings = AskSettings();

            game = new Game(settings);
            game.ChangeQuestion += Draw;
            game.ChangeStat += Draw;
            game.ChangeTimer += Draw;
            game.GameOver += () =>
            {
                Draw();
                StopTimer();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine();
                Console.WriteLine("Игра завершена. Нажмите любую клавишу для выхода...");
                Console.ResetColor();
            };

            game.Restart();
            StartTimer();

            Draw();

            while (!game.IsFinished)
            {
                Console.SetCursorPosition(0, Console.CursorTop);
                Console.Write("Ответ [Y/N], R - рестарт, Q - выход: ");
                var line = Console.ReadLine()?.Trim().ToUpperInvariant();

                if (string.IsNullOrWhiteSpace(line))
                    continue;

                if (line == "Q")
                    break;

                if (line == "R")
                {
                    game.Restart();
                    continue;
                }

                if (line == "Y")
                    game.Answer(true);
                else if (line == "N")
                    game.Answer(false);
            }

            StopTimer();
        }

        private static GameSettings AskSettings()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Игра 'Устный счёт'");
            Console.ResetColor();

            Console.WriteLine("Выберите уровень сложности:");
            Console.WriteLine("1 - до 20");
            Console.WriteLine("2 - до 40");
            Console.WriteLine("3 - до 60");
            Console.WriteLine("4 - до 100");
            Console.Write("Ваш выбор: ");
            var diffLine = Console.ReadLine();

            DifficultyLevel difficulty = diffLine switch
            {
                "2" => DifficultyLevel.UpTo40,
                "3" => DifficultyLevel.UpTo60,
                "4" => DifficultyLevel.UpTo100,
                _ => DifficultyLevel.UpTo20
            };

            Console.Write("Время на игру в секундах (0 = без таймера): ");
            int.TryParse(Console.ReadLine(), out int timeLimit);
            if (timeLimit < 0)
                timeLimit = 0;

            var settings = new GameSettings
            {
                Difficulty = difficulty,
                TimeLimitSeconds = timeLimit,
                AllowAdd = true,
                AllowSubtract = true,
                AllowMultiply = difficulty >= DifficultyLevel.UpTo60,
                AllowDivide = difficulty >= DifficultyLevel.UpTo100
            };

            Console.Write("Ограничение по вопросам (0 = без ограничения): ");
            int.TryParse(Console.ReadLine(), out int maxQuestions);
            if (maxQuestions < 0)
                maxQuestions = 0;
            settings.MaxQuestions = maxQuestions;

            Console.WriteLine();
            Console.WriteLine("Нажмите Enter для старта...");
            Console.ReadLine();

            return settings;
        }

        private static void StartTimer()
        {
            if (game.RemainingSeconds <= 0)
                return;

            timer = new System.Timers.Timer(1000);
            timer.Elapsed += (_, _) => game.Tick();
            timer.AutoReset = true;
            timer.Start();
        }

        private static void StopTimer()
        {
            if (timer == null)
                return;

            timer.Stop();
            timer.Dispose();
            timer = null;
        }

        private static void Draw()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Игра 'Устный счёт'");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(game.QuestionLine);
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Верно: {game.CountCorrect}");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Неверно: {game.CountIncorrect}");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"Монетки: {game.Coins}");
            Console.ResetColor();

            if (game.RemainingSeconds > 0)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"Время: {game.RemainingSeconds} сек.");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine("Таймер: отключён");
            }

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(game.StatusLine);
            Console.ResetColor();

            Console.WriteLine();
            Console.WriteLine("Y - правда, N - ложь, R - рестарт, Q - выход");
        }
    }
}