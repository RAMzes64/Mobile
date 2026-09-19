using System.Windows.Input;

namespace WpfCounter
{
    internal class CounterIncrementCommand (MainViewModel mainViewModel) : ICommand
    {
        public event EventHandler? CanExecuteChanged;
        public bool CanExecute(object? parameter) => true;
        public void Execute(object? parameter) => mainViewModel.Counter++;
    }
}