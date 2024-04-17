namespace FormPicture
{
    public partial class Form1 : Form
    {
        public Bitmap img;

        public Form1()
        {
            InitializeComponent();
        }

        private void butLoad_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "jpg files (*.jpg)|*.jpg|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var file = openFileDialog1.FileName;
                if (file != null)
                {
                    img = new Bitmap(file);
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    pictureBox1.Image = img;
                }
            }
        }
        private void butProces_Click(object sender, EventArgs e)
        {
            List<Bitmap> imgProcessed = new List<Bitmap>
            {
                new Bitmap(img.Width, img.Height),
                new Bitmap(img.Width, img.Height),
                new Bitmap(img.Width, img.Height),
                new Bitmap(img.Width, img.Height)
            };

            PictureBox[] listPicBox = { pictureBox2, pictureBox3, pictureBox4, pictureBox5 };

            List<ImgProces> imgProces = new List<ImgProces>
            {
                new ImgProces(0, img, imgProcessed[0]),
                new ImgProces(1, img, imgProcessed[1]),
                new ImgProces(2, img, imgProcessed[2]),
                new ImgProces(3, img, imgProcessed[3])
            };
            ParallelOptions opt = new ParallelOptions() { MaxDegreeOfParallelism = 4 };
            Parallel.ForEach(imgProces, opt, x =>
            {
                x.Process();
                listPicBox[x.imgFilter].SizeMode = PictureBoxSizeMode.StretchImage;
                listPicBox[x.imgFilter].Image = x.imgProcessed;
            });
        }


    }
}
