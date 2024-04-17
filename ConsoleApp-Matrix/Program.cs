using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace ConsoleApp_Matrix
{
    internal class Program
    {
        public static volatile Matrix e; //wynikowa mnożenie Parallel
        public static volatile bool isFirst; //wynikowa mnożenie Parallel
        static void Main(string[] args)
        {
            //Tests();
            Functionality(0, 0, false, false);


        }

        static void Tests() {
            int[] n = { 1, 2, 5, 10, 20, 50, 100, 200, 500, 1000, 2000 };
            int[] threads = { 1, 2, 4, 8, 16 };
            Console.WriteLine("Trwają testy...");
            foreach (int x in n) {
                foreach (int y in threads)
                {
                    Functionality(x, y, false, true);
                }
                Console.WriteLine("Wykonano dla macierzy o rozmiarze {0}", x);
            }
            Console.WriteLine("Zapisano dane do pliku");
        }
        static void Functionality(int n, int threads_number, bool print, bool isTest) {
            isFirst = true;
            if (!isTest)
            {
                Console.WriteLine("Wprowadź rozmiar macierzy: ");
                n = int.Parse(Console.ReadLine());

                Console.WriteLine($"\nTwój komputer posiada {Environment.ProcessorCount} rdzeni.");
                Console.WriteLine("Wprowadź liczbę wątków: ");
                threads_number = int.Parse(Console.ReadLine());

                Console.WriteLine("Czy wyświetlać macierze? true/false: ");
                print = bool.Parse(Console.ReadLine());

            }
            Tuple<double, double, double>[] times = new Tuple<double, double, double>[10];
            foreach (int i in Enumerable.Range(0, 10))
            {
                times[i] = Math(n, threads_number, print);
                if (isFirst)
                {
                    isFirst = false;
                }
                else
                {
                    times[i] = times[0];
                }
                
            }

            string path = @"..\\..\\..\\Obliczanie_predkosci_srednia1.txt";
            StreamWriter sw;

            if (!File.Exists(path))
            {
                sw = File.CreateText(path);
            }
            else
            {
                sw = new StreamWriter(path, true);
            }
            double avTimeSeq = (from time in times select time.Item1).Average();
            double avTimeThr = (from time in times select time.Item2).Average();
            double avTimePar = (from time in times select time.Item3).Average();
            sw.WriteLine($"{n} {threads_number} {avTimeSeq.ToString("0.00000000")} {avTimeThr.ToString("0.00000000")} {avTimePar.ToString("0.00000000")}");
            sw.Close();
        }

        static Tuple<double, double, double> Math(int n, int threads_number, bool print) {
            Matrix a = new Matrix(n); //pierwotna
            Matrix b = new Matrix(n); //pierwotna
            Matrix c = new Matrix(n); //wynikowa bez mnożenia wątkowo
            Matrix d = new Matrix(n); //wynikowa mnożenie Threads
            e = new Matrix(n);

            a.RandomMatrix();
            b.RandomMatrix();

            var watchC = System.Diagnostics.Stopwatch.StartNew();
            if (isFirst)
            {
                c = a * b;
                watchC.Stop();
            }
            else {
                watchC.Stop();
            }

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

            ParallelOptions opt = new ParallelOptions() { MaxDegreeOfParallelism = threads_number };
            //int[] threadUesed = new int[Environment.ProcessorCount];
            //Parallel.ForEach(tabMulti, opt, x => { x.Multiplication(); threadUesed[Thread.CurrentThread.ManagedThreadId]++; });

            var watchE = System.Diagnostics.Stopwatch.StartNew();
            Parallel.ForEach(tabMulti, opt, x => { x.Multiplication(); });
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

                Console.WriteLine($"Czas wykonania mnożenia macierzy o rozmiarze {n} wyniósł:\n\t -sekwencyjnie: \t{watchC.Elapsed.TotalSeconds} s\n\t -wątkowo Threads: \t{watchD.Elapsed.TotalSeconds} s\n\t -wątkowo Parallel: \t{watchE.Elapsed.TotalSeconds} s");
            }

            return new Tuple<double, double, double>(watchC.Elapsed.TotalSeconds, watchD.Elapsed.TotalSeconds, watchE.Elapsed.TotalSeconds);

        }


    }
}
