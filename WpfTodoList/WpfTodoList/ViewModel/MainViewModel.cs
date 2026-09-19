using System.Collections.ObjectModel;
using System.Configuration;
using System.Windows.Input;

namespace WpfTodoList
{
    internal class MainViewModel:ViewModelBase
    {
        //public List<string> Todos { get;  }

        public ObservableCollection<TodoItem> Todos { get; } = new();
        
        public ICommand AddTodoCommand { get; }
        public ICommand RemoveTodoCommand { get; }
        public ICommand RemoveDoneTodoCommand {  get; }
        
        public string NewTitle { get; set => SetProperty(ref field, value); }
        public TodoItem? SelectedTodo { get; set => SetProperty(ref field, value); }

        public MainViewModel()
        {
            Todos.Add(new TodoItem ("1slfjsdkjflksdj"));
            Todos.Add(new TodoItem ("2",true) );
            Todos.Add(new TodoItem ("3"));
        
            AddTodoCommand = new RelayCommand(_ =>  AddTodo(), _ => CanAddTodo());
            RemoveTodoCommand = new RelayCommand(_ => RemoveTodo(), _ => SelectedTodo != null);
            RemoveDoneTodoCommand = new RelayCommand(_ => RemoveDoneTodo());
        }

        private void RemoveTodo()
        {
            if (SelectedTodo != null)
                Todos.Remove(SelectedTodo);
        }

        public bool CanAddTodo() => !string.IsNullOrWhiteSpace(NewTitle);

        private void AddTodo()
        {
            Todos.Add(new TodoItem(NewTitle, false));
            NewTitle = "";
        }

        private void RemoveDoneTodo()
        {
            foreach (TodoItem item in Todos)
                if (item.IsDone == true) Todos.Remove(item);
        }
    }
}