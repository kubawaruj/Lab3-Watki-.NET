namespace FormPicture
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
            pictureBox1 = new PictureBox();
            butLoad = new Button();
            butProces = new Button();
            pictureBox2 = new PictureBox();
            pictureBox3 = new PictureBox();
            pictureBox4 = new PictureBox();
            pictureBox5 = new PictureBox();
            openFileDialog1 = new OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(12, 28);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(380, 380);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // butLoad
            // 
            butLoad.BackColor = Color.Gainsboro;
            butLoad.FlatAppearance.BorderColor = Color.Black;
            butLoad.FlatStyle = FlatStyle.Popup;
            butLoad.Location = new Point(398, 154);
            butLoad.Name = "butLoad";
            butLoad.Size = new Size(69, 45);
            butLoad.TabIndex = 1;
            butLoad.Text = "Load Image";
            butLoad.UseVisualStyleBackColor = false;
            butLoad.Click += butLoad_Click;
            // 
            // butProces
            // 
            butProces.BackColor = Color.Gainsboro;
            butProces.FlatStyle = FlatStyle.Popup;
            butProces.Location = new Point(398, 222);
            butProces.Name = "butProces";
            butProces.Size = new Size(69, 45);
            butProces.TabIndex = 2;
            butProces.Text = "Parallel Procesing";
            butProces.UseVisualStyleBackColor = false;
            butProces.Click += butProces_Click;
            // 
            // pictureBox2
            // 
            pictureBox2.Location = new Point(472, 28);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(185, 185);
            pictureBox2.TabIndex = 3;
            pictureBox2.TabStop = false;
            // 
            // pictureBox3
            // 
            pictureBox3.Location = new Point(663, 28);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(185, 185);
            pictureBox3.TabIndex = 4;
            pictureBox3.TabStop = false;
            // 
            // pictureBox4
            // 
            pictureBox4.Location = new Point(472, 223);
            pictureBox4.Name = "pictureBox4";
            pictureBox4.Size = new Size(185, 185);
            pictureBox4.TabIndex = 5;
            pictureBox4.TabStop = false;
            // 
            // pictureBox5
            // 
            pictureBox5.Location = new Point(663, 223);
            pictureBox5.Name = "pictureBox5";
            pictureBox5.Size = new Size(185, 185);
            pictureBox5.TabIndex = 6;
            pictureBox5.TabStop = false;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(864, 431);
            Controls.Add(pictureBox5);
            Controls.Add(pictureBox4);
            Controls.Add(pictureBox3);
            Controls.Add(pictureBox2);
            Controls.Add(butProces);
            Controls.Add(butLoad);
            Controls.Add(pictureBox1);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox pictureBox1;
        private Button butLoad;
        private Button butProces;
        private PictureBox pictureBox2;
        private PictureBox pictureBox3;
        private PictureBox pictureBox4;
        private PictureBox pictureBox5;
        private OpenFileDialog openFileDialog1;
    }
}
