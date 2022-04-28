using System;
using System.Collections.Generic;

namespace evomethods
{
    class Program
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

        public class Operator
        {
            public double prob;
            bool mode;
            public Func<string[], string[]> operationt;
            public Func<string, string> operationf;
            public Operator(Func<string[], string[]> operation, double prob)
            {
                mode = true;
                this.operationt = operation;
                this.prob = prob;
            }
            public Operator(Func<string, string> operation, double prob)
            {
                mode = false;
                this.operationf = operation;
                this.prob = prob;
            }
        }

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

        public static string Mutation(string rbcvalue, int position)
        {
            string temp = "";
            temp += rbcvalue.Substring(0, position);
            temp += rbcvalue[position] == '0' ? '1' : '0';
            temp += rbcvalue.Substring(position + 1);
            return temp;
        }

        public static string Inversion(string rbcvalue, int position)
        {
            string temp = "";
            temp += rbcvalue.Substring(position);
            temp += rbcvalue.Substring(0, position);
            return temp;
        }

        public static string[] Crossover(string[] rbcvalues, int position)
        {
            string rbcvalue1 = rbcvalues[0], rbcvalue2 = rbcvalues[1];
            string temp1 = rbcvalue1.Substring(0, position);
            string temp2 = rbcvalue2.Substring(0, position);
            temp1 += rbcvalue1.Substring(position);
            temp2 += rbcvalue2.Substring(position);
            rbcvalue1 = temp1;
            rbcvalue2 = temp2;
            return new string[]{ rbcvalue1, rbcvalue2 };
        }

        public static string Translocate(string rbcvalue, int[] index)
        {

            string output = rbcvalue.Substring(0, index[0]);
            string toShake = rbcvalue.Substring(index[0], index[1] - index[0]);
            string tail = rbcvalue.Substring(index[1]);
            toShake = Shuffle(toShake);
            output = output + toShake + tail;
            return output;
        }

        public static string Shuffle(string str)
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
            string output = new string(array);
            return new string(array);
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

        static void Main(string[] args)
        {
            Random rand = new Random();
            int startcount = 40;
            double eps = 0.01;
            double[] pos1 = { -10, -4, eps };
            double[] pos2 = { 0, 6, eps };
            double[] pos3 = { 9, 15, eps };
            double[] pos4 = { -2, 4, eps };
            double[] pos5 = { 3, 9, eps };
            double[][] inp0 = { pos1, pos2, pos3, pos4, pos5 };
            int segcount = 0;
            int fullcount = 0;
            for (int i = 0; i < 100000; i++)
            {
                if (i%1000 == 0)
                {
                    Console.WriteLine("Тест №" + i.ToString());
                }
                Population testFull = new Population(inp0, 1);
                Population testSeg = new Population(inp0, 1);
                testFull.fitnessTest(fitness);
                double fit1 = testFull.curPop[0].fitness;
                int segNumb = rand.Next(1, 5);
                int indFull = rand.Next(testFull.fragmentSize * 5);
                string rbcInp = testFull.curPop[0].rbcvalue;
                string toInverse = rbcInp.Substring(segNumb, testFull.fragmentSize);
                string inversed = Inversion(toInverse, rand.Next(testFull.fragmentSize));
                string checkFull = Inversion(rbcInp, indFull);
                string checkSeg = rbcInp.Substring(0, segNumb) +inversed+rbcInp.Substring(segNumb + testFull.fragmentSize);
                testFull.curPop[0].rbcvalue = checkFull;
                testSeg.curPop[0].rbcvalue = checkSeg;
                testFull.fitnessTest(fitness);
                testSeg.fitnessTest(fitness);
                if (testFull.curPop[0].fitness < fit1) 
                {
                    fullcount++;
                }
                if (testSeg.curPop[0].fitness < fit1) 
                {
                    segcount++;
                }
            }
            Console.WriteLine("Процент улучшений при транслокации на всей гамете: " + ((double)(fullcount / 1000)).ToString());
            Console.WriteLine("Процент улучшений при транслокации на сегменте: " + ((double)(segcount / 1000)).ToString());

            /*int startcount = 40;
            double eps = 0.01;
            double[] pos1 = { -10, -4, eps };
            double[] pos2 = { 0, 6, eps };
            double[] pos3 = { 9, 15, eps };
            double[] pos4 = { -2, 4, eps };
            double[] pos5 = { 3, 9, eps };
            double[][] inp0 = { pos1, pos2, pos3, pos4, pos5 };
            Operator op1 = new Operator(Mutation, 0.6);
            Operator op2 = new Operator(Inversion, 0.2);
            Operator op3 = new Operator(Crossover, 0.2);
            Operator[] ops = { op1, op2, op3 };


            Population test = new Population(inp0, 10);
            test.fitnessTest(fitness);


            double bestFitness = test.curPop[test.popSize - 1].fitness;
            int count = startcount;
            while (test.curPop[test.popSize-1].fitness > eps && count > 0)
            {
                count--;
                ApplyOperatorsToPop(test, ops, fitness);
                test.outputFitness();
                if (bestFitness > test.curPop[test.popSize - 1].fitness)
                {
                    bestFitness = test.curPop[test.popSize - 1].fitness;
                    count = startcount;
                }
            }
            double[] fin = test.getGametValue(test.popSize - 1);
            foreach (double cur in fin)
            {
                Console.Write(String.Format("{0:0.00}", cur) + '\t');
            }
            Console.Write("\tНайденные значения");
            Console.WriteLine();
            fin = test.outputDif(test.popSize - 1);
            foreach (double cur in fin)
            {
                Console.Write(String.Format("{0:0.00}", cur) + '\t');
            }
            Console.Write("\tОтклонения от истинных значений");*/
        }
    }
}
