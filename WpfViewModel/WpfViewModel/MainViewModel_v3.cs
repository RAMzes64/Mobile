using System.ComponentModel;
using System.Configuration;
using System.Runtime.CompilerServices;

namespace WpfViewModel
{
    internal class MainViewModel_v3 : INotifyPropertyChanged
    {
        public MainViewModel_v3()
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
        
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private bool SetProperty<T>(
            ref T field,
            T value,
            [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

    }
}