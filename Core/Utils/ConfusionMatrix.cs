using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Utils
{
    public class ConfusionMatrix
    {
        public int TruePositive { get; private set; }
        public int FalsePositive { get; private set; }
        public int TrueNegative { get; private set; }
        public int FalseNegative { get; private set; }

        public ConfusionMatrix()
        {
            TruePositive = 0;
            TrueNegative = 0;
            FalseNegative = 0;
            FalsePositive = 0;
        }

        public ConfusionMatrix(IEnumerable<int> actual, IEnumerable<int> expected) : this()
        {
            Compute(actual.ToArray(), expected.ToArray());
        }

        private void Compute(int[] actual, int[] expected)
        {
            if (actual.Length != expected.Length) throw new ArgumentException();
            for (int i = 0; i < expected.Length; ++i)
            {
                int u = actual[i];
                int v = expected[i];
                if (u == 1 && v == 1)
                    ++TruePositive;
                else if (u == 1 && v == 0)
                    ++FalsePositive;
                else if (u == 0 && v == 1)
                    ++FalseNegative;
                else if (u == 0 && v == 0)
                    ++TrueNegative;
            }
        }

        public void Print()
        {
            Console.WriteLine("\t\tExpected");
            Console.WriteLine("\t\t1\t0");
            Console.WriteLine($"Actual\t1\t{ TruePositive }\t{ FalsePositive }");
            Console.WriteLine($"\t0\t{ FalseNegative }\t{ TrueNegative }");
        }
    }
}
