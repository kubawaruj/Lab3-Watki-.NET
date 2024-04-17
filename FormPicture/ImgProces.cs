using System.Drawing.Imaging;

namespace FormPicture
{
    internal class ImgProces
    {
        public volatile Bitmap? imgProcessed;
        private static Mutex _mutex = new Mutex();
        public int imgFilter { get; set; }
        public static Bitmap img { get; set; }

        public ImgProces(int _filter, Bitmap _img, Bitmap _imgProc)
        {
            imgFilter = _filter;
            img = _img;
            imgProcessed = _imgProc;
        }


        public void Process()
        {
            switch (imgFilter)
            {
                case 0:
                    imgProcessed = Negatyw(img);
                    
                    break;
                //progowanie();
                case 1:
                    imgProcessed = Progowanie(img);
                    //Odcinani_Szarosci();
                    break;
                case 2:
                    //tmp = Negatyw();
                    break;
                case 3:
                    //Wykrywanie_Krawendzi();
                    break;
                default:
                    break;
            }

        }
        private Bitmap Progowanie1(Bitmap _img)
        {
            Bitmap tmp = _img;
            _mutex.WaitOne();
            tmp = Negatyw(_img);

            //_mutex.WaitOne();
            for (int i = 0; i < tmp.Width; i++)
            {
                for (int j = 0; j < tmp.Height; j++)
                {
                    if (tmp.GetPixel(i, j).R > 128)
                    {
                        tmp.SetPixel(i, j, Color.Black);
                    }
                    else
                    {
                        tmp.SetPixel(i, j, Color.White);
                    }
                }
            }
            _mutex.ReleaseMutex();
            return tmp;
        }
        private Bitmap Progowanie(Bitmap _img)
        {
            Bitmap tmp = _img;
            

            //Pobierz wartosc wszystkich punktow obrazu
            _mutex.WaitOne();
            tmp = Negatyw(_img);
            BitmapData bmpData = tmp.LockBits(new Rectangle(0, 0, _img.Width, _img.Height), ImageLockMode.ReadWrite, tmp.PixelFormat);
            byte[] pixelValues = new byte[Math.Abs(bmpData.Stride) * _img.Height];
            System.Runtime.InteropServices.Marshal.Copy(bmpData.Scan0, pixelValues, 0, pixelValues.Length);

            byte value;
            //Dla kolejnych punktow obrazu
            for (int i = 0; i < (_img.Width * _img.Height); i++)
            {
                //Wartosci kolorow zapisane sa w kolejnosci Blue, Green, Red
                if (pixelValues[3 * i] > (pixelValues.Max() / 2))
                {
                    value = pixelValues.Max();
                }
                else
                {
                    value = pixelValues.Min();
                }

                pixelValues[3 * i] = value;
                pixelValues[3 * i + 1] = value;
                pixelValues[3 * i + 2] = value;
            }

            //Wczytaj przetworzony obraz
            System.Runtime.InteropServices.Marshal.Copy(pixelValues, 0, bmpData.Scan0, pixelValues.Length);
            tmp.UnlockBits(bmpData);
            _mutex.ReleaseMutex();
            return tmp;

        }
        private Bitmap Negatyw(Bitmap _img)
        {

            //Pobierz wartosc wszystkich punktow obrazu
            _mutex.WaitOne();
            Bitmap bitmap = _img;
            BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, _img.Width, _img.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
            byte[] pixelValues = new byte[Math.Abs(bmpData.Stride) * _img.Height];
            System.Runtime.InteropServices.Marshal.Copy(bmpData.Scan0, pixelValues, 0, pixelValues.Length);

            //Dla kolejnych punktow obrazu
            for (int i = 0; i < (_img.Width * _img.Height); i++)
            {
                //Wartosci kolorow zapisane sa w kolejnosci Blue, Green, Red
                byte value = (byte)(0.299 * pixelValues[3 * i + 2] +
                0.587 * pixelValues[3 * i + 1] +
                0.114 * pixelValues[3 * i]);
                pixelValues[3 * i] = value;
                pixelValues[3 * i + 1] = value;
                pixelValues[3 * i + 2] = value;
            }

            //Wczytaj przetworzony obraz
            System.Runtime.InteropServices.Marshal.Copy(pixelValues, 0, bmpData.Scan0, pixelValues.Length);
            bitmap.UnlockBits(bmpData);
            _mutex.ReleaseMutex();
            return bitmap;

        }
    }
}
