namespace ggame
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private Panel panel1;
        private Label label1;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            panel1 = new Panel();
            label1 = new Label();
            Back = new Button();
            pictureBox1 = new PictureBox();
            button1 = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Location = new Point(14, 77);
            panel1.Margin = new Padding(3, 4, 3, 4);
            panel1.Name = "panel1";
            panel1.Size = new Size(735, 719);
            panel1.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(744, 205);
            label1.Name = "label1";
            label1.Size = new Size(76, 31);
            label1.TabIndex = 1;
            label1.Text = "label1";
            // 
            // Back
            // 
            Back.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            Back.Location = new Point(14, 12);
            Back.Name = "Back";
            Back.Size = new Size(130, 47);
            Back.TabIndex = 2;
            Back.Text = "返回";
            Back.UseVisualStyleBackColor = true;
            Back.Click += Back_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(744, 77);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(125, 125);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 3;
            pictureBox1.TabStop = false;
            // 
            // button1
            // 
            button1.Font = new Font("Microsoft JhengHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            button1.Location = new Point(522, 13);
            button1.Name = "button1";
            button1.Size = new Size(130, 47);
            button1.TabIndex = 4;
            button1.Text = "重置";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(9F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            ClientSize = new Size(938, 808);
            ControlBox = false;
            Controls.Add(button1);
            Controls.Add(pictureBox1);
            Controls.Add(Back);
            Controls.Add(label1);
            Controls.Add(panel1);
            Margin = new Padding(3, 4, 3, 4);
            MaximizeBox = false;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Reversi";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private Button Back;
        private PictureBox pictureBox1;
        private Button button1;
    }
}