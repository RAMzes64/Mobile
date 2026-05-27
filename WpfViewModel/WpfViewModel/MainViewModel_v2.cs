using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfViewModel
{
    internal class MainViewModel_v2: INotifyPropertyChanged
    {
        public MainViewModel_v2()
        {
        }

        private string? firstName;

        public string FirstName
        {
            get => firstName;
            set
            {
                if (firstName == value) return;
                firstName = value;
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FirstName"));
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FirstName)));
                OnPropertyChanged();
                OnPropertyChanged(nameof(FullName));
            }
        }


        public string LastName
        {
            get => field;
            set
            {
                if (field == value) return;
                field = value;
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LastName)));
                OnPropertyChanged();
                OnPropertyChanged(nameof(FullName));
            }
        }
        public int? Age
        {
            get;
            set
            {
                if (field == value) return;
                field = value;
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LastName)));
                OnPropertyChanged();
            }
        }
        public string FullName => $"{FirstName} {LastName}";
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public event PropertyChangedEventHandler? PropertyChanged;

    }
}