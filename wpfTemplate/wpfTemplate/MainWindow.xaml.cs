using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace wpfTemplate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<MyTask> listTasks;
        MyTask newTask = new();

        public MainWindow()
        {
            InitializeComponent();

            List<string> ListPhones = new() {"iphone13", "NIF", "Samasung" };
            lbPhones.ItemsSource = ListPhones;

            listTasks = new()
            {
                new() {Name="Open laptop",Description="My laptop", Priority=1},
                new() {Name="Do homework", Priority=1},
                new() {Name="Close laptop",Description="Off", Priority=5}
            };
            lbTasks.ItemsSource = listTasks;
            stackPanelAdd.DataContext = newTask;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            listTasks.Add(new() {Name = newTask.Name, Description= newTask.Description, Priority=newTask.Priority});
            MessageBox.Show($"! {newTask.Name}");
        }
    }

    internal class MyTask
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int Priority { get; set; }
    }
}