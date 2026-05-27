namespace wfaGameCounter
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
            tableLayoutPanel1 = new TableLayoutPanel();
            laCountCorrect = new Label();
            laCountIncorrect = new Label();
            tableLayoutPanel2 = new TableLayoutPanel();
            buYes = new Button();
            buNo = new Button();
            laQuestion = new Label();
            label4 = new Label();
            laStatus = new Label();
            laTimer = new Label();
            laCoins = new Label();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(laCountCorrect, 0, 0);
            tableLayoutPanel1.Controls.Add(laCountIncorrect, 1, 0);
            tableLayoutPanel1.Dock = DockStyle.Top;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(724, 125);
            tableLayoutPanel1.TabIndex = 0;
            tableLayoutPanel1.Paint += tableLayoutPanel1_Paint;
            // 
            // laCountCorrect
            // 
            laCountCorrect.BackColor = Color.FromArgb(192, 255, 192);
            laCountCorrect.Dock = DockStyle.Fill;
            laCountCorrect.Font = new Font("Segoe UI Semibold", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 204);
            laCountCorrect.Location = new Point(3, 0);
            laCountCorrect.Name = "laCountCorrect";
            laCountCorrect.Size = new Size(356, 125);
            laCountCorrect.TabIndex = 0;
            laCountCorrect.Text = "ВЕРНО = 0";
            laCountCorrect.TextAlign = ContentAlignment.MiddleCenter;
            laCountCorrect.Click += label1_Click;
            // 
            // laCountIncorrect
            // 
            laCountIncorrect.BackColor = Color.FromArgb(255, 192, 192);
            laCountIncorrect.Dock = DockStyle.Fill;
            laCountIncorrect.Font = new Font("Segoe UI Semibold", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            laCountIncorrect.ForeColor = Color.Black;
            laCountIncorrect.Location = new Point(365, 0);
            laCountIncorrect.Name = "laCountIncorrect";
            laCountIncorrect.Size = new Size(356, 125);
            laCountIncorrect.TabIndex = 1;
            laCountIncorrect.Text = "НЕВЕРНО = 0";
            laCountIncorrect.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Controls.Add(buYes, 0, 0);
            tableLayoutPanel2.Controls.Add(buNo, 1, 0);
            tableLayoutPanel2.Dock = DockStyle.Bottom;
            tableLayoutPanel2.Location = new Point(0, 353);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Size = new Size(724, 125);
            tableLayoutPanel2.TabIndex = 1;
            // 
            // buYes
            // 
            buYes.Dock = DockStyle.Fill;
            buYes.Font = new Font("Segoe UI Semibold", 16.2F, FontStyle.Bold);
            buYes.ForeColor = Color.Lime;
            buYes.Location = new Point(3, 3);
            buYes.Name = "buYes";
            buYes.Size = new Size(356, 119);
            buYes.TabIndex = 0;
            buYes.Text = "ДА";
            buYes.UseVisualStyleBackColor = true;
            buYes.Click += buYes_Click;
            // 
            // buNo
            // 
            buNo.Dock = DockStyle.Fill;
            buNo.Font = new Font("Segoe UI Semibold", 16.2F, FontStyle.Bold);
            buNo.ForeColor = Color.Red;
            buNo.Location = new Point(365, 3);
            buNo.Name = "buNo";
            buNo.Size = new Size(356, 119);
            buNo.TabIndex = 1;
            buNo.Text = "НЕТ";
            buNo.UseVisualStyleBackColor = true;
            buNo.Click += buNo_Click;
            // 
            // laQuestion
            // 
            laQuestion.Dock = DockStyle.Fill;
            laQuestion.Font = new Font("Segoe UI Symbol", 22.8000011F, FontStyle.Bold, GraphicsUnit.Point, 0);
            laQuestion.Location = new Point(0, 125);
            laQuestion.Name = "laQuestion";
            laQuestion.Size = new Size(724, 228);
            laQuestion.TabIndex = 2;
            laQuestion.Text = "10 + 11 = 21";
            laQuestion.TextAlign = ContentAlignment.MiddleCenter;
            laQuestion.Click += label3_Click_1;
            // 
            // label4
            // 
            label4.Dock = DockStyle.Bottom;
            label4.Font = new Font("Segoe UI Semibold", 13.8F, FontStyle.Bold);
            label4.Location = new Point(0, 317);
            label4.Name = "label4";
            label4.Size = new Size(724, 36);
            label4.TabIndex = 3;
            label4.Text = "Верно?";
            label4.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // laStatus
            // 
            laStatus.AutoSize = true;
            laStatus.Location = new Point(566, 206);
            laStatus.Name = "laStatus";
            laStatus.Size = new Size(50, 20);
            laStatus.TabIndex = 4;
            laStatus.Text = "label1";
            // 
            // laTimer
            // 
            laTimer.AutoSize = true;
            laTimer.Location = new Point(550, 307);
            laTimer.Name = "laTimer";
            laTimer.Size = new Size(50, 20);
            laTimer.TabIndex = 5;
            laTimer.Text = "label2";
            // 
            // laCoins
            // 
            laCoins.AutoSize = true;
            laCoins.Location = new Point(306, 159);
            laCoins.Name = "laCoins";
            laCoins.Size = new Size(50, 20);
            laCoins.TabIndex = 6;
            laCoins.Text = "label3";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(724, 478);
            Controls.Add(laCoins);
            Controls.Add(laTimer);
            Controls.Add(laStatus);
            Controls.Add(label4);
            Controls.Add(laQuestion);
            Controls.Add(tableLayoutPanel2);
            Controls.Add(tableLayoutPanel1);
            MinimumSize = new Size(446, 429);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Label laCountCorrect;
        private TableLayoutPanel tableLayoutPanel2;
        private Button buYes;
        private Button buNo;
        private Label laCountIncorrect;
        private Label laQuestion;
        private Label label4;
        private EventHandler button1_Click;
        private Label laStatus;
        private Label laTimer;
        private Label laCoins;
    }
}
