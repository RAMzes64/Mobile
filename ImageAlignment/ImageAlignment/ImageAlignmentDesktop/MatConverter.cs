using System.Runtime.InteropServices;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using OpenCvSharp;

namespace ImageAlignmentDesktop
{
    [ValueConversion(typeof(Mat), typeof(BitmapSource))]
    public class MatConverter : IValueConverter
    {
        public MatConverter() { }
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            // Обработка null (например, когда свойство ViewModel ещё не установлено)
            if (value is null)
                return null;

            // Проверка типа
            if (value is not Mat mat)
                throw new ArgumentException($"Ожидался тип Mat, получен {value.GetType().Name}.");

            if (mat.Empty())
                return null;

            // Определяем формат – метод выбрасывает исключение, если формат не поддерживается
            PixelFormat pixelFormat = GetPixelFormat(mat);

            int width = mat.Width;
            int height = mat.Rows;
            int step = (int)mat.Step();   // шаг строки в байтах
            int bufferSize = height * step;

            // Копируем данные в новый массив (иммутабельность)
            byte[] pixelData = new byte[bufferSize];
            Marshal.Copy(mat.Data, pixelData, 0, bufferSize);

            return BitmapSource.Create(
                width, height,
                96, 96,
                pixelFormat,
                null,          // палитра не нужна
                pixelData,
                step
            );
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            => throw new NotSupportedException("Обратное преобразование (BitmapSource → Mat) не поддерживается.");

        /// <summary>
        /// Возвращает WPF PixelFormat для данного Mat.
        /// Генерирует исключение, если формат не поддерживается.
        private static PixelFormat GetPixelFormat(Mat mat)
        {
            if (mat.Depth() != MatType.CV_8U)
                throw new NotSupportedException(
                    $"Поддерживается только 8-битная глубина (CV_8U). Текущая глубина: {mat.Depth()}");

            return mat.Channels() switch
            {
                1 => PixelFormats.Gray8,
                3 => PixelFormats.Bgr24,
                4 => PixelFormats.Bgra32,
                _ => throw new NotSupportedException(
                    $"Неподдерживаемое количество каналов: {mat.Channels()}. Допустимо 1, 3 или 4.")
            };
        }
    }
}