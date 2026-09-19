using OpenCvSharp;

namespace ImageAlignmentLib
{
    public class ImageAlignment
    {
        private const string PATH = "C:\\Users\\lemon\\source\\repos\\Mobile\\ImageAlignment\\ImageAlignment\\ticket.jpg";

        public static void process(string path = PATH)
        {
            Mat img = ReadImg(path);

            Cv2.NamedWindow("Original");
            Cv2.ImShow("Original", img);

            Mat rot = Rotate(img, 30);

            Cv2.NamedWindow("Rotate");
            Cv2.ImShow("Rotate", rot);


            int a = Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();
        }

        public static Mat Rotate(Mat img, double angle = 90)
        {
            Point2f center = new Point2f(img.Width / 2f, img.Height / 2f);
            double scale = 1.0;
            Mat rotMat = Cv2.GetRotationMatrix2D(center, angle, scale);
            Mat dst = new Mat();
            Cv2.WarpAffine(img, dst, rotMat, img.Size(),
                           InterpolationFlags.Linear,
                           BorderTypes.Constant,
                           Scalar.White);

            return dst;
        }

        public static Mat Crop(Rect rect, Mat img)
        {
            if (rect.Width <= 0 || rect.Height <= 0)
                throw new Exception("Объект не выделен");

            // Гарантируем, что прямоугольник не выходит за границы (на всякий случай)
            rect = rect.Intersect(new Rect(0, 0, img.Width, img.Height));

            // Создаём новое изображение как ROI исходного
            Mat cropped = new Mat(img, rect);

            return cropped;
        }

        public static (Rect, Mat) GetRect( Mat img, Scalar color, int thickness = 10, int size = 3, double minArea = 100.0)
        {
            //Приведение в черно-белый вид
            var channels = Cv2.Split(img);
            Mat gray = new Mat();
            Cv2.CvtColor(img, gray, ColorConversionCodes.BGR2GRAY);

            //Размытие (убирание шума)
            Mat blurred = new Mat();
            Size ksize = new Size(size, size);
            double sigmaX = 1.5;
            double sigmaY = 0;
            Cv2.GaussianBlur(gray, blurred, ksize, sigmaX, sigmaY, BorderTypes.Default);

            //Бинаризация
            Mat monoChrome = new Mat();
            Cv2.AdaptiveThreshold(blurred, monoChrome, 255, AdaptiveThresholdTypes.GaussianC, ThresholdTypes.BinaryInv, 15, 5);

            //Очистка от мелких пятен (черных и белых)
            Mat withoutNoise = new Mat();
            Size noiseSize = new Size(size, size);
            Mat noiseEllipse = Cv2.GetStructuringElement(MorphShapes.Ellipse, noiseSize);
            Cv2.MorphologyEx(monoChrome, withoutNoise, MorphTypes.Open, noiseEllipse);

            Mat cleared = new Mat();
            Size gapSize = new Size(size, size);
            Mat gapsEllips = Cv2.GetStructuringElement(MorphShapes.Ellipse, gapSize);
            Cv2.MorphologyEx(withoutNoise, cleared, MorphTypes.Close, gapsEllips);

            //Поиск контуров
            Point[][] contours;
            HierarchyIndex[] hierarchy;
            Cv2.FindContours(
                image: cleared,
                contours: out contours,
                hierarchy: out hierarchy,
                mode: RetrievalModes.External,
                method: ContourApproximationModes.ApproxSimple
                );

            if (contours.Length == 0)
            {
                Console.WriteLine("Объекты не найдены.");
                throw new Exception();
            }

            var filtered = new List<Point[]>();
            for (int i = 0; i < contours.Length; i++)
            {
                var cnt = contours[i];
                if (cnt == null || cnt.Length < 3) continue;
                double area = Math.Abs(Cv2.ContourArea(cnt));
                if (area >= minArea) filtered.Add(cnt);
            }

            Rect overallRect = Cv2.BoundingRect(filtered[0]);
            for (int i = 1; i < filtered.Count; i++)
            {
                Rect r = Cv2.BoundingRect(filtered[i]);
                overallRect = overallRect.Union(r);
            }

            Mat rectMat = img.Clone();
            Cv2.Rectangle(rectMat, overallRect, color, thickness);

            return (overallRect, rectMat);
        }

        public static Mat ReadImg(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException("path");
            var mat = Cv2.ImRead(path, ImreadModes.Color);
            if (mat == null || mat.Empty()) throw new FileNotFoundException($"Cannot read image: {path}");
            return mat;
        }
    }
}
