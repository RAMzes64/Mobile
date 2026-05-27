using System.Drawing.Imaging;

namespace wfaPaint
{
    public partial class Form1 : Form
    {
        private enum MyDrawMode
        {
            Pencil,
            Line,
            Ellipse,
            Rectangle,
            Arrow,
            Star
        }
        private Bitmap b;
        private Graphics g;
        private MyDrawMode myDrawMode = MyDrawMode.Pencil;
        private Point startLocation;
        private Bitmap bb;
        private Pen myPen;

        public Form1()
        {
            InitializeComponent();

            b = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            g = Graphics.FromImage(b);

            myPen = new Pen(paColor1.BackColor, 10);
            myPen.StartCap = myPen.EndCap = System.Drawing.Drawing2D.LineCap.Round;

            paColor1.Click += (s, e) => myPen.Color = paColor1.BackColor;
            paColor2.Click += (s, e) => myPen.Color = paColor2.BackColor;
            paColor3.Click += (s, e) => myPen.Color = paColor3.BackColor;
            paColor4.Click += (s, e) => myPen.Color = paColor4.BackColor;
            paColor5.Click += (s, e) => myPen.Color = paColor5.BackColor;

            trPenWidth.Minimum = 1;
            trPenWidth.Maximum = 20;
            trPenWidth.Value = Convert.ToInt32(myPen.Width);
            trPenWidth.ValueChanged += (s, e) => myPen.Width = trPenWidth.Value;


            buModPencil.Click += (s, e) => myDrawMode = MyDrawMode.Pencil;
            buModLine.Click += (s, e) => myDrawMode = MyDrawMode.Line;
            buModEllipse.Click += (s, e) => myDrawMode = MyDrawMode.Ellipse;
            buModArrow.Click += (s, e) => myDrawMode = MyDrawMode.Arrow;
            buModStar.Click += (s, e) => myDrawMode = MyDrawMode.Star;
            buodRectangle.Click += (s, e) => myDrawMode = MyDrawMode.Rectangle;

            pxImage.MouseDown += PxImage_MouseDown;
            pxImage.MouseMove += PxImage_MouseMove;
            pxImage.MouseUp += PxImage_MouseUp;
            pxImage.Paint += (s, e) => e.Graphics.DrawImage(b, 0, 0);

            buImageClear.Click += BuImageClear_Click;
            buImageSaveToFile.Click += BuImageSaveToFile_Click;
            buImageLoadFromFile.Click += BuImageLoadFromFile_Click;
            buCopyToCliapboard.Click += (s, e) => Clipboard.SetImage(b);
        }

        private void BuImageLoadFromFile_Click(object? sender, EventArgs e)
        {
            OpenFileDialog dialog = new();
            dialog.Filter = "Image Files (*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                g.Clear(DefaultBackColor);
                g.DrawImage(Bitmap.FromFile(dialog.FileName), 0, 0);
                pxImage.Invalidate();
            }
        }

        private void BuImageSaveToFile_Click(object? sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Image Files (*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                b.Save(dialog.FileName);
            }
        }

        private void BuImageClear_Click(object? sender, EventArgs e)
        {
            g.Clear(DefaultBackColor);
            pxImage.Invalidate();
        }

        private void PxImage_MouseUp(object? sender, MouseEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void PxImage_MouseMove(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                switch (myDrawMode)
                {
                    case MyDrawMode.Pencil:
                        g.DrawLine(myPen, startLocation, e.Location);
                        startLocation = e.Location;
                        break;
                    case MyDrawMode.Line:
                        RestoreBitmap();
                        g.DrawLine(myPen, startLocation, e.Location);
                        break;
                    case MyDrawMode.Ellipse:
                        RestoreBitmap();
                        g.DrawEllipse(myPen, startLocation.X, startLocation.Y, e.Location.X - startLocation.X, e.Location.Y - startLocation.Y);
                        break;
                    case MyDrawMode.Rectangle:
                        RestoreBitmap();
                        g.DrawRectangle(myPen,
                            startLocation.X,
                            startLocation.Y,
                            e.Location.X - startLocation.X,
                            e.Location.Y - startLocation.Y);
                        break;
                    case MyDrawMode.Arrow:
                        RestoreBitmap();
                        g.DrawLine(myPen, startLocation.X, startLocation.Y + (e.Y - startLocation.Y) / 4 , startLocation.X, startLocation.Y + ((e.Y - startLocation.Y)* 3/4) );
                        g.DrawLine(myPen, startLocation.X, startLocation.Y + ((e.Y - startLocation.Y) * 3 / 4), startLocation.X + (e.X - startLocation.X) / 2, startLocation.Y + ((e.Y - startLocation.Y)* 3/4) );
                        g.DrawLine(myPen, startLocation.X + (e.X - startLocation.X) / 2, startLocation.Y + ((e.Y - startLocation.Y) * 3 / 4), startLocation.X + (e.X - startLocation.X) / 2, e.Y);
                        g.DrawLine(myPen, startLocation.X + (e.X - startLocation.X) / 2, e.Y, e.X, startLocation.Y + (e.Y - startLocation.Y)/2);
                        g.DrawLine(myPen, e.X, startLocation.Y + (e.Y - startLocation.Y) / 2, startLocation.X + (e.X - startLocation.X) / 2, startLocation.Y);
                        g.DrawLine(myPen, startLocation.X + (e.X - startLocation.X) / 2, startLocation.Y, startLocation.X + (e.X - startLocation.X) / 2, startLocation.Y + (e.Y - startLocation.Y)/4);
                        g.DrawLine(myPen, startLocation.X + (e.X - startLocation.X) / 2, startLocation.Y + (e.Y - startLocation.Y) / 4, startLocation.X, startLocation.Y + (e.Y - startLocation.Y)/4);
                        break;
                    case MyDrawMode.Star:
                        RestoreBitmap();
                        g.DrawLine(myPen, startLocation.X + (e.X - startLocation.X)/4, startLocation.Y, startLocation.X + (e.X - startLocation.X) / 3, startLocation.Y + ((e.Y - startLocation.Y) * 1/3));
                        g.DrawLine(myPen, startLocation.X + (e.X - startLocation.X) / 3, startLocation.Y + ((e.Y - startLocation.Y) * 1 / 3), startLocation.X, startLocation.Y + ((e.Y - startLocation.Y) * 2/3));
                        g.DrawLine(myPen, startLocation.X, startLocation.Y + ((e.Y - startLocation.Y) * 2/3), startLocation.X + ((e.X - startLocation.X) * 3/7), startLocation.Y + ((e.Y - startLocation.Y) * 2 / 3));
                        g.DrawLine(myPen, startLocation.X + ((e.X - startLocation.X) * 3 / 7), startLocation.Y + ((e.Y - startLocation.Y) * 2 / 3), startLocation.X + (e.X - startLocation.X) / 2, e.Y);
                        g.DrawLine(myPen, startLocation.X + (e.X - startLocation.X) / 2, e.Y, startLocation.X + ((e.X - startLocation.X) * 4/7), startLocation.Y + ((e.Y - startLocation.Y) * 2/3));
                        g.DrawLine(myPen, startLocation.X + ((e.X - startLocation.X) * 4 / 7), startLocation.Y + ((e.Y - startLocation.Y) * 2 / 3), e.X, startLocation.Y + ((e.Y - startLocation.Y) * 2 / 3));
                        g.DrawLine(myPen, e.X, startLocation.Y + ((e.Y - startLocation.Y) * 2 / 3), (startLocation.X + (e.X - startLocation.X) * 2/ 3), startLocation.Y + (e.Y - startLocation.Y) / 3);
                        g.DrawLine(myPen, (startLocation.X + (e.X - startLocation.X) * 2 / 3), startLocation.Y + (e.Y - startLocation.Y) / 3, (startLocation.X + (e.X - startLocation.X) * 3/4), startLocation.Y);
                        g.DrawLine(myPen, (startLocation.X + (e.X - startLocation.X) * 3 / 4), startLocation.Y, startLocation.X + (e.X - startLocation.X) / 2, startLocation.Y + (e.Y - startLocation.Y) / 4);
                        g.DrawLine(myPen, startLocation.X + (e.X - startLocation.X) / 2, startLocation.Y + (e.Y - startLocation.Y) / 4, startLocation.X + (e.X - startLocation.X) / 4, startLocation.Y);
                        
                        break;
                    default:
                        break;
                }

                pxImage.Invalidate();
            }
        }

        private void RestoreBitmap()
        {
            g.Dispose();
            b = (Bitmap)bb.Clone();
            g = Graphics.FromImage(b);
        }

        private void PxImage_MouseDown(object? sender, MouseEventArgs e)
        {
            startLocation = e.Location;
            bb = (Bitmap)b.Clone();
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
