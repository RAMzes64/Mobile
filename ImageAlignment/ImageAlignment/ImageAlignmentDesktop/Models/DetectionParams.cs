using System.ComponentModel;
using System.Runtime.CompilerServices;
using OpenCvSharp;

namespace ImageAlignmentDesktop.Models
{    
    /// Параметры поиска описывающего прямоугольника.
    /// Все свойства оповещают об изменениях через INotifyPropertyChanged.
    
    public class DetectionParams : INotifyPropertyChanged
    {
        private Scalar _color;
        private int _thickness;
        private int _kernelSize;
        private double _minArea;

        
        /// Цвет контура (BGR-формат OpenCV).
        public Scalar Color
        {
            get => _color;
            set { _color = value; OnPropertyChanged(); }
        }

        
        /// Толщина линий контура (по умолчанию 10). 
        public int Thickness
        {
            get => _thickness;
            set { _thickness = value; OnPropertyChanged(); }
        }

        
        /// Размер ядра для морфологических операций (по умолчанию 3).
        public int KernelSize
        {
            get => _kernelSize;
            set { _kernelSize = value; OnPropertyChanged(); }
        }

        
        /// Минимальная площадь контура для фильтрации (по умолчанию 100).
        public double MinArea
        {
            get => _minArea;
            set { _minArea = value; OnPropertyChanged(); }
        }

        public DetectionParams()
        {
            // Значения по умолчанию, как в методе GetRect API
            Color = new Scalar(0, 0, 255); // Красный
            Thickness = 10;
            KernelSize = 3;
            MinArea = 100.0;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}