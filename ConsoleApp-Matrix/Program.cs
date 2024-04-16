namespace ConsoleApp_Matrix
{
    internal class Program
    {
        public static volatile Matrix e; //mnożenie Parallel
        static void Main(string[] args)
        {

            int n = 0, threads_number = 0;

            Console.WriteLine("Wprowadź rozmiar macierzy: ");
            n = int.Parse(Console.ReadLine());

            Console.WriteLine($"Twój komputer posiada {Environment.ProcessorCount} rdzeni.");
            Console.WriteLine("Wprowadź liczbę wątków: ");
            threads_number = int.Parse(Console.ReadLine());

            Matrix a = new Matrix(n); //pierwotna
            Matrix b = new Matrix(n); //pierwotna
            Matrix c = new Matrix(n); //bez mnożenia wątkowo
            Matrix d = new Matrix(n); //mnożenie Threads
            e = new Matrix(n);

            a.RandomMatrix();
            b.RandomMatrix();


            int[] threads_fields = new int[threads_number];
            for (int i = 0; i < threads_number; i++)
            {
                threads_fields[i] = n * n / threads_number;
            }
            if (((n * n) % threads_number) > 0)
            {
                for (int i = 0; i < ((n * n) % threads_number); i++)
                {
                    threads_fields[i]++;
                }
            }

            int tmp = 0;
            Multi[] tabMulti = new Multi[threads_number];
            for (int i = 0; i < threads_number; i++)
            {
                tabMulti[i] = new Multi(threads_number, a, b, d, tmp, threads_fields[i]);
                tmp += threads_fields[i];
                tabMulti[i].c = d;
            }


            Thread[] threads = new Thread[threads_number];

            for (int i = 0; i < threads_number; i++)
            {
                threads[i] = new Thread(tabMulti[i].Multiplication);
                threads[i].Name = i.ToString();
            }
            DateTime startTime1 = DateTime.Now;
            foreach (Thread x in threads) x.Start();
            foreach (Thread x in threads) x.Join();

            DateTime stopTime1 = DateTime.Now;
            TimeSpan totalTime1 = stopTime1 - startTime1;


            /*Console.WriteLine("Macierz A:");
            Console.WriteLine(a);

            Console.WriteLine("Macierz B:");
            Console.WriteLine(b);

            Console.WriteLine("Macierz C - Threads:");
            Console.WriteLine(c);*/

            DateTime startTime = DateTime.Now;
            c = a * b;
            DateTime stopTime = DateTime.Now;
            TimeSpan totalTime = stopTime - startTime;

            Console.WriteLine("Macierz A*B:");
            Console.WriteLine(c);

            Console.WriteLine($"Czas wykonania mnożenia macierzy o rozmiarze {n} wyniósł: {totalTime.TotalSeconds} s - Threads: {totalTime1.TotalSeconds} s");

            if (Console.ReadLine() == "p")
            {
                Console.WriteLine("Macierz A:");
                Console.WriteLine(a);

                Console.WriteLine("Macierz B:");
                Console.WriteLine(b);

                Console.WriteLine("Macierz C - Threads:");
                Console.WriteLine(d);
            }

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    e.matrix[i, j] = 0;
                }
            }

            Console.WriteLine("Macierz C - ZERO:");
            Console.WriteLine(e);
            for (int i = 0; i < threads_number; i++)
            {
                tabMulti[i].c = e;
            }

            //threads[i] = new Thread(tabMulti[i].Multiplication);
            ParallelOptions opt = new ParallelOptions() {MaxDegreeOfParallelism = threads_number };
            int[] threadUesed = new int[Environment.ProcessorCount];
            var watch = System.Diagnostics.Stopwatch.StartNew();
            Parallel.ForEach(tabMulti, opt, x => { x.Multiplication(); threadUesed[Thread.CurrentThread.ManagedThreadId]++; }) ;

            Console.WriteLine("Macierz C - Parallel:");
            Console.WriteLine(e);

            Console.WriteLine(string.Join(" ", threadUesed));
            watch.Stop();
            Console.WriteLine($"{threads_number} threads ended in {watch.Elapsed.TotalSeconds} s.");
        }



    }
}
