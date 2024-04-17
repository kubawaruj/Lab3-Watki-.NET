namespace ConsoleApp_Matrix
{
    internal class Program
    {
        public static volatile Matrix e; //wynikowa mnożenie Parallel
        public static volatile bool isFirst; //wynikowa mnożenie Parallel

        static void Main(string[] args)
        {
            //Functionality(0, 0, false, false);
            Tests();
        }

        static void Tests()
        {
            int[] n = { 1, 2, 5, 10, 20, 50, 100, 200 };
            int[] threads = { 1, 2, 4, 8, 16 };
            Console.WriteLine("Trwają testy...");
            foreach (int x in n)
            {
                foreach (int y in threads)
                {
                    Functionality(x, y, false, true);
                }
                Console.WriteLine("Wykonano dla macierzy o rozmiarze {0}", x);
            }
            Console.WriteLine("Zapisano dane do pliku");
        }

        static void Functionality(int n, int threads_number, bool print, bool isTest)
        {
            Tuple<double, double, double>[] times = new Tuple<double, double, double>[10];
            isFirst = true;
            if (!isTest)//Jednokrotne wywołanie programu przez użytkownika
            {
                Console.WriteLine("Wprowadź rozmiar macierzy: ");
                n = int.Parse(Console.ReadLine());

                Console.WriteLine($"\nTwój komputer posiada {Environment.ProcessorCount} rdzeni.");
                Console.WriteLine("Wprowadź liczbę wątków: ");
                threads_number = int.Parse(Console.ReadLine());

                Console.WriteLine("Czy wyświetlać macierze? true/false: ");
                print = bool.Parse(Console.ReadLine());
                times[0] = Math(n, threads_number, print);
                Console.WriteLine($"Czas wykonania mnożenia macierzy o rozmiarze {n} i liczbie wątków {threads_number} wyniósł:\n\t -sekwencyjnie: \t{times[0].Item1.ToString("0.00000000")} s\n\t -wątkowo Threads: \t{times[0].Item2.ToString("0.00000000")} s\n\t -wątkowo Parallel: \t{times[0].Item3.ToString("0.00000000")} s");
            }
            else //Wywołanie 10 razy, aby zliczyć średnią wartość czasów
            {
                foreach (int i in Enumerable.Range(0, 10))
                {
                    times[i] = Math(n, threads_number, print);
                    if (isFirst) //Pierwszy raz liczy sekwencyjnie
                    {
                        isFirst = false;
                    }
                    else //Następne razy dla sekwencyjnego mnożenia, przepisuje wartość czasu pierwszego wywołania - optymalizacja czasowa dla dużego N, ale małej ilości wątków
                    {
                        times[i] = times[0];
                    }

                }

                //Zapisywanie do pliku dla testów wartości średniej czasów
                string path = @"..\\..\\..\\Obliczanie_predkosci_srednia2.txt";
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
        }


        static Tuple<double, double, double> Math(int n, int threads_number, bool print)
        {
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
                c = a * b; //mnożenie sekwencyjne
                watchC.Stop();
            }
            else
            {
                watchC.Stop();
            }



            //Obliczenie ile pól w macierzy wynikowej będzie liczone przez dany wątek
            int[] threads_fields = new int[threads_number];
            for (int i = 0; i < threads_number; i++)
            {
                threads_fields[i] = n * n / threads_number;
            }
            if (((n * n) % threads_number) > 0) //Jeżeli liczba pól nie jest całkowita to do kolejnych dodaj po jednym polu z reszty nieprzydzielonych pól
            {
                for (int i = 0; i < ((n * n) % threads_number); i++)
                {
                    threads_fields[i]++;
                }
            }

            //Tworzenie tablicy przekazującej dane do wątków
            int tmp = 0;
            Multi[] tabMulti = new Multi[threads_number];
            for (int i = 0; i < threads_number; i++)
            {
                tabMulti[i] = new Multi(threads_number, a, b, d, tmp, threads_fields[i]);
                tmp += threads_fields[i];
                tabMulti[i].c = d;
            }


            /*----------------Threads---------------*/
            //Tworzenie i uruchomienie wątków Threads
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



            /*----------------Parallel---------------*/
            //Ustawienie macierzy e jako macierzy wynikowej dla tablicy zmiennych przekazywanej do wątków - Parallel
            for (int i = 0; i < threads_number; i++) { tabMulti[i].c = e; }
            ParallelOptions opt = new ParallelOptions() { MaxDegreeOfParallelism = threads_number };

            //Uruchomienie wątków Parallel
            var watchE = System.Diagnostics.Stopwatch.StartNew();
            Parallel.ForEach(tabMulti, opt, x => { x.Multiplication(); });
            watchE.Stop();

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

            return new Tuple<double, double, double>(watchC.Elapsed.TotalSeconds, watchD.Elapsed.TotalSeconds, watchE.Elapsed.TotalSeconds);
        }
    }
}
