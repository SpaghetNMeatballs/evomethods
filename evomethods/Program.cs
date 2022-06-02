using System;
using System.Collections.Generic;

namespace evomethods
{
    partial class Program
    {

        protected class Gamet : IComparable<Gamet>
        {
            public string rbcvalue;
            public double fitness;
            public Gamet(string bininp)
            {
                rbcvalue = binaryToRBC(bininp);
                fitness = double.MaxValue / 2;
            }

            public void printValue()
            {
                Console.WriteLine(rbcvalue);
            }

            public int CompareTo(Gamet other)
            {
                return fitness.CompareTo(other.fitness);
            }
        }

        public static bool checkBinary(string inp)
        {
            foreach (char symbol in inp)
            {
                if (!symbol.Equals('1') && !symbol.Equals('0'))
                {
                    return false;
                }
            }
            return true;
        }

        public static string binaryToRBC(string inp)
        {
            if (!checkBinary(inp))
            {
                throw new ArgumentException("String is not binary");
            }
            string output = "";
            inp = '0' + inp;
            for (int i = 0; i < inp.Length-1; i++)
            {
                output += inp[i] != inp[i+1] ? '1' : '0';
            }
            return output;
        }

        public static string RBCtoBinary(string inp)
        {
            if (!checkBinary(inp))
            {
                throw new ArgumentException("String is not binary");
            }
            string output = "";
            inp = '0' + inp;
            for (int i = 1; i < inp.Length; i++)
            {
                char temp = inp[0] == '1'? '1' : '0';
                for (int j = 1; j < i+1; j++)
                {
                    temp = inp[j] != temp ? '1' : '0';
                }
                output += temp;
            }
            return output;
        }

        protected static Dictionary<string, double> toPopulation(double xMin, double xMax, double epsilon)
        {
            double nb = 1 + (xMax - xMin) / epsilon;
            double n2 = Math.Ceiling(Math.Log2(nb));
            double newEpsilon = (xMax - xMin) / (Math.Pow(2, n2) - 1);
            int size = Convert.ToInt32(Math.Pow(2, n2));
            Dictionary<string, double> output = new Dictionary<string, double>();
            for (int i = 0; i<size; i++)
            {
                string curstring = Convert.ToString(i, 2);
                while (curstring.Length < n2)
                {
                    curstring = '0' + curstring;
                }
                output[curstring] = xMin + newEpsilon * i;
            }
            return output;
        }

        public static void DoublePrint(double[] inp)
        {
            for (int i = 0; i < inp.Length; i++)
            {
                Console.WriteLine("x"+(i+1).ToString()+" = " + String.Format("{0:0.000}", inp[i]));
            }
        }

        private static void PerformFull(Population test)
        {
            for (int i = 0; i < 100; i++)
            {
                //Console.WriteLine("Итерация №" + i.ToString());
                double cur = test.curPop[0].fitness;
                test.EraFull();
                test.fitnessTest();
                double output = test.curPop[0].fitness;
                Console.Write(output.ToString() + ' ');
            }
            //Console.WriteLine("Лучшее значение за весь процесс" + best.ToString());
            //DoublePrint(test.getValuesFromRbc(bestrbc));
            //Console.WriteLine("Текущее лучшее " + test.curPop[0].fitness.ToString());
            double[] toprint = new double[100];
            for (int i = 0; i < 100; i++)
            {
                toprint[i] = test.curPop[i].fitness;
            }
            Console.WriteLine('\n');
            DoublePrint(test.getGametValue(0));
            //DoublePrint(toprint);
        }
        private static void PerformSegment(Population test)
        {
            for (int i = 0; i < 100; i++)
            {
                //Console.WriteLine("Итерация №" + i.ToString());
                double cur = test.curPop[0].fitness;
                test.EraSegment();
                test.fitnessTest();
                double output = test.curPop[0].fitness;
                Console.Write(output.ToString() + ' ');
            }
            //Console.WriteLine("Текущее лучшее " + test.curPop[0].fitness.ToString());
            double[] toprint = new double[100];
            for (int i = 0; i < 100; i++)
            {
                toprint[i] = test.curPop[i].fitness;
            }
            Console.WriteLine('\n');
            DoublePrint(test.getGametValue(0));
            //DoublePrint(toprint);
        }

        static void Main(string[] args)
        {
            double[] p0 = { -5, 5, 0.01 };
            double[][] p = { p0, p0, p0, p0 };
            Population test = new Population(p, 10000, Rastrigin);
            test.fitnessTest();
            PerformFull(test);
        }
    }
}
