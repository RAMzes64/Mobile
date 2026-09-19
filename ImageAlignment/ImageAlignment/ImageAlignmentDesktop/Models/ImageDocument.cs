using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace ImageAlignmentDesktop.Models
{
    /// Представляет документ с изображением и результатами обработки.
    /// Содержит все промежуточные состояния Mat и параметры
    public class ImageDocument : INotifyPropertyChanged
    {
        private string _filePath;
        private Mat _sourceImage;
        private Mat _rotatedImage;
        private Mat _croppedImage;
        private Rect _detectedRect;
        private double _angle;
        private DetectionParams _detectionParams;
        private Mat _displayMat; // для привязки к UI

        
        /// Путь к файлу изображения.
        public string FilePath
        {
            get => _filePath;
            set { _filePath = value; OnPropertyChanged(); }
        }

        
        /// Исходное изображение (после ReadImg).
        public Mat SourceImage
        {
            get => _sourceImage;
            set { _sourceImage = value; OnPropertyChanged(); }
        }

        
        /// Изображение после поворота.
        public Mat RotatedImage
        {
            get => _rotatedImage;
            set { _rotatedImage = value; OnPropertyChanged(); }
        }

        
        /// Изображение после обрезки (результат Crop).
        public Mat CroppedImage
        {
            get => _croppedImage;
            set { _croppedImage = value; OnPropertyChanged(); }
        }

        
        /// Обнаруженный описывающий прямоугольник.
        public Rect DetectedRect
        {
            get => _detectedRect;
            set { _detectedRect = value; OnPropertyChanged(); }
        }

        
        /// Угол поворота в градусах (связан с ползунком -180..180).
        public double Angle
        {
            get => _angle;
            set { _angle = value; OnPropertyChanged(); }
        }

        
        /// Параметры поиска контуров.
        public DetectionParams DetectionParams
        {
            get => _detectionParams;
            set { _detectionParams = value; OnPropertyChanged(); }
        }

        
        /// Текущее изображение для отображения в UI.
        /// Может быть SourceImage, RotatedImage или CroppedImage.
        /// Конвертер MatToBitmapSourceConverter автоматически преобразует его в BitmapSource.
        public Mat DisplayMat
        {
            get => _displayMat;
            set { _displayMat = value; OnPropertyChanged(); }
        }

        public ImageDocument()
        {
            DetectionParams = new DetectionParams();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
