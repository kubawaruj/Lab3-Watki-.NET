namespace ConsoleApp_Matrix
{
    internal class Program
    {
        public static volatile Matrix e; //wynikowa mnożenie Parallel
        static void Main(string[] args)
        {

            int n = 0, threads_number = 0;
            bool print = false;

            Console.WriteLine("Wprowadź rozmiar macierzy: ");
            n = int.Parse(Console.ReadLine());

            Console.WriteLine($"\nTwój komputer posiada {Environment.ProcessorCount} rdzeni.");
            Console.WriteLine("Wprowadź liczbę wątków: ");
            threads_number = int.Parse(Console.ReadLine());

            Console.WriteLine("Czy wyświetlać macierze? true/false: ");
            print = bool.Parse(Console.ReadLine());

            Matrix a = new Matrix(n); //pierwotna
            Matrix b = new Matrix(n); //pierwotna
            Matrix c = new Matrix(n); //wynikowa bez mnożenia wątkowo
            Matrix d = new Matrix(n); //wynikowa mnożenie Threads
            e = new Matrix(n);

            a.RandomMatrix();
            b.RandomMatrix();

            var watchC = System.Diagnostics.Stopwatch.StartNew();
            c = a * b;
            watchC.Stop();

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
            var watchD = System.Diagnostics.Stopwatch.StartNew();
            foreach (Thread x in threads) x.Start();
            foreach (Thread x in threads) x.Join();
            watchD.Stop();

            for (int i = 0; i < threads_number; i++) { tabMulti[i].c = e; }

            ParallelOptions opt = new ParallelOptions() {MaxDegreeOfParallelism = threads_number };
            int[] threadUesed = new int[Environment.ProcessorCount];

            var watchE = System.Diagnostics.Stopwatch.StartNew();
            Parallel.ForEach(tabMulti, opt, x => { x.Multiplication(); threadUesed[Thread.CurrentThread.ManagedThreadId]++; }) ;
            watchE.Stop();

            //Console.WriteLine(string.Join(" ", threadUesed));
            if (print)
            {
                Console.WriteLine("Macierz A:");
                Console.WriteLine(a);

                Console.WriteLine("Macierz B:");
                Console.WriteLine(b);

                Console.WriteLine("Macierz C:");
                Console.WriteLine(c);

                Console.WriteLine("Macierz D - Threads:");
                Console.WriteLine(d);

                Console.WriteLine("Macierz E - Parallel:");
                Console.WriteLine(e);
            }
            Console.WriteLine($"Czas wykonania mnożenia macierzy o rozmiarze {n} wyniósł:\n\t -sekwencyjnie: \t{watchC.Elapsed.TotalSeconds} s\n\t -wątkowo Threads: \t{watchD.Elapsed.TotalSeconds} s\n\t -wątkowo Parallel: \t{watchE.Elapsed.TotalSeconds} s");
          
            
            string path = @"..\\..\\..\\Obliczanie_predkosci.txt";
            StreamWriter sw;

            if (!File.Exists(path))
            {
                sw = File.CreateText(path);
            }
            else
            {
                sw = new StreamWriter(path, true);
            }
            sw.WriteLine($"{n} {threads_number} {watchC.Elapsed.TotalSeconds} {watchD.Elapsed.TotalSeconds} {watchE.Elapsed.TotalSeconds}");
            sw.Close();
        }

        

    }
}
