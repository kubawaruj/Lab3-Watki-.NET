namespace ConsoleApp_Matrix
{
    internal class Multi
    {
        public int threads_number { get; set; }
        public int start { get; set; }
        public int fields { get; set; }
        public Matrix a;
        public Matrix b;
        public Matrix c;

        public Multi(int number, Matrix _a, Matrix _b, Matrix _c, int _start, int _fields)
        {
            threads_number = number;
            a = _a;
            b = _b;
            c = _c;
            start = _start;
            fields = _fields;
        }

        public void Welcome()
        {
            Console.WriteLine($"{Thread.CurrentThread.Name} : Hello !! fields={fields}, start={start}");
        }
        public void Multiplication()
        {
            int row = 0;
            int col = 0;

            for (int i = 0; i < fields; i++)
            {
                row = (start + i) / a.n;
                col = (start + i) % a.n;
                for (int j = 0; j < a.n; j++)
                {
                    c.matrix[row, col] += a.matrix[row, j] * b.matrix[j, col];
                }
            }


        }
    }
}
