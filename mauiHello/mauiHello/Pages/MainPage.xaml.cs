using mauiHello.Models;
using mauiHello.PageModels;

namespace mauiHello.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageModel model)
        {
            InitializeComponent();
            BindingContext = model;
        }
    }
}