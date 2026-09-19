using Microsoft.Win32;
using System.Text;
using System.Windows;
using ImageAlignmentDesktop;

namespace ImageAlignmentDesktop
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ImageAlignmentViewModel();    
        }
    }
}