using System.Windows.Input;

namespace WpfCounter
{
    internal class RelayCommand_v1 : ICommand
    {
        private Action<object?> _execute;
        private Predicate<object?> _canExecute;

        public RelayCommand_v1(Action<object?> execute, Predicate<object?> canExecute = null)
        {
            this._execute = execute;
            this._canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object? parameter) => _canExecute == null || _canExecute.Invoke(parameter);

        public void Execute(object? parameter) => _execute(parameter);
    }
}