using System;
using System.Collections.Generic;
using System.Linq;

namespace libCore
{
    public enum DifficultyLevel
    {
        UpTo20,
        UpTo40,
        UpTo60,
        UpTo100
    }

    public enum ArithmeticOperation
    {
        Add,
        Subtract,
        Multiply,
        Divide
    }

    public sealed class GameSettings
    {
        public DifficultyLevel Difficulty { get; set; } = DifficultyLevel.UpTo20;

        public bool AllowAdd { get; set; } = true;
        public bool AllowSubtract { get; set; } = true;
        public bool AllowMultiply { get; set; } = false;
        public bool AllowDivide { get; set; } = false;

        public int TimeLimitSeconds { get; set; } = 60;
        public int MaxQuestions { get; set; } = 0;

        public int MaxOperand => Difficulty switch
        {
            DifficultyLevel.UpTo20 => 20,
            DifficultyLevel.UpTo40 => 40,
            DifficultyLevel.UpTo60 => 60,
            DifficultyLevel.UpTo100 => 100,
            _ => 20
        };

        public IEnumerable<ArithmeticOperation> GetAllowedOperations()
        {
            if (AllowAdd) yield return ArithmeticOperation.Add;
            if (AllowSubtract) yield return ArithmeticOperation.Subtract;
            if (AllowMultiply) yield return ArithmeticOperation.Multiply;
            if (AllowDivide) yield return ArithmeticOperation.Divide;
        }
    }

    public sealed class Game
    {
        private readonly Random rnd = new();
        private readonly GameSettings settings;
        private bool answerCorrect;
        private int correctStreak;
        private int incorrectStreak;
        private List<ArithmeticOperation> allowedOperations = new();

        public int QuestionNumber { get; private set; }
        public int CountCorrect { get; private set; }
        public int CountIncorrect { get; private set; }
        public int Coins { get; private set; }
        public int RemainingSeconds { get; private set; }
        public bool IsFinished { get; private set; }

        public string QuestionLine { get; private set; } = string.Empty;
        public string StatusLine { get; private set; } = string.Empty;

        public event Action? ChangeQuestion;
        public event Action? ChangeStat;
        public event Action? ChangeTimer;
        public event Action? GameOver;

        public Game(GameSettings? gameSettings = null)
        {
            settings = gameSettings ?? new GameSettings();
            ReloadAllowedOperations();
        }

        public void Restart()
        {
            ReloadAllowedOperations();

            CountCorrect = 0;
            CountIncorrect = 0;
            Coins = 0;
            correctStreak = 0;
            incorrectStreak = 0;
            QuestionNumber = 0;
            RemainingSeconds = settings.TimeLimitSeconds;
            IsFinished = false;

            StatusLine = "Игра началась";
            ChangeStat?.Invoke();
            ChangeTimer?.Invoke();

            GenerateNextQuestion();
        }

        public void Tick()
        {
            if (IsFinished)
                return;

            if (settings.TimeLimitSeconds <= 0)
                return;

            if (RemainingSeconds > 0)
            {
                RemainingSeconds--;
                ChangeTimer?.Invoke();

                if (RemainingSeconds <= 0)
                    EndGame("Время вышло");
            }
        }

        public void Answer(bool userSaysTrue)
        {
            if (IsFinished)
                return;

            bool userCorrect = userSaysTrue == answerCorrect;

            if (userCorrect)
            {
                CountCorrect++;
                correctStreak++;
                incorrectStreak = 0;

                Coins += 1 << (correctStreak - 1);
                StatusLine = $"Верно! Серия: {correctStreak}";
            }
            else
            {
                CountIncorrect++;
                incorrectStreak++;
                correctStreak = 0;

                Coins -= 1 << (incorrectStreak - 1);
                if (Coins < 0)
                    Coins = 0;

                StatusLine = $"Неверно! Ошибка подряд: {incorrectStreak}";
            }

            ChangeStat?.Invoke();

            if (settings.MaxQuestions > 0 && QuestionNumber >= settings.MaxQuestions)
            {
                EndGame("Достигнуто максимальное число вопросов");
                return;
            }

            GenerateNextQuestion();
        }

        private void ReloadAllowedOperations()
        {
            allowedOperations = settings.GetAllowedOperations().ToList();

            if (allowedOperations.Count == 0)
            {
                allowedOperations.Add(ArithmeticOperation.Add);
                settings.AllowAdd = true;
            }

            if (settings.Difficulty == DifficultyLevel.UpTo20)
            {
                settings.AllowMultiply = false;
                settings.AllowDivide = false;
                allowedOperations = settings.GetAllowedOperations().ToList();
            }
            else if (settings.Difficulty == DifficultyLevel.UpTo40)
            {
                settings.AllowDivide = false;
                allowedOperations = settings.GetAllowedOperations().ToList();
            }
            else if (settings.Difficulty == DifficultyLevel.UpTo60)
            {
                settings.AllowDivide = false;
                if (!settings.AllowMultiply)
                    settings.AllowMultiply = true;
                allowedOperations = settings.GetAllowedOperations().ToList();
            }
            else if (settings.Difficulty == DifficultyLevel.UpTo100)
            {
                if (!settings.AllowMultiply)
                    settings.AllowMultiply = true;
                if (!settings.AllowDivide)
                    settings.AllowDivide = true;

                allowedOperations = settings.GetAllowedOperations().ToList();
            }

            if (allowedOperations.Count == 0)
                allowedOperations.Add(ArithmeticOperation.Add);
        }

        private void GenerateNextQuestion()
        {
            QuestionNumber++;

            var operation = allowedOperations[rnd.Next(allowedOperations.Count)];
            int a;
            int b;
            int correctResult;
            string sign;

            switch (operation)
            {
                case ArithmeticOperation.Add:
                    a = rnd.Next(0, settings.MaxOperand + 1);
                    b = rnd.Next(0, settings.MaxOperand + 1);
                    correctResult = a + b;
                    sign = "+";
                    break;

                case ArithmeticOperation.Subtract:
                    a = rnd.Next(0, settings.MaxOperand + 1);
                    b = rnd.Next(0, settings.MaxOperand + 1);
                    if (a < b)
                    {
                        int temp = a;
                        a = b;
                        b = temp;
                    }
                    correctResult = a - b;
                    sign = "-";
                    break;

                case ArithmeticOperation.Multiply:
                    a = rnd.Next(0, Math.Max(2, settings.MaxOperand / 2) + 1);
                    b = rnd.Next(0, Math.Max(2, settings.MaxOperand / 2) + 1);
                    correctResult = a * b;
                    sign = "×";
                    break;

                case ArithmeticOperation.Divide:
                    b = rnd.Next(1, Math.Max(2, settings.MaxOperand / 2) + 1);
                    correctResult = rnd.Next(0, Math.Max(2, settings.MaxOperand / 2) + 1);
                    a = correctResult * b;
                    sign = "÷";
                    break;

                default:
                    a = rnd.Next(0, settings.MaxOperand + 1);
                    b = rnd.Next(0, settings.MaxOperand + 1);
                    correctResult = a + b;
                    sign = "+";
                    break;
            }

            bool showTrue = rnd.Next(2) == 0;
            int shownResult = showTrue ? correctResult : CreateWrongAnswer(correctResult);

            QuestionLine = $"Вопрос {QuestionNumber}: {a} {sign} {b} = {shownResult}";
            answerCorrect = shownResult == correctResult;

            ChangeQuestion?.Invoke();
        }

        private int CreateWrongAnswer(int correctResult)
        {
            int delta;
            int maxDelta = Math.Max(3, settings.MaxOperand / 2 + 3);

            do
            {
                delta = rnd.Next(1, maxDelta + 1);
                if (rnd.Next(2) == 0)
                    delta = -delta;
            }
            while (correctResult + delta < 0 || delta == 0);

            return correctResult + delta;
        }

        private void EndGame(string reason)
        {
            if (IsFinished)
                return;

            IsFinished = true;
            StatusLine = reason;
            ChangeStat?.Invoke();
            GameOver?.Invoke();
        }
    }
}