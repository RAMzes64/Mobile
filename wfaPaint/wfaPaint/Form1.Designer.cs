namespace wfaPaint
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panel1 = new Panel();
            buModStar = new Button();
            buModArrow = new Button();
            buCopyToCliapboard = new Button();
            buImageLoadFromFile = new Button();
            buImageSaveToFile = new Button();
            buImageClear = new Button();
            buodRectangle = new Button();
            buModEllipse = new Button();
            buModLine = new Button();
            buModPencil = new Button();
            trPenWidth = new TrackBar();
            paColor5 = new Panel();
            paColor4 = new Panel();
            paColor3 = new Panel();
            paColor2 = new Panel();
            paColor1 = new Panel();
            pxImage = new PictureBox();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trPenWidth).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pxImage).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(buModStar);
            panel1.Controls.Add(buModArrow);
            panel1.Controls.Add(buCopyToCliapboard);
            panel1.Controls.Add(buImageLoadFromFile);
            panel1.Controls.Add(buImageSaveToFile);
            panel1.Controls.Add(buImageClear);
            panel1.Controls.Add(buodRectangle);
            panel1.Controls.Add(buModEllipse);
            panel1.Controls.Add(buModLine);
            panel1.Controls.Add(buModPencil);
            panel1.Controls.Add(trPenWidth);
            panel1.Controls.Add(paColor5);
            panel1.Controls.Add(paColor4);
            panel1.Controls.Add(paColor3);
            panel1.Controls.Add(paColor2);
            panel1.Controls.Add(paColor1);
            panel1.Dock = DockStyle.Left;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(250, 450);
            panel1.TabIndex = 0;
            // 
            // buModStar
            // 
            buModStar.Location = new Point(148, 313);
            buModStar.Name = "buModStar";
            buModStar.Size = new Size(94, 29);
            buModStar.TabIndex = 12;
            buModStar.Text = "Star";
            buModStar.UseVisualStyleBackColor = true;
            // 
            // buModArrow
            // 
            buModArrow.Location = new Point(12, 261);
            buModArrow.Name = "buModArrow";
            buModArrow.Size = new Size(230, 29);
            buModArrow.TabIndex = 11;
            buModArrow.Text = "Arrow";
            buModArrow.UseVisualStyleBackColor = true;
            // 
            // buCopyToCliapboard
            // 
            buCopyToCliapboard.Location = new Point(12, 418);
            buCopyToCliapboard.Name = "buCopyToCliapboard";
            buCopyToCliapboard.Size = new Size(132, 29);
            buCopyToCliapboard.TabIndex = 10;
            buCopyToCliapboard.Text = "Copy to cliapboard";
            buCopyToCliapboard.UseVisualStyleBackColor = true;
            buCopyToCliapboard.Click += button1_Click;
            // 
            // buImageLoadFromFile
            // 
            buImageLoadFromFile.Location = new Point(12, 383);
            buImageLoadFromFile.Name = "buImageLoadFromFile";
            buImageLoadFromFile.Size = new Size(132, 29);
            buImageLoadFromFile.TabIndex = 9;
            buImageLoadFromFile.Text = "Load from file";
            buImageLoadFromFile.UseVisualStyleBackColor = true;
            // 
            // buImageSaveToFile
            // 
            buImageSaveToFile.Location = new Point(12, 348);
            buImageSaveToFile.Name = "buImageSaveToFile";
            buImageSaveToFile.Size = new Size(132, 29);
            buImageSaveToFile.TabIndex = 8;
            buImageSaveToFile.Text = "Save to file";
            buImageSaveToFile.UseVisualStyleBackColor = true;
            // 
            // buImageClear
            // 
            buImageClear.Location = new Point(12, 313);
            buImageClear.Name = "buImageClear";
            buImageClear.Size = new Size(132, 29);
            buImageClear.TabIndex = 7;
            buImageClear.Text = "Clear Image";
            buImageClear.UseVisualStyleBackColor = true;
            buImageClear.Click += button4_Click;
            // 
            // buodRectangle
            // 
            buodRectangle.Location = new Point(12, 226);
            buodRectangle.Name = "buodRectangle";
            buodRectangle.Size = new Size(230, 29);
            buodRectangle.TabIndex = 6;
            buodRectangle.Text = "Rectangle";
            buodRectangle.UseVisualStyleBackColor = true;
            // 
            // buModEllipse
            // 
            buModEllipse.Location = new Point(12, 191);
            buModEllipse.Name = "buModEllipse";
            buModEllipse.Size = new Size(230, 29);
            buModEllipse.TabIndex = 5;
            buModEllipse.Text = "Ellipse";
            buModEllipse.UseVisualStyleBackColor = true;
            // 
            // buModLine
            // 
            buModLine.Location = new Point(12, 156);
            buModLine.Name = "buModLine";
            buModLine.Size = new Size(230, 29);
            buModLine.TabIndex = 4;
            buModLine.Text = "Line";
            buModLine.UseVisualStyleBackColor = true;
            // 
            // buModPencil
            // 
            buModPencil.Location = new Point(12, 121);
            buModPencil.Name = "buModPencil";
            buModPencil.Size = new Size(230, 29);
            buModPencil.TabIndex = 3;
            buModPencil.Text = "Pencil";
            buModPencil.UseVisualStyleBackColor = true;
            // 
            // trPenWidth
            // 
            trPenWidth.Location = new Point(12, 80);
            trPenWidth.Name = "trPenWidth";
            trPenWidth.Size = new Size(230, 56);
            trPenWidth.TabIndex = 2;
            // 
            // paColor5
            // 
            paColor5.BackColor = Color.Black;
            paColor5.Location = new Point(199, 19);
            paColor5.Name = "paColor5";
            paColor5.Size = new Size(43, 42);
            paColor5.TabIndex = 1;
            // 
            // paColor4
            // 
            paColor4.BackColor = Color.Yellow;
            paColor4.Location = new Point(150, 19);
            paColor4.Name = "paColor4";
            paColor4.Size = new Size(43, 42);
            paColor4.TabIndex = 1;
            // 
            // paColor3
            // 
            paColor3.BackColor = Color.FromArgb(192, 0, 192);
            paColor3.Location = new Point(101, 19);
            paColor3.Name = "paColor3";
            paColor3.Size = new Size(43, 42);
            paColor3.TabIndex = 1;
            // 
            // paColor2
            // 
            paColor2.BackColor = Color.Lime;
            paColor2.Location = new Point(52, 19);
            paColor2.Name = "paColor2";
            paColor2.Size = new Size(43, 42);
            paColor2.TabIndex = 1;
            // 
            // paColor1
            // 
            paColor1.BackColor = Color.Red;
            paColor1.Location = new Point(3, 19);
            paColor1.Name = "paColor1";
            paColor1.Size = new Size(43, 42);
            paColor1.TabIndex = 0;
            // 
            // pxImage
            // 
            pxImage.Dock = DockStyle.Fill;
            pxImage.Location = new Point(250, 0);
            pxImage.Name = "pxImage";
            pxImage.Size = new Size(550, 450);
            pxImage.TabIndex = 1;
            pxImage.TabStop = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(pxImage);
            Controls.Add(panel1);
            Name = "Form1";
            Text = "wfaPaint";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trPenWidth).EndInit();
            ((System.ComponentModel.ISupportInitialize)pxImage).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel paColor5;
        private Panel paColor4;
        private Panel paColor3;
        private Panel paColor2;
        private Panel paColor1;
        private PictureBox pxImage;
        private Button buImageLoadFromFile;
        private Button buImageSaveToFile;
        private Button buImageClear;
        private Button buodRectangle;
        private Button buModEllipse;
        private Button buModLine;
        private Button buModPencil;
        private TrackBar trPenWidth;
        private Button buCopyToCliapboard;
        private Button buModArrow;
        private Button buModStar;
    }
}
