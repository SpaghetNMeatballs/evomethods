using System;
using System.Collections.Generic;
using System.Text;

namespace evomethods
{
    partial class Program
    {
        private static readonly Random rand = new Random();

        public static int[] getIndex(int length, int eps)
        {
            int temp1 = rand.Next(length);
            int min = temp1 > eps ? temp1 - eps : 0;
            int max = temp1 < length-eps ? temp1 + eps : length;
            int temp2 = rand.Next(min, max);
            return temp1 > temp2 ? new int[] { temp2, temp1 } : new int[] { temp1, temp2 };
        }

        public static double RanaFitness(double[] x)
        {
            double output = 0;
            for (int i = 0; i < x.Length-1; i++)
            {
                double t1 = Math.Sqrt(Math.Abs(x[i + 1] + x[i] + 1));
                double t2 = Math.Sqrt(Math.Abs(x[i + 1] - x[i] + 1));
                output += (x[i + 1] + 1) * Math.Cos(t2) * Math.Sin(t1) + x[i] * Math.Cos(t1) * Math.Sin(t2);
            }
            return output;
        }

        public static double Rastrigin(double[] x)
        {
            double sum = 10 * x.Length;
            for (int i = 0; i < x.Length; i++)
            {
                sum += x[i] * x[i] - 10 * Math.Cos(2 * Math.PI * x[i]);
            }
            return sum;
        }

        public static double Rozenbrok(double[] x)
        {
            double sum = 0;
            for (int i = 0; i < x.Length-1; i++)
            {
                sum += 100 * Math.Pow(x[i + 1] - x[i] * x[i], 2) + Math.Pow(x[i] - 1, 2);
            }
            return sum;
        }

        public static double test(double[] x)
        {
            double output = 0;
            for (int i = 1; i < x.Length + 1; i++)
            {
                output += Math.Pow(x[i - 1] * x[i - 1] - i, 2);
            }
            return output;
        }

        private static string Shuffle(string str)
        {
            char[] array = str.ToCharArray();
            Random rng = new Random();
            int n = array.Length;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                var value = array[k];
                array[k] = array[n];
                array[n] = value;
            }
            return new string(array);
        }

        private static int[] Shuffle(int[] array)
        {
            int n = array.Length;
            while (n > 1)
            {
                n--;
                int k = rand.Next(n + 1);
                var value = array[k];
                array[k] = array[n];
                array[n] = value;
            }
            return array;
        }

        private static int[] GenerateCrossoverPool(int size)
        {
            int[] output = new int[size];
            for (int i = 0; i < size; i++)
            {
                output[i] = i;
            }
            return Shuffle(output);
        }

        private static int[] GenerateExchangePool(int size)
        {
            int[] output = new int[(int)(size * 0.75)];
            for (int i = 0; i < (int)(size * 0.75); i++)
            {
                output[i] = i + (int)(size * 0.25);
            }

            return new ArraySegment<int>(Shuffle(output), 0, size/2).ToArray();
        }

        private static string RandomBinary(int length)
        {
            string output = "";
            for (int i = 0; i < length; i++)
            {
                output += rand.Next() % 2 == 0 ? '1' : '0';
            }
            return output;
        }
        public static int[] Check(int[] ind)
        {
            if (ind[0] > ind[1])
            {
                ind[1] += ind[0];
                ind[0] = ind[1] - ind[0];
                ind[1] -= ind[0];
            }
            return ind;
        }

        static string GetSegment(string inp, int index)
        {
            int size = inp.Length / 5;
            return inp.Substring(size * index, inp.Length / 5);
        }

        static string SetSegment(string inp, string segment, int index)
        {
            int size = inp.Length / 5;
            return inp.Substring(0, size*index)+segment+inp.Substring(size*(index+1));
        }

        static int[] GetMultiIndex(int length)
        {
            int[] output = new int[rand.Next(2, 7)];
            int flag = 0;
            while (flag < output.Length)
            {
                int temp = rand.Next(1, length);
                bool flagHas = false;
                for (int i = 0; i < flag; i++)
                {
                    if (output[i] == temp)
                    {
                        flagHas = true;
                    }
                }
                if (flagHas)
                {
                    continue;
                }
                output[flag] = temp;
                flag++;
            }
            Array.Sort(output);
            return output;
        }

    }
}
