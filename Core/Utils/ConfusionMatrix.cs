using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Utils
{
    public class ConfusionMatrix
    {
        public int[] TruePositive { get; private set; }
        public int[] FalsePositive { get; private set; }
        public int[] TrueNegative { get; private set; }
        public int[] FalseNegative { get; private set; }
        public int Category;

        public ConfusionMatrix(int categroy)
        {
            TruePositive = new int[categroy];
            TrueNegative = new int[categroy];
            FalseNegative = new int[categroy];
            FalsePositive = new int[categroy];
            Category = categroy;
        }

        public ConfusionMatrix(IEnumerable<int> actual, IEnumerable<int> expected, int category) : this(category)
        {
            Compute(actual.ToArray(), expected.ToArray());
        }

        private void Compute(int[] actual, int[] expected)
        {
            if (actual.Length != expected.Length) throw new ArgumentException();

            for (int c = 0; c < Category; ++c)
            {
                for (int i = 0; i < expected.Length; ++i)
                {
                    int u = actual[i];
                    int v = expected[i];
                    if (u == c && v == c)
                        ++TruePositive[c];
                    else if (u == c && v != c)
                        ++FalsePositive[c];
                    else if (u != c && v == c)
                        ++FalseNegative[c];
                    else if (u != c && v != c)
                        ++TrueNegative[c];
                }
            }
        }

        public void Print()
        {
            for(int i = 0; i < Category; ++i)
            {
                Print(i);
            }
           
        }

        public void Print(int category)
        {
            Console.WriteLine($"{ category }\t\tExpected");
            Console.WriteLine("\t\t1\t0");
            Console.WriteLine($"Actual\t1\t{ TruePositive[category] }\t{ FalsePositive[category] }");
            Console.WriteLine($"\t0\t{ FalseNegative[category] }\t{ TrueNegative[category] }");
            Console.WriteLine($"Sensitivity: {(double)TruePositive[category] / (TruePositive[category] + FalseNegative[category]) }");
            Console.WriteLine($"Specificity: {(double)FalsePositive[category] / (TrueNegative[category] + FalsePositive[category]) }");
        }
    }
}
