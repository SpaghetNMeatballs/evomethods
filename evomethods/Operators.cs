using System;
using System.Collections.Generic;
using System.Text;

namespace evomethods
{
    partial class Program
    {
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

        // Однопозиционный кроссовер
        public static string[] Crossover(string[] rbcvalues, int position)
        {
            string rbcvalue1 = rbcvalues[0], rbcvalue2 = rbcvalues[1];
            string temp1 = rbcvalue1.Substring(0, position);
            string temp2 = rbcvalue2.Substring(0, position);
            temp1 += rbcvalue2.Substring(position);
            temp2 += rbcvalue1.Substring(position);
            return new string[] { temp1, temp2 };
        }

        // Многопозиционный кроссовер
        public static string[] Crossover(string[] rbcvalues, int[] position)
        {
            string rbc1 = rbcvalues[0], rbc2 = rbcvalues[1];
            string temp1 = rbc2.Substring(0, position[0] - 1);
            string temp2 = rbc1.Substring(0, position[0] - 1);
            for (int i = 1; i < position.Length; i++)
            {
                temp1 += rbc1.Substring(position[i - 1], position[i] - position[i - 1]);
                temp2 += rbc2.Substring(position[i - 1], position[i] - position[i - 1]);

                string temp = temp1;
                temp1 = temp2;
                temp2 = temp;
            }
            temp1 += rbc1.Substring(position[position.Length - 1] - 1);
            temp2 += rbc2.Substring(position[position.Length - 1] - 1);
            return new string[] { temp1, temp2 };
        }

        // Гладкий кроссовер
        public static string Crossover(string[] rbcvalues, double threshhold)
        {
            string output = "";
            for (int i = 0; i < rbcvalues[0].Length; i++)
            {
                double temp = rand.NextDouble();
                output += temp > threshhold ? rbcvalues[0][i] : rbcvalues[1][i];
            }
            return output;
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

        public static string Segregate(string[] rbcvalues)
        {
            string output = "";
            while (output.Length < rbcvalues[0].Length)
            {
                int index = rand.Next(rbcvalues.Length - 2);
                int leng = rbcvalues.Length - index < rbcvalues[0].Length - output.Length ? rbcvalues.Length - index : rbcvalues[0].Length - output.Length;
                output += rbcvalues[rand.Next(rbcvalues.Length)].Substring(index, leng);
            }
            return output;
        }
    }
}
