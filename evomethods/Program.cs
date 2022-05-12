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
                return -fitness.CompareTo(other.fitness);
            }
        }

        protected class Population
        {
            public Gamet[] curPop;
            // Количество особей
            public int popSize;
            private Dictionary<string, double>[] valueLibrary;
            private List<string>[] keyLibrary;
            // Длина одной переменной в коде Грея
            public int fragmentSize;
            public Population(double[][] inp0, int popSize)
            {
                this.popSize = popSize;
                var rand = new Random();
                valueLibrary = new Dictionary<string, double>[5];
                keyLibrary = new List<string>[5];
                for (int i = 0; i < 5; i++)
                {
                    Dictionary<string, double> cur = toPopulation(inp0[i][0], inp0[i][1], inp0[i][2]);
                    keyLibrary[i] = new List<string>(cur.Keys);
                    valueLibrary[i] = cur;
                }
                fragmentSize = keyLibrary[0][0].Length;
                curPop = new Gamet[popSize];
                for (int i = 0; i < popSize; i++)
                {
                    string temp = "";
                    for (int j = 0; j < 5; j++)
                    {
                        int key = rand.Next(keyLibrary[j].Count);
                        temp += keyLibrary[j][key];
                    }
                    curPop[i] = new Gamet(temp);
                }
            }

            public void fitnessTest(Func<double[], double> fitness)
            {
                double[] temp = new double[5];
                double[] output = new double[popSize];
                for (int i = 0; i < popSize; i++)
                {
                    string tempString = curPop[i].rbcvalue;
                    tempString = RBCtoBinary(tempString);
                    for (int j = 0; j < 5; j++)
                    {
                        temp[j] = valueLibrary[j][tempString.Substring(j * fragmentSize, fragmentSize)];
                    }
                    output[i] = fitness(temp);
                    curPop[i].fitness = output[i];
                }
                Array.Sort(curPop);
            }

            public double[] getGametValue(int index)
            {
                double[] output = new double[5];
                string tempString = curPop[index].rbcvalue;
                tempString = RBCtoBinary(tempString);
                for (int j = 0; j < 5; j++)
                {
                    output[j] = valueLibrary[j][tempString.Substring(j * fragmentSize, fragmentSize)];
                }
                return output;
            }

            public void outputPopulation()
            {
                foreach (Gamet cur in curPop)
                {
                    cur.printValue();
                }
            }

            public void outputFitness()
            {
                foreach(Gamet cur in curPop)
                {
                    Console.Write(String.Format("{0:0.00}", cur.fitness) + '\t');
                }
                Console.WriteLine();
            }

            public double[] outputDif(int index)
            {
                double[] dif = { 7, -3, -12, -1, -6};
                double[] output = new double[5];
                string tempString = curPop[index].rbcvalue;
                tempString = RBCtoBinary(tempString);
                for (int j = 0; j < 5; j++)
                {
                    output[j] = Math.Abs(valueLibrary[j][tempString.Substring(j * fragmentSize, fragmentSize)]+dif[j]);
                }
                return output;
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

        static void Main(string[] args)
        {
            double[] temp = TestCrossover(100000);
            double temp1 = temp[0];
            double temp2 = temp[1];
            Console.WriteLine("Улучшения на всей гамете " + temp1.ToString());
            Console.WriteLine("Улучшения на одной переменной " + temp2.ToString());
        }
    }
}
