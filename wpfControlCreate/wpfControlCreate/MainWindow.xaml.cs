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

namespace wpfControlCreate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.MouseDown += MainWindow_MouseDown;
        }

        private void MainWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //if (e.LeftButton == MouseButtonState.Pressed) 
            if (e.ChangedButton == MouseButton.Left)
            {
                var la = new Label();
                la.Background = Brushes.LightCoral;
                la.SetValue(Canvas.LeftProperty, e.GetPosition(this).X);
                la.SetValue(Canvas.TopProperty, e.GetPosition(this).Y);
                la.Content = $"{la.GetValue(Canvas.LeftProperty)}:{la.GetValue(Canvas.TopProperty)}";
                main.Children.Add(la);
            }
            if (e.ChangedButton == MouseButton.Right)
            {
                var rnd = new Random();
                for (int i = 0; i < 10; i++)
                {
                    var la = new Label();
                    double _left = rnd.Next((int)this.Width);
                    double _top = rnd.Next((int)this.Height);
                    la.SetValue(Canvas.LeftProperty, _left);
                    la.SetValue(Canvas.TopProperty, _top);
                    la.Content = $"{_left}:{_top}";
                    la.Background = new SolidColorBrush(Color.FromRgb(
                        (byte)rnd.Next(256), (byte)rnd.Next(256), (byte)rnd.Next(256)));
                    main.Children.Add(la);

                }
            }
        }
    }
}