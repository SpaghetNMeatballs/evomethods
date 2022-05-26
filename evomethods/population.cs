using System;
using System.Collections.Generic;
using System.Text;

namespace evomethods
{
    partial class Program
    {
        protected class Population
        {
            public Gamet[] curPop;

            private double[] crossoverThresh = { 0, 0.5, 0.75 };
            // Количество особей
            public int popSize;
            // 
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
                for (int i = 0; i < popSize; i++)
                {
                    string tempString = curPop[i].rbcvalue;
                    tempString = RBCtoBinary(tempString);
                    for (int j = 0; j < 5; j++)
                    {
                        temp[j] = valueLibrary[j][tempString.Substring(j * fragmentSize, fragmentSize)];
                    }
                    curPop[i].fitness = fitness(temp);
                }
                Array.Sort(curPop);
            }

            public double RbcFitnessTest(string rbcinp, Func<double[], double> fitness)
            {
                double[] temp = new double[5];
                rbcinp = RBCtoBinary(rbcinp);
                for (int j = 0; j < 5; j++)
                {
                    temp[j] = valueLibrary[j][rbcinp.Substring(j * fragmentSize, fragmentSize)];
                }
                return fitness(temp);
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
                foreach (Gamet cur in curPop)
                {
                    Console.Write(String.Format("{0:0.00}", cur.fitness) + '\t');
                }
                Console.WriteLine();
            }

            public double[] outputDif(int index)
            {
                double[] dif = { 7, -3, -12, -1, -6 };
                double[] output = new double[5];
                string tempString = curPop[index].rbcvalue;
                tempString = RBCtoBinary(tempString);
                for (int j = 0; j < 5; j++)
                {
                    output[j] = Math.Abs(valueLibrary[j][tempString.Substring(j * fragmentSize, fragmentSize)] + dif[j]);
                }
                return output;
            }

            public void Era()
            {
                int[] indCross = GenerateCrossoverPool(popSize);
                Gamet[] temp = new Gamet[popSize];
                Array.Copy(curPop, temp, popSize);
                for (int i = 0; i < popSize/2; i++)
                {
                    double check = rand.NextDouble();
                    int crossInd = 0;
                    for (int j = 0; j < crossoverThresh.Length; j++)
                    {
                        if (check > crossoverThresh[j])
                        {
                            crossInd = j;
                            break;
                        }
                    }
                    string[] inp = { curPop[indCross[2 * i]].rbcvalue, curPop[indCross[2 * i + 1]].rbcvalue };
                    string child = PerformCrossover(inp, crossInd);
                }
            }

            string PerformCrossover(string[] rbcinp, int ind)
            {
                string[] children = new string[2];
                string child;
                switch (ind)
                {
                    case 0:
                        children = Crossover(rbcinp, rand.Next(rbcinp[0].Length)); 
                        break;
                    case 1:
                        children = Crossover(rbcinp, GetMultiIndex(rbcinp[0].Length));
                        break;
                    case 2:
                        child = Crossover(rbcinp, rand.NextDouble());
                        return child;
                }
                if (RbcFitnessTest(children[0], RanaFitness) < RbcFitnessTest(children[1], RanaFitness))
                {
                    return children[0];
                }
                return children[1];
            }
        }
    }
}
