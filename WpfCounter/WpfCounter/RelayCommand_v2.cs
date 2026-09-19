using System.Windows.Input;

namespace WpfCounter
{
    internal class RelayCommand_v2 : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Predicate<object?>? _canExecute;
        private EventHandler? _canExecuteChanged;

        public RelayCommand_v2(Action<object?> execute, Predicate<object?>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged
        {
            add
            {
                _canExecuteChanged += value;
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                _canExecuteChanged -= value;
                CommandManager.RequerySuggested -= value;
            }
        }

        public bool CanExecute(object? parameter) =>
            _canExecute is null || _canExecute(parameter);

        public void Execute(object? parameter) => _execute(parameter);

        public void RaiseExecuteChanged() =>
            _canExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}