using System.ComponentModel;
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

namespace wpfBinding
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var heroEx = (HeroEx) this.Resources["myHeroEx"];
            heroEx.Name = "Not ABu";
            heroEx.Clan = "dwadadww";
            heroEx.Description = "dadadad";
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var heroEx = (HeroEx)this.Resources["myHeroEx"];
            buGetName.Content = heroEx.Name;
          
        }
    }

    public class Hero
    {
        public string? Name { get; set; }
        public string? Clan { get; set; }
        public string? Description { get; set; }
        public int? HP { get; set; } = 100;
    }
    public class HeroEx : INotifyPropertyChanged
    {
        private string? name;
        private string? clan;

        public string? Name
        {
            get => name;
            set
            {
                name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
            }
        }
        public string? Clan
        {
            get => clan;
            set
            {
                clan = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Clan)));
            }
        }
        public string? Description { get; set; }
        public int? HP { get; set; } = 100;

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}