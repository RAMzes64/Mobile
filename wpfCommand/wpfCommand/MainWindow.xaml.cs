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

namespace wpfCommand
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CommandNew_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("CommandNew_Executed");
        }

        private void CommandSave_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("CommandSave_Executed");
        }

        private void CommandSave_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CommandBinding_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            EdNote.Text += "fewk ";
        }
    }
    public class MyCommand 
    {
        public static RoutedCommand cmdAdd { get; private set; } = new RoutedCommand("cmdAdd", typeof(MainWindow));
    }
}