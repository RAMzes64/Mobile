namespace wfaGraphicSinCos
{
    internal class MyGraphic
    {
        public MyGraphic(int width, int height)
        {
            Width = width;
            Height = height;

            b = new Bitmap(Width, Height);
            g = Graphics.FromImage(b);

            

            grShiftY = b.Height / 2;
            grHeight = grShiftY * 0.9;   
            
        }

        public int Width { get; }
        public int Height { get; }
        public int DotDiameter { get; } = 4;
        public double CountWave { get; private set; } = 5;
        private Bitmap b;
        private Graphics g;
        private int grShiftY;
        private double grHeight;

        internal void DrawAxes()
        {
            g.DrawLine(new Pen(Color.Black), 0, grShiftY, b.Width, grShiftY);
            for (int i = 0; i < CountWave; i++)
            {
                var _x = b.Width / CountWave * i;
                //g.DrawLine(new Pen(Color.DarkGray), (int)_x, 0, (int)_x, b.Height);
                g.DrawLine(new Pen(Color.DarkGray, 2), (int)_x, grShiftY - 30, (int)_x, grShiftY + 30);
            }
            //g.DrawLine(new Pen(Color.Black), 0, 0, 0, b.Height);
            g.DrawLine(Pens.Black, 0, 0, 0, b.Height);
        }

        internal void DrawSin(Color color)
        {
            double _x;
            double _y;
            for (int i = 0; i < b.Width; i++)
            {
                _x = i;
                var grWidthPI = Math.PI / (b.Width - 1);
                _y = grShiftY - Math.Sin(i * grWidthPI * CountWave) * grHeight;
                g.FillEllipse(new SolidBrush(color),
                    (int)_x - DotDiameter / 2, (int)_y - DotDiameter / 2, DotDiameter, DotDiameter);
            }
        }

        internal void DrawCos(Color color)
        {
            double _x;
            double _y;
            for (int i = 0; i < b.Width; i++)
            {
                _x = i;
                var grWidthPI = Math.PI / (b.Width - 1);
                _y = grShiftY - Math.Cos(i * grWidthPI * CountWave) * grHeight;
                g.FillEllipse(new SolidBrush(color),
                    (int)_x - DotDiameter / 2, (int)_y - DotDiameter / 2, DotDiameter, DotDiameter);
            }
        }

        internal Bitmap? GetBitmap()
        {
            return b;
        }
    }
}