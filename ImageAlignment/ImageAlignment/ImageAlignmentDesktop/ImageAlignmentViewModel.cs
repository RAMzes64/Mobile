using ImageAlignmentDesktop.Models;
using OpenCvSharp;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using ImageAlignmentLib;


namespace ImageAlignmentDesktop
{
    public class ImageAlignmentViewModel : INotifyPropertyChanged
    {
        private ImageDocument _currentDocument;
        private string _errorMessage;
        private bool _isBusy;

        // Словарь доступных цветов для привязки к ComboBox (Формат OpenCV: BGR)
        // Набор данных во ViewModel (изменения не требуются, оставлен для сверки)
        public Dictionary<string, Scalar> AvailableColors { get; } = new Dictionary<string, Scalar>
        {
            { "Красный", Scalar.Red }, // Значение совпадает с дефолтным в DetectionParams.cs
            { "Черный", new Scalar(0, 0, 0) },
            { "Зеленый", new Scalar(0, 255, 0) },
            { "Синий", new Scalar(255, 0, 0) }
        };

        /// Текущий рабочий документ с изображениями и параметрами.
        public ImageDocument CurrentDocument
        {
            get => _currentDocument;
            set { _currentDocument = value; OnPropertyChanged(); }
        }

        
        /// Текст ошибки для вывода в интерфейс.
        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(); }
        }

        
        /// Флаг выполнения фоновой операции (для отображения индикатора загрузки).
        public bool IsBusy
        {
            get => _isBusy;
            set { _isBusy = value; OnPropertyChanged(); }
        }

        // Команды для привязки к кнопкам в XAML
        public ICommand LoadImageCommand { get; }
        public ICommand RotateCommand { get; }
        public ICommand DetectAndCropCommand { get; }
        public ICommand DetectCommand { get; }
        public ICommand SaveImageCommand { get; }

        public ImageAlignmentViewModel()
        {
            // Инициализируем документ по умолчанию
            CurrentDocument = new ImageDocument();

            // Привязываем действия к командам
            LoadImageCommand = new RelayCommand(_ => LoadImageAsync());
            RotateCommand = new RelayCommand(_ => RotateImageAsync());
            DetectAndCropCommand = new RelayCommand(async (_) => await CropAsync(), (_) => CanProcess());
            DetectCommand = new RelayCommand(async (_) => await DetectAsync(), (_) => CanProcess());
            SaveImageCommand = new RelayCommand(_ => SaveImageAsync());
        }

        
        /// Проверка: загружено ли исходное изображение для работы.
        
        private bool CanProcess()
        {
            return CurrentDocument?.SourceImage != null && !CurrentDocument.SourceImage.Empty();
        }


        // Метод асинхронной загрузки файла
        private async void LoadImageAsync()
        {
            // Инициализация стандартного диалога Windows
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Изображения|*.jpg;*.jpeg;*.png;*.bmp;*.gif|Все файлы|*.*",
                Title = "Выберите изображение"
            };

            // Обязательная проверка: если пользователь отменил диалог или закрыл окно крестиком
            if (openFileDialog.ShowDialog() != true)
            {
                return; // Мягкий выход из метода без изменений состояния
            }

            IsBusy = true;
            ErrorMessage = string.Empty;

            try
            {
                string selectedPath = openFileDialog.FileName;

                // Очищаем ресурсы старого изображения перед чтением нового
                ClearCurrentDocumentMemory();

                // Асинправное чтение тяжелого файла через API библиотеки (OpenCVSharp)
                var mat = await Task.Run(() => ImageAlignment.ReadImg(selectedPath));

                // Обновляем модель данных
                CurrentDocument.FilePath = selectedPath;
                CurrentDocument.SourceImage = mat;
                CurrentDocument.DisplayMat = mat; // Выводим на экран
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Не удалось открыть файл: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }


        /// Асинхронный поворот изображения на заданный угол.
        private async void RotateImageAsync()
        {
            // Гарантия защиты от Null и пустых данных на старте
            if (CurrentDocument?.SourceImage == null || CurrentDocument.SourceImage.Empty())
            {
                ErrorMessage = "Изображение не загружено или повреждено.";
                return;
            }

            IsBusy = true;
            ErrorMessage = string.Empty;

            var source = CurrentDocument.SourceImage;
            double angle = CurrentDocument.Angle;

            try
            {
                // 1. Асинхронный расчет поворота в OpenCV
                Mat rotatedResult = await Task.Run(() => ImageAlignment.Rotate(source, angle));

                // Проверяем, не вернула ли библиотека пустой результат
                if (rotatedResult == null || rotatedResult.Empty())
                    throw new InvalidOperationException("Библиотека OpenCV вернула пустую матрицу после поворота.");

                // 2. Устраняем скрытое падение конвертера: принудительно приводим к CV_8U (8 бит на канал)
                if (rotatedResult.Depth() != MatType.CV_8U)
                {
                    Mat convertedMat = new Mat();
                    // Преобразуем формат (например, из CV_32F обратно в стандартный CV_8U)
                    rotatedResult.ConvertTo(convertedMat, MatType.CV_8U);
                    rotatedResult.Dispose(); // Уничтожаем промежуточный неподдерживаемый Mat
                    rotatedResult = convertedMat;
                }

                // 3. Сброс и принудительное обновление триггера Binding в WPF
                CurrentDocument.DisplayMat = null;

                // Безопасная очистка старой памяти
                if (CurrentDocument.RotatedImage != null && !CurrentDocument.RotatedImage.IsDisposed)
                    CurrentDocument.RotatedImage.Dispose();

                // Записываем финальный гарантированно валидный Mat в UI
                CurrentDocument.RotatedImage = rotatedResult;
                CurrentDocument.DisplayMat = rotatedResult;
            }
            catch (Exception ex)
            {
                // Выводим реальную причину падения (которую раньше скрывал FallbackValue в XAML)
                ErrorMessage = $"Ошибка трансформации: {ex.Message}";

                // Откат UI в стабильное состояние (показываем исходную картинку)
                CurrentDocument.DisplayMat = CurrentDocument.SourceImage;
            }
            finally
            {
                IsBusy = false;
            }
        }


        /// Асинхронный поиск контура и обрезка выровненного изображения.
        private async Task CropAsync()
        {
            StartProcessing();
            try
            {
                // Работаем с повернутым изображением. Если поворота не было — берем исходное.
                Mat workingMat = CurrentDocument.RotatedImage ?? CurrentDocument.SourceImage;

                // Обрезка области
                Mat croppedMat = await Task.Run(() => ImageAlignment.Crop(CurrentDocument.DetectedRect, workingMat));

                // Безопасно очищаем старый обрезанный Mat
                SafeDisposeMat(CurrentDocument.CroppedImage);

                CurrentDocument.CroppedImage = croppedMat;
                CurrentDocument.DisplayMat = croppedMat; // Выводим финальный результат на экран
                
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Ошибка обработки: {ex.Message}";
            }
            finally
            {
                EndProcessing();
            }
        }

        private async Task DetectAsync()
        {
            StartProcessing();
            try
            {
                // Работаем с повернутым изображением. Если поворота не было — берем исходное.
                Mat workingMat = CurrentDocument.RotatedImage ?? CurrentDocument.SourceImage;
                DetectionParams p = CurrentDocument.DetectionParams;
                Rect rect;
                Mat rectMat;
                // 1. Поиск ограничивающего прямоугольника
                (rect, rectMat) = await Task.Run(() =>
                    ImageAlignment.GetRect(workingMat, p.Color, p.Thickness, p.KernelSize, p.MinArea));

                if (rect.Width <= 0 || rect.Height <= 0)
                    throw new Exception("Объект с заданными параметрами цвета не найден.");

                CurrentDocument.DetectedRect = rect;
                
                CurrentDocument.DisplayMat = rectMat;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Ошибка обработки: {ex.Message}";
            }
            finally
            {
                EndProcessing();
            }
        }

        private async void SaveImageAsync()
        {
            // Проверка на null и пустоту матрицы перед началом
            if (CurrentDocument?.DisplayMat == null || CurrentDocument.DisplayMat.Empty())
            {
                ErrorMessage = "Действие невозможно: нет изображения для сохранения.";
                return;
            }

            // Настройка диалога сохранения
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "Изображение JPEG|*.jpg;*.jpeg|Изображение PNG|*.png|Битмап|*.bmp",
                Title = "Сохранить обработанное изображение",
                // Предлагаем имя на основе исходного файла, если оно есть
                FileName = string.IsNullOrEmpty(CurrentDocument.FilePath)
                    ? "result.jpg"
                    : System.IO.Path.GetFileNameWithoutExtension(CurrentDocument.FilePath) + "_processed"
            };

            // Проверка: если пользователь отменил диалог сохранения
            if (saveFileDialog.ShowDialog() != true)
            {
                return; // Безопасный выход без изменений
            }

            StartProcessing();

            try
            {
                string savePath = saveFileDialog.FileName;

                // Фиксируем ссылку на текущую матрицу для потока
                var matToSave = CurrentDocument.DisplayMat;

                // Асинхронное сохранение средствами OpenCvSharp на диск
                await Task.Run(() => matToSave.SaveImage(savePath));
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Не удалось сохранить файл: {ex.Message}";
            }
            finally
            {
                EndProcessing();
            }
        }

        #region Вспомогательные методы управления состоянием и памятью

        private void StartProcessing()
        {
            IsBusy = true;
            ErrorMessage = string.Empty;
        }

        private void EndProcessing()
        {
            IsBusy = false;
        }

        
        /// Метод безопасной очистки ресурсов нативного объекта Mat.
        private void SafeDisposeMat(Mat mat)
        {
            if (mat != null && !mat.IsDisposed)
            {
                mat.Dispose();
            }
        }

        
        /// Полная очистка всех Mat в текущем документе во избежание утечек памяти.
        private void ClearCurrentDocumentMemory()
        {
            if (CurrentDocument == null) return;

            SafeDisposeMat(CurrentDocument.SourceImage);
            SafeDisposeMat(CurrentDocument.RotatedImage);
            SafeDisposeMat(CurrentDocument.CroppedImage);

            // DisplayMat обычно ссылается на один из объектов выше, 
            // но на всякий случай обнуляем ссылку.
            CurrentDocument.DisplayMat = null;
        }

        #endregion

        #region Реализация INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        #endregion
    }

    
    /// Простая реализация интерфейса ICommand для отделения логики от View (Code-Behind).
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);
        public void Execute(object parameter) => _execute(parameter);
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
