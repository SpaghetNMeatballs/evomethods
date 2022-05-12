using System;

namespace evomethods
{
    partial class Program
    {
        public static double[] TestTranslocate(int size)
        {
            double[] interval = { -500, 500, 0.01 };
            int fullimprov = 0;
            int oneimprov = 0;
            double[][] inp0 = { interval, interval, interval, interval, interval };
            Population test = new Population(inp0, 1);
            Population testone = new Population(inp0, 1);
            for (int i = 0; i < size; i++)
            {
                if (i % (size / 10) == 0)
                {
                    Console.WriteLine("Тест транслокации №" + i.ToString());
                }
                string rbctemp = RandomBinary(test.fragmentSize * 5);
                test.curPop[0].rbcvalue = rbctemp;
                test.fitnessTest(RanaFitness);
                double start = test.curPop[0].fitness;
                int[] fullIndex = getIndex(test.fragmentSize * 5, 7);
                int[] oneIndex = getIndex(test.fragmentSize, 7);
                int segment = rand.Next(5);
                oneIndex[0] *= segment + 1;
                oneIndex[1] *= segment + 1;
                test.curPop[0].rbcvalue = Translocate(rbctemp, fullIndex);
                test.fitnessTest(RanaFitness);

                testone.curPop[0].rbcvalue = Translocate(rbctemp, oneIndex);
                testone.fitnessTest(RanaFitness);

                if (start > test.curPop[0].fitness)
                {
                    fullimprov++;
                }

                if (start > testone.curPop[0].fitness)
                {
                    oneimprov++;
                }
            }
            double temp1 = (double)fullimprov * 100 / size;
            double temp2 = (double)oneimprov * 100 / size;
            return new double[] { temp1, temp2 };
        }

        public static double[] TestInversion(int size)
        {
            double[] interval = { -500, 500, 0.01 };
            int fullimprov = 0;
            int oneimprov = 0;
            double[][] inp0 = { interval, interval, interval, interval, interval };
            Population test = new Population(inp0, 1);
            Population testone = new Population(inp0, 1);
            for (int i = 0; i < size; i++)
            {
                if (i % (size / 10) == 0)
                {
                    Console.WriteLine("Тест инверсии №" + i.ToString());
                }
                string rbctemp1 = RandomBinary(test.fragmentSize * 5);
                test.curPop[0].rbcvalue = rbctemp1;
                test.fitnessTest(RanaFitness);
                double startFit = test.curPop[0].fitness;
                int fullIndex = rand.Next(test.fragmentSize * 5);
                int oneIndex = rand.Next(test.fragmentSize);
                int segment = rand.Next(5);

                test.curPop[0].rbcvalue = Inversion(rbctemp1, fullIndex);
                test.fitnessTest(RanaFitness);

                testone.curPop[0].rbcvalue = SetSegment(rbctemp1, Inversion(GetSegment(rbctemp1, segment), oneIndex), segment);
                testone.fitnessTest(RanaFitness);

                if (startFit > test.curPop[0].fitness)
                {
                    fullimprov++;
                }

                if (startFit > testone.curPop[0].fitness)
                {
                    oneimprov++;
                }
            }
            double temp1 = (double)fullimprov * 100 / size;
            double temp2 = (double)oneimprov * 100 / size;
            return new double[] { temp1, temp2 };
        }

        public static double[] TestCrossover(int size)
        {
            double[] interval = { -500, 500, 0.01 };
            int fullimprov = 0;
            int oneimprov = 0;
            double[][] inp0 = { interval, interval, interval, interval, interval };
            Population test = new Population(inp0, 2);
            Population testone = new Population(inp0, 2);
            for (int i = 0; i < size; i++)
            {
                if (i % (size / 10) == 0)
                {
                    Console.WriteLine("Тест кроссовера №" + i.ToString());
                }
                string rbctemp1 = RandomBinary(test.fragmentSize * 5);
                string rbctemp2 = RandomBinary(test.fragmentSize * 5);
                test.curPop[0].rbcvalue = rbctemp1;
                test.curPop[1].rbcvalue = rbctemp2;
                test.fitnessTest(RanaFitness);
                int start = 0;
                if (test.curPop[start].fitness > test.curPop[Math.Abs(start - 1)].fitness)
                {
                    start = Math.Abs(start - 1);
                }
                double startFit = test.curPop[start].fitness;
                int fullIndex = rand.Next(test.fragmentSize * 5);
                int oneIndex = rand.Next(test.fragmentSize);
                int segment = rand.Next(5);
                string[] temp = Crossover(new string[] { rbctemp1, rbctemp2 }, fullIndex);
                string[] tempSmall = Crossover(new string[] { GetSegment(rbctemp1, segment), GetSegment(rbctemp2, segment) }, oneIndex);
                test.curPop[0].rbcvalue = temp[0];
                test.curPop[1].rbcvalue = temp[1];
                test.fitnessTest(RanaFitness);
                int end = 0;
                if (test.curPop[end].fitness > test.curPop[Math.Abs(end - 1)].fitness)
                {
                    end = Math.Abs(start - 1);
                }
                double endFit = test.curPop[end].fitness;

                testone.curPop[0].rbcvalue = SetSegment(rbctemp1, tempSmall[0], segment);
                testone.curPop[1].rbcvalue = SetSegment(rbctemp2, tempSmall[1], segment);
                testone.fitnessTest(RanaFitness);
                int endOne = 0;
                if (testone.curPop[endOne].fitness > testone.curPop[Math.Abs(endOne - 1)].fitness)
                {
                    endOne = Math.Abs(start - 1);
                }
                double endFitOne = testone.curPop[endOne].fitness;

                if (startFit > endFit)
                {
                    fullimprov++;
                }

                if (startFit > endFitOne)
                {
                    oneimprov++;
                }
            }
            double temp1 = (double)fullimprov * 100 / size;
            double temp2 = (double)oneimprov * 100 / size;
            return new double[] { temp1, temp2 };
        }

        public static double[] TestMultiCrossover(int size)
        {
            double[] interval = { -500, 500, 0.01 };
            int fullimprov = 0;
            int oneimprov = 0;
            double[][] inp0 = { interval, interval, interval, interval, interval };
            Population test = new Population(inp0, 2);
            Population testone = new Population(inp0, 2);
            for (int i = 0; i < size; i++)
            {
                if (i % (size / 10) == 0)
                {
                    Console.WriteLine("Тест многопозиционного кроссовера №" + i.ToString());
                }
                string rbctemp1 = RandomBinary(test.fragmentSize * 5);
                string rbctemp2 = RandomBinary(test.fragmentSize * 5);
                test.curPop[0].rbcvalue = rbctemp1;
                test.curPop[1].rbcvalue = rbctemp2;
                test.fitnessTest(RanaFitness);
                int start = 0;
                if (test.curPop[start].fitness > test.curPop[Math.Abs(start - 1)].fitness)
                {
                    start = Math.Abs(start - 1);
                }
                double startFit = test.curPop[start].fitness;
                int[] fullIndex = GetMultiIndex(test.fragmentSize * 5);
                int[] oneIndex = GetMultiIndex(test.fragmentSize);
                int segment = rand.Next(5);

                string[] temp = Crossover(new string[] { rbctemp1, rbctemp2 }, fullIndex);
                string[] tempSmall = Crossover(new string[] { GetSegment(rbctemp1, segment), GetSegment(rbctemp2, segment) }, oneIndex);

                test.curPop[0].rbcvalue = temp[0];
                test.curPop[1].rbcvalue = temp[1];
                test.fitnessTest(RanaFitness);
                int end = 0;
                if (test.curPop[end].fitness > test.curPop[Math.Abs(end - 1)].fitness)
                {
                    end = Math.Abs(start - 1);
                }
                double endFit = test.curPop[end].fitness;


                testone.curPop[0].rbcvalue = SetSegment(rbctemp1, tempSmall[0], segment);
                testone.curPop[1].rbcvalue = SetSegment(rbctemp2, tempSmall[1], segment);
                testone.fitnessTest(RanaFitness);
                int endOne = 0;
                if (testone.curPop[endOne].fitness > testone.curPop[Math.Abs(endOne - 1)].fitness)
                {
                    endOne = Math.Abs(start - 1);
                }
                double endFitOne = testone.curPop[endOne].fitness;

                if (startFit > endFit)
                {
                    fullimprov++;
                }

                if (startFit > endFitOne)
                {
                    oneimprov++;
                }
            }
            double temp1 = (double)fullimprov * 100 / size;
            double temp2 = (double)oneimprov * 100 / size;
            return new double[] { temp1, temp2 };
        }

        public static double[] TestSmoothCrossover(int size)
        {
            double[] interval = { -500, 500, 0.01 };
            int fullimprov = 0;
            int oneimprov = 0;
            double[][] inp0 = { interval, interval, interval, interval, interval };
            Population test = new Population(inp0, 2);
            Population testfull = new Population(inp0, 2);
            Population testone = new Population(inp0, 2);
            for (int i = 0; i < size; i++)
            {
                if (i % (size / 10) == 0)
                {
                    Console.WriteLine("Тест гладкого кроссовера №" + i.ToString());
                }
                // Подготовка
                string rbctemp1 = RandomBinary(test.fragmentSize * 5);
                string rbctemp2 = RandomBinary(test.fragmentSize * 5);
                test.curPop[0].rbcvalue = rbctemp1;
                test.curPop[1].rbcvalue = rbctemp2;
                test.fitnessTest(RanaFitness);
                // Вычисление лучшей особи перед оператором
                int start = 0;
                if (test.curPop[start].fitness < test.curPop[Math.Abs(start - 1)].fitness)
                {
                    start = Math.Abs(start - 1);
                }
                double startFit = test.curPop[start].fitness;
                // Подготовка операторов
                int fullIndex = rand.Next(test.fragmentSize * 5);
                int oneIndex = rand.Next(test.fragmentSize);
                int segment = rand.Next(5);
                double threshold = rand.NextDouble();
                // Применение оператора на всю гамету
                testfull.curPop[0].rbcvalue = Crossover(new string[] { rbctemp1, rbctemp2 }, threshold);
                testfull.fitnessTest(RanaFitness);
                // Применение оператора на переменную
                testone.curPop[0].rbcvalue = SetSegment(rbctemp1, Crossover(new string[] { GetSegment(rbctemp1, segment), GetSegment(rbctemp2, segment) }, threshold), segment);
                testone.fitnessTest(RanaFitness);

                if (startFit > testfull.curPop[0].fitness)
                {
                    fullimprov++;
                }

                if (startFit > testone.curPop[0].fitness)
                {
                    oneimprov++;
                }
            }
            double temp1 = (double)fullimprov * 100 / size;
            double temp2 = (double)oneimprov * 100 / size;
            return new double[] { temp1, temp2 };
        }

        public static double[] TestTemplateCrossover(int size)
        {
            double[] interval = { -500, 500, 0.01 };
            int fullimprov = 0;
            int oneimprov = 0;
            double[][] inp0 = { interval, interval, interval, interval, interval };
            Population test = new Population(inp0, 2);
            Population testfull = new Population(inp0, 2);
            Population testone = new Population(inp0, 2);
            for (int i = 0; i < size; i++)
            {
                if (i % (size / 10) == 0)
                {
                    Console.WriteLine("Тест шаблонного кроссовера №" + i.ToString());
                }
                // Подготовка
                string rbctemp1 = RandomBinary(test.fragmentSize * 5);
                string rbctemp2 = RandomBinary(test.fragmentSize * 5);
                test.curPop[0].rbcvalue = rbctemp1;
                test.curPop[1].rbcvalue = rbctemp2;
                test.fitnessTest(RanaFitness);
                // Вычисление лучшей особи перед оператором
                int start = 0;
                if (test.curPop[start].fitness > test.curPop[Math.Abs(start - 1)].fitness)
                {
                    start = Math.Abs(start - 1);
                }
                double startFit = test.curPop[start].fitness;
                // Подготовка операторов
                int fullIndex = rand.Next(test.fragmentSize * 5);
                int oneIndex = rand.Next(test.fragmentSize);
                int segment = rand.Next(5);
                double threshold = 0.5;
                // Применение оператора на всю гамету
                testfull.curPop[0].rbcvalue = Crossover(new string[] { rbctemp1, rbctemp2 }, threshold);
                testfull.fitnessTest(RanaFitness);
                // Применение оператора на переменную
                testone.curPop[0].rbcvalue = SetSegment(rbctemp1, Crossover(new string[] { GetSegment(rbctemp1, segment), GetSegment(rbctemp2, segment) }, threshold), segment);
                testone.fitnessTest(RanaFitness);

                if (startFit > testfull.curPop[0].fitness)
                {
                    fullimprov++;
                }

                if (startFit > testone.curPop[0].fitness)
                {
                    oneimprov++;
                }
            }
            double temp1 = (double)fullimprov * 100 / size;
            double temp2 = (double)oneimprov * 100 / size;
            return new double[] { temp1, temp2 };
        }

        public static double[] TestSegregate(int size)
        {
            double[] interval = { -500, 500, 0.01 };
            int fullimprov = 0;
            int oneimprov = 0;
            double[][] inp0 = { interval, interval, interval, interval, interval };
            Population test = new Population(inp0, 4);
            Population testfull = new Population(inp0, 4);
            Population testone = new Population(inp0, 4);
            for (int i = 0; i < size; i++)
            {
                if (i % (size / 10) == 0)
                {
                    Console.WriteLine("Тест шаблонного кроссовера №" + i.ToString());
                }
                // Подготовка
                string rbctemp1 = RandomBinary(test.fragmentSize * 5);
                string rbctemp2 = RandomBinary(test.fragmentSize * 5);
                test.curPop[0].rbcvalue = rbctemp1;
                test.curPop[1].rbcvalue = rbctemp2;
                test.fitnessTest(RanaFitness);
                // Вычисление лучшей особи перед оператором
                double[] tempFit = { test.curPop[0].fitness, test.curPop[1].fitness, test.curPop[2].fitness, test.curPop[3].fitness };
                Array.Sort(tempFit);
                double startFit = tempFit[3];
                // Подготовка операторов
                int fullIndex = rand.Next(test.fragmentSize * 5);
                int oneIndex = rand.Next(test.fragmentSize);
                int segment = rand.Next(5);
                double threshold = 0.5;
                // Применение оператора на всю гамету
                testfull.curPop[0].rbcvalue = Crossover(new string[] { rbctemp1, rbctemp2 }, threshold);
                testfull.fitnessTest(RanaFitness);
                // Применение оператора на переменную
                testone.curPop[0].rbcvalue = SetSegment(rbctemp1, Crossover(new string[] { GetSegment(rbctemp1, segment), GetSegment(rbctemp2, segment) }, threshold), segment);
                testone.fitnessTest(RanaFitness);

                if (startFit > testfull.curPop[0].fitness)
                {
                    fullimprov++;
                }

                if (startFit > testone.curPop[0].fitness)
                {
                    oneimprov++;
                }
            }
            double temp1 = (double)fullimprov * 100 / size;
            double temp2 = (double)oneimprov * 100 / size;
            return new double[] { temp1, temp2 };
        }
    }
}
