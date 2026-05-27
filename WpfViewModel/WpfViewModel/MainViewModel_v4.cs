using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfViewModel
{
    internal class MainViewModel_v4: ViewModelBase
    {
        public MainViewModel_v4()
        {
        }

        public string FirstName
        {
            get => field;
            set
            {
                if (!SetProperty(ref field, value)) return;
                OnPropertyChanged(nameof(FullName));
            }
        }


        public string LastName
        {
            get => field;
            set
            {
                if (!SetProperty(ref field, value)) return;
                OnPropertyChanged(nameof(FullName));
            }
        }


        public int? Age
        {
            get;
            set => SetProperty(ref field, value);
        }
        public string FullName => $"{FirstName} {LastName}";
    }
}