using System.Windows.Input;

namespace WpfTodoList
{
    internal class RelayCommand(Action<object?> execute, Predicate<object?> canExecute = null) : ICommand
    {
        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object? parameter) => canExecute == null || canExecute.Invoke(parameter);

        public void Execute(object? parameter) => execute(parameter);
    }
}