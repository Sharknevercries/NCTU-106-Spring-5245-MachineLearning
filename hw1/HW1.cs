using System;
using CoreLib;

namespace HW1
{
    class HW1
    {
        static void Main(string[] args)
        {
            string filepath = "";
            double lambda = 0;
            int n = 0;

            if (args.Length == 0)
            {
                PrintHelp();
            }
            else
            {
                foreach (var arg in args)
                {
                    if (arg.StartsWith("--input="))
                    {
                        filepath = arg.Substring(8);
                    }
                    else if (arg.StartsWith("--lambda="))
                    {
                        lambda = double.Parse(arg.Substring(9));
                    }
                    else if (arg.StartsWith("--n="))
                    {
                        n = int.Parse(arg.Substring(4));
                    }
                }

                
            }
        }

        static void PrintHelp()
        {
            Console.WriteLine("Usage: hw1 [arguments]");
            Console.WriteLine("arguments:");
            Console.WriteLine("\t--input=FILENAME\tInput file path");
            Console.WriteLine("\t--lambda=LAMBDA \tRegularization coef");
            Console.WriteLine("\t--n=N           \tThe number of polynomial bases");
        }
    }
}
