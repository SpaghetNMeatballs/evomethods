using System;
using System.Collections.Generic;

namespace evomethods
{
    class Program
    {

        public static double fitness(double[] inp)
        {
            return Math.Pow(inp[0] + 7, 2) + Math.Pow(inp[1] - 3, 2) + Math.Pow(inp[2] - 12, 2) + Math.Pow(inp[3] - 1, 2) + Math.Pow(-inp[4] + 6, 2);
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

        protected class Gamet
        {
            public string rbcvalue;
            public Gamet(string bininp)
            {
                this.rbcvalue = binaryToRBC(bininp);
            }

            public void printValue()
            {
                Console.WriteLine(rbcvalue);
            }

            // Однопозиционная мутация
            public void Mutation(int position)
            {
                string temp = "";
                temp += rbcvalue.Substring(0, position);
                temp += rbcvalue[position] == '0' ? '1' : '0';
                temp += rbcvalue.Substring(position + 1);
                rbcvalue = temp;
            }

            // Многопозиционная мутация
            public void Mutation(int[] position)
            {
                string temp = "";
                int posAmount = position.Length;
                temp += rbcvalue.Substring(0, position[0]);
                for (int i = 0; i < posAmount - 1; i++)
                {
                    temp += rbcvalue[position[i]] == '0' ? '1' : '0';
                    temp += rbcvalue.Substring(position[i] + 1, position[i + 1]-1);
                }
                temp += rbcvalue[position[posAmount-1]] == '0' ? '1' : '0';
                temp += rbcvalue.Substring(position[posAmount-1] + 1);
                rbcvalue = temp;
            }

            // Инверсия по заданной позиции
            public void Inversion(int position)
            {
                string temp = "";
                temp += rbcvalue.Substring(position);
                temp += rbcvalue.Substring(0, position);
                rbcvalue = temp;
            }

            // Кроссовер
            public void Crossover(Gamet secondParent, int position)
            {
                string temp1 = rbcvalue.Substring(0, position);
                string temp2 = secondParent.rbcvalue.Substring(0, position);
                temp1 += secondParent.rbcvalue.Substring(position);
                temp2 += rbcvalue.Substring(position);
                rbcvalue = temp1;
                secondParent.rbcvalue = temp2;
            }

            // Многопозиционный кроссовер
            public void Crossover(Gamet secondParent, int[] position)
            {
                string rbcvalue1 = rbcvalue;
                string rbcvalue2 = secondParent.rbcvalue;
                string temp1 = rbcvalue1.Substring(0, position[0]-1);
                string temp2 = rbcvalue2.Substring(0, position[0]-1);
                string temp = rbcvalue1;
                rbcvalue1 = rbcvalue2;
                rbcvalue2 = temp;
                for (int i = 0; i < position.Length-1; i++)
                {
                    temp1 += rbcvalue1.Substring(position[i], position[i + 1] - position[i]);
                    temp2 += rbcvalue2.Substring(position[i], position[i + 1] - position[i]);
                    temp = rbcvalue1;
                    rbcvalue1 = rbcvalue2;
                    rbcvalue2 = temp;
                }
                temp1 += rbcvalue1.Substring(position[position.Length - 1] - 1);
                temp2 += rbcvalue2.Substring(position[position.Length - 1] - 1);
                rbcvalue = temp1;
                secondParent.rbcvalue = temp2;
            }
        }

        protected class Population
        {
            private Gamet[] curPop;
            private int popSize;
            private Dictionary<string, double>[] valueLibrary;
            private List<string>[] keyLibrary;
            private int fragmentSize;
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
                        int key = rand.Next(keyLibrary[i].Count);
                        temp += keyLibrary[i][key];
                    }
                    curPop[i] = new Gamet(temp);
                }
            }

            public double[] fitnessTest(Func<double[], double> fitness)
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
                    Console.WriteLine(string.Format("({0}+7)^2+({1}-3)^2+({2}-12)^2+({3}-1)^2+(-{4}+6)^2 = {5}", temp[0], temp[1], temp[2], temp[3], temp[4], output[i]));
                }

                return output;
            }

            public void outputPopulation()
            {
                foreach(Gamet cur in curPop)
                {
                    cur.printValue();
                }
            }
        }
        static void Main(string[] args)
        {
            double[] pos1 = { -10, -4, 0.5 };
            double[] pos2 = { 0, 6, 0.5 };
            double[] pos3 = { 9, 15, 0.5 };
            double[] pos4 = { -2, 4, 0.5 };
            double[] pos5 = { 3, 9, 0.5 };
            double[][] inp0 = { pos1, pos2, pos3, pos4, pos5 };
            Population test = new Population(inp0, 4);
            test.outputPopulation();
            double[] fit = test.fitnessTest(fitness);
            foreach(double val in fit)
            {
                Console.WriteLine(val);
            }
        }
    }
}
