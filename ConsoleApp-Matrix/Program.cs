namespace ConsoleApp_Matrix
{
    internal class Program
    {
        static void Main(string[] args)
        {

            int n = 0, threads_number = 0;

            Console.WriteLine("Wprowadź rozmiar macierzy: ");
            n = int.Parse(Console.ReadLine());

            Console.WriteLine($"Twój komputer posiada {Environment.ProcessorCount} rdzeni.");
            Console.WriteLine("Wprowadź liczbę wątków: ");
            threads_number = int.Parse(Console.ReadLine());

            Matrix a = new Matrix(n);
            Matrix b = new Matrix(n);
            Matrix c = new Matrix(n);

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
                tabMulti[i] = new Multi(threads_number, a, b, c, tmp, threads_fields[i]);
                tmp += threads_fields[i];
                tabMulti[i].c = c;
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




            Console.WriteLine("Macierz A:");
            Console.WriteLine(a);

            Console.WriteLine("Macierz B:");
            Console.WriteLine(b);

            Console.WriteLine("Macierz C - Threads:");
            Console.WriteLine(c);

             DateTime startTime = DateTime.Now;
             c = a * b;
             DateTime stopTime = DateTime.Now;
             TimeSpan totalTime = stopTime - startTime;
            
            Console.WriteLine("Macierz A*B:");
            Console.WriteLine(c);

            Console.WriteLine($"Czas wykonania mnożenia macierzy o rozmiarze {n} wyniósł: {totalTime.TotalSeconds} s - Threads: {totalTime1.TotalSeconds} s");
        }



    }
}
