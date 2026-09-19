using System.Windows.Input;

namespace WpfCounter
{
    internal class MainViewModel : ViewModelBase
    {

        //public int Counter { get; set => SetProperty(ref field, value); }

        public int Counter
        {
            get;
            set
            {
                if (!SetProperty(ref field, value)) return;

                (DecrementCommand as RelayCommand_v2)?.RaiseExecuteChanged();
                (ResetCommand as RelayCommand_v2)?.RaiseExecuteChanged();
            }
        }
        public ICommand IncrementCommand { get; }
        public ICommand DecrementCommand { get; }
        public ICommand ResetCommand { get; }
        public ICommand AddCommand { get; }
        public MainViewModel()
        {
            //IncrementCommand = new CounterIncrementCommand(this);
            //IncrementCommand = new RelayCommand_v1(_ => Counter++);
            //DecrementCommand = new RelayCommand_v1(_ => Counter--, _ => Counter > 0);
            //ResetCommand = new RelayCommand_v1(_ => Counter = 0, _ => Counter != 0);

            IncrementCommand = new RelayCommand_v2(_ => Counter++);
            DecrementCommand = new RelayCommand_v2(_ => Counter--, _ => Counter > 0);
            ResetCommand = new RelayCommand_v2(_ => Counter = 0, _ => Counter != 0);

            AddCommand = new RelayCommand_v2(parameter =>
            {   
                if (parameter is string text && int.TryParse(text, out int number))
                Counter += number;
            });
        }
    }
}

/*
 * Рекомендуемый шаблон именования:
 * Для свойств: Глагол + Сущность + Command
 * Для классов: Сущность + Глагол + Command
 */