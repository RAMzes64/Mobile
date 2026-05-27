namespace libCore
{
    public class Game
    {
        bool answerCorrect;
        String QuestionLine;
        int CounterCorrect;
        int CounterIncorrect;
        private Random rnd = new();

        public void GenNextQuestion() {
            int xValue1 = rnd.Next(20);
            int xValue2 = rnd.Next(20);
            int xResult = xValue1 + xValue2;
            int xResultNew = xResult;

            if (rnd.Next(2) == 1)
            {
                xResultNew += rnd.Next(1, 7) * (rnd.Next(2) == 1 ? 1 : -1);
            }

            QuestionLine = $"{xValue1} + {xValue2} = {xResultNew}";

            answerCorrect = xResult == xResultNew;
            ChangeQuestion?.Invoke();
        }

        public void answer(bool v)
        {
            if (v == answerCorrect)
                CounterCorrect++;
            else
                CounterIncorrect++;

            ChangeStat?.Invoke();
            GenNextQuestion()
        }
    }
}
file:///C:/Users/lemon/AppData/Local/Temp/tmpE3E8.tmp.gif