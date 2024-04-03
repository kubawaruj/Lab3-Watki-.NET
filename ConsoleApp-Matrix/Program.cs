namespace ConsoleApp_Matrix
{
    internal class Program
    {
        static void Main(string[] args)
        {

            int n = 0, threads_number = 0;

            Console.WriteLine("Wprowadź rozmiar macierzy: ");
            n = int.Parse(Console.ReadLine());

            Console.WriteLine("Wprowadź liczbę wątków: ");
            threads_number = int.Parse(Console.ReadLine());


            Thread[] threads = new Thread[n];
            for (int i = 0; i < threads_number; i++)
            {
                threads[i] = new Thread(Welcome);
                threads[i].Name = String.Format(" Thread : {0}", i + 1);
            }
            foreach (Thread x in threads)
                x.Start();
            Console.WriteLine(" Main : Hello !");


            Matrix a = new Matrix(n);
            Matrix b = new Matrix(n);
            Matrix c = new Matrix(n);

            a.RandomMatrix();
            b.RandomMatrix();

            Console.WriteLine("Macierz A:");
            Console.WriteLine(a);

            Console.WriteLine("Macierz B:");
            Console.WriteLine(b);

            DateTime startTime = DateTime.Now;
            c = a * b;
            DateTime stopTime = DateTime.Now;
            TimeSpan totalTime = stopTime - startTime;

            Console.WriteLine("Macierz A*B:");
            Console.WriteLine(c);

            Console.WriteLine($"Czas wykonania mnożenia macierzy o rozmiarze {n} wyniósł: {totalTime.TotalSeconds} s");
        }
    }
}
