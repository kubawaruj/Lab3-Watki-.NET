using System.Threading;
using System.Windows.Forms;

namespace FormPicture
{
    public partial class Form1 : Form
    {
        private Bitmap img;

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

            PictureBox[] listPicBox = { pictureBox2, pictureBox3, pictureBox4, pictureBox5 };

            List<ImgProces> imgProces = new List<ImgProces>
            {
                new ImgProces(0, new Bitmap(img), new Bitmap(img.Width, img.Height)),
                new ImgProces(1, new Bitmap(img), new Bitmap(img.Width, img.Height)),
                new ImgProces(2, new Bitmap(img), new Bitmap(img.Width, img.Height)),
                new ImgProces(3, new Bitmap(img), new Bitmap(img.Width, img.Height))
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
