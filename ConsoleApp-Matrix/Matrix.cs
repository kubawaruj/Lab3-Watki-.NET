using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConsoleApp_Matrix
{
    internal class Matrix
    {
        public int n {  get; set; } //rozmiar macierzy
        public int[,] matrix { get; set; } //macierz 
        public Matrix(int _n) {
            n = _n;
            matrix = new int[n, n];
        }

        public void RandomMatrix(){
            var rand = new Random();
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    matrix[i, j] = rand.Next(10);
                }
            }
        }

        public static Matrix operator *(Matrix a, Matrix b){
            Matrix c = new Matrix(a.n);

            for (int i = 0; i < a.n; i++)
            {
                for (int k = 0; k < a.n; k++)
                {
                    for (int j = 0; j < a.n; j++)
                    {
                        c.matrix[i, k] += a.matrix[i, j] * b.matrix[j, k];
                    }
                }
            }
            return c;
        }

        public override string ToString(){
            string s = "";

            for (int i = 0; i < n; i++){
                for (int j = 0; j < n; j++){
                    s += $"{matrix[i, j]} ";
                }
                s += "\n";
            }
            return s;
        }



    }
}
