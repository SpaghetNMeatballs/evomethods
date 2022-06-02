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
            // 0 - 
            private double[] otherThresh = { 0.1, 0.5, 0.6, 0.8 };
            // 0 - нет операций, 1 - мутация, 2 - инверсия, 3 - транслокация
            // Количество особей
            public int popSize;
            // 
            private Dictionary<string, double>[] valueLibrary;
            private List<string>[] keyLibrary;
            private Func<double[], double> fitness;
            private int chromocount;
            // Длина одной переменной в коде Грея
            public int fragmentSize;
            public Population(double[][] inp0, int popSize, Func<double[], double> fitness)
            {
                this.fitness = fitness;
                this.popSize = popSize;
                chromocount = inp0.Length;
                var rand = new Random();
                valueLibrary = new Dictionary<string, double>[inp0.Length];
                keyLibrary = new List<string>[inp0.Length];
                for (int i = 0; i < inp0.Length; i++)
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
                    for (int j = 0; j < inp0.Length; j++)
                    {
                        int key = rand.Next(keyLibrary[j].Count);
                        temp += keyLibrary[j][key];
                    }
                    curPop[i] = new Gamet(temp);
                }
            }

            public void fitnessTest()
            {
                double[] temp = new double[chromocount];
                for (int i = 0; i < popSize; i++)
                {
                    string tempString = curPop[i].rbcvalue;
                    tempString = RBCtoBinary(tempString);
                    for (int j = 0; j < chromocount; j++)
                    {
                        temp[j] = valueLibrary[j][tempString.Substring(j * fragmentSize, fragmentSize)];
                    }
                    curPop[i].fitness = fitness(temp);
                }
                Array.Sort(curPop);
            }

            public double[] getValuesFromRbc(string rbcinp)
            {
                double[] temp = new double[chromocount];
                rbcinp = RBCtoBinary(rbcinp);
                for (int j = 0; j < chromocount; j++)
                {
                    temp[j] = valueLibrary[j][rbcinp.Substring(j * fragmentSize, fragmentSize)];
                }
                return temp;
            }

            public double RbcFitnessTest(string rbcinp, Func<double[], double> fitness)
            {
                double[] temp = new double[chromocount];
                rbcinp = RBCtoBinary(rbcinp);
                for (int j = 0; j < chromocount; j++)
                {
                    temp[j] = valueLibrary[j][rbcinp.Substring(j * fragmentSize, fragmentSize)];
                }
                return fitness(temp);
            }

            public double[] getGametValue(int index)
            {
                double[] output = new double[chromocount];
                string tempString = curPop[index].rbcvalue;
                tempString = RBCtoBinary(tempString);
                for (int j = 0; j < chromocount; j++)
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

            private bool ElitismCheck(string[] parents, string child)
            {
                int[] diff = new int[2];
                for (int i = 0; i < child.Length; i++)
                {
                    if (parents[0][i] != child[i])
                    {
                        diff[0]++;
                    }
                    if (parents[1][i] != child[i])
                    {
                        diff[1]++;
                    }
                }
                if (diff[0] > 2 && diff[1] > 2) return true;
                return false;
            }

            public void EraFull()
            {
                int[] indCross = GenerateCrossoverPool(popSize);
                int[] indExchange = GenerateExchangePool(popSize);
                Gamet[] temp = new Gamet[popSize];
                Array.Copy(curPop, temp, popSize);
                for (int i = 0; i < popSize / 2; i++)
                {
                    bool iterPass = true;
                    int crossInd, otherInd;
                    string child, postCrossChild = null;
                    while (iterPass)
                    {
                        crossInd = getOperInd(crossoverThresh, rand.NextDouble());
                        otherInd = getOperInd(otherThresh, rand.NextDouble());
                        string[] inp = { curPop[indCross[2 * i]].rbcvalue, curPop[indCross[2 * i + 1]].rbcvalue };
                        child = PerformCrossover(inp, crossInd);
                        postCrossChild = PerformOtherOperators(child, otherInd);
                        if (ElitismCheck(inp, postCrossChild))
                        {
                            iterPass = false;
                        }
                    }

                    temp[indExchange[i]].rbcvalue = postCrossChild;
                }
                if (curPop[0].rbcvalue == curPop[popSize - 1].rbcvalue)
                {
                    Console.WriteLine("Trolling");
                }
            }

            public void EraSegment()
            {
                int[] indCross = GenerateCrossoverPool(popSize);
                int[] indExchange = GenerateExchangePool(popSize);
                Gamet[] temp = new Gamet[popSize];
                Array.Copy(curPop, temp, popSize);
                for (int i = 0; i < popSize / 2; i++)
                {
                    bool iterPass = true;
                    int crossInd, otherInd;
                    string child, postCrossChild = null;
                    while (iterPass)
                    {
                        crossInd = getOperInd(crossoverThresh, rand.NextDouble());
                        otherInd = getOperInd(otherThresh, rand.NextDouble());
                        string[] inp = { curPop[indCross[2 * i]].rbcvalue, curPop[indCross[2 * i + 1]].rbcvalue };
                        int segment = rand.Next(chromocount);
                        child = PerformCrossoverSegment(inp, crossInd, segment);
                        postCrossChild = PerformOtherOperatorsSegment(child, otherInd, segment);
                        if (ElitismCheck(inp, postCrossChild))
                        {
                            iterPass = false;
                        }
                    }
                    temp[indExchange[i]].rbcvalue = postCrossChild;
                }
                if (curPop[0].rbcvalue == curPop[popSize - 1].rbcvalue)
                {
                    Console.WriteLine("Trolling");
                }
            }

            private int getOperInd(double[] thresh, double check)
            {
                int operInd = 0;
                for (int j = -1; j < crossoverThresh.Length-1; j++)
                {
                    if (check < crossoverThresh[j+1])
                    {
                        return j;
                        break;
                    }
                }
                return crossoverThresh.Length-1;
            }

            private string PerformCrossover(string[] rbcinp, int ind)
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
                if (RbcFitnessTest(children[0], fitness) < RbcFitnessTest(children[1], fitness))
                {
                    return children[0];
                }
                return children[1];
            }

            private string PerformOtherOperators(string rbcinp, int ind)
            {
                switch (ind)
                {
                    case 0:
                        return rbcinp;
                    case 1:
                        return Mutation(rbcinp, rand.Next(fragmentSize * chromocount));
                    case 2:
                        return Inversion(rbcinp, rand.Next(fragmentSize * chromocount));
                    case 3:
                        return Translocate(rbcinp, GetMultiIndex(fragmentSize * chromocount));
                }
                return "";
            }

            private string PerformCrossoverSegment(string[] rbcinp, int ind, int segment)
            {
                string[] children = new string[2];
                string child;
                switch (ind)
                {
                    case 0:
                        children = Crossover(new string[] { GetSegment(rbcinp[0], segment), GetSegment(rbcinp[1], segment) }, rand.Next(fragmentSize));
                        break;
                    case 1:
                        children = Crossover(new string[] { GetSegment(rbcinp[0], segment), GetSegment(rbcinp[1], segment) }, GetMultiIndex(fragmentSize));
                        break;
                    case 2:
                        child = Crossover(new string[] { GetSegment(rbcinp[0], segment), GetSegment(rbcinp[1], segment) }, 0.5);
                        if (RbcFitnessTest(SetSegment(rbcinp[0], child, segment), fitness) < RbcFitnessTest(SetSegment(rbcinp[1], child, segment), fitness))
                        {
                            return SetSegment(rbcinp[0], child, segment);
                        }
                        return SetSegment(rbcinp[1], child, segment);
                }
                children[0] = SetSegment(rbcinp[0], children[0], segment);
                children[1] = SetSegment(rbcinp[1], children[1], segment);
                if (RbcFitnessTest(children[0], fitness) < RbcFitnessTest(children[1], fitness))
                {
                    return rbcinp[0];
                }
                return rbcinp[1];
            }

            private string PerformOtherOperatorsSegment(string rbcinp, int ind, int segment)
            {
                switch (ind)
                {
                    case 0:
                        return rbcinp;
                    case 1:
                        return Mutation(rbcinp, chromocount * segment+rand.Next(fragmentSize));
                    case 2:
                        return SetSegment(rbcinp, Inversion(GetSegment(rbcinp, segment), rand.Next(fragmentSize-1)), segment);
                    case 3:
                        return Translocate(rbcinp, getIndex(fragmentSize, 7));
                }
                return "";
            }
        }
    }
}
