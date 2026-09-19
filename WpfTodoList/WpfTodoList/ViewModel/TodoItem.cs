namespace WpfTodoList
{
    public class TodoItem(string? Title, bool IsDone = false) : ViewModelBase
    {
        public string? Title { get; set => SetProperty(ref field, value); }
        public bool IsDone { get; set => SetProperty(ref field, value); } = false;

    }
}