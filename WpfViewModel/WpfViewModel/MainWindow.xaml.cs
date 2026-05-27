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

namespace WpfViewModel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //DataContext = new MainViewModel_v1() { FirstName = "Гомер" , LastName = "Родионович", Age = "20"};
            //DataContext = new MainViewModel_v2() { FirstName = "Гомер" , LastName = "Родионович", Age = 20};
            //DataContext = new MainViewModel_v3() { FirstName = "Гомер" , LastName = "Родионович", Age = 20};
            DataContext = new MainViewModel_v4() { FirstName = "Гомер" , LastName = "Родионович", Age = 20};
        }
    }
}