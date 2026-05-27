namespace wfaGraphicSinCos
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            this.BackgroundImageLayout = ImageLayout.None;
            this.Text += " : (sin - красный, cos - зеленый, tan - синий)";
            DrawAll();
            this.ResizeEnd += (s, e) => DrawAll();
        }

        private void DrawAll()
        {
            MyGraphic myGraphic = new(
                            this.ClientSize.Width,
                            this.ClientSize.Height);

            myGraphic.DrawAxes();
            myGraphic.DrawSin(Color.Red);
            myGraphic.DrawCos(Color.Green);

            this.BackgroundImage = myGraphic.GetBitmap();
        }
    }
}
