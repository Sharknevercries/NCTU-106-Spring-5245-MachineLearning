using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HW1
{
    class HW1
    {
        static void Main(string[] args)
        {
            string filepath = "";
            double lambda = 0;
            string mode = "lse";
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
                    else if (arg.StartsWith("--mode="))
                    {
                        mode = arg.Substring(7);
                    }
                }

                var data = new List<(double, double)>();
                foreach (var line in File.ReadLines(filepath))
                {
                    var items = line.Split(new char[] { ',' });
                    data.Add((double.Parse(items[0]), double.Parse(items[1])));
                }

                ITrainer<double, double> trainer;

                if(mode == "lse")
                {
                    trainer = new PolynomialRegressionTrainer(n, lambda);
                }
                else if (mode == "newton")
                {
                    trainer = new NewtonMethodTrainer(n);
                }
                else
                {
                    PrintHelp();
                    return;
                }

                trainer.Train(data);
                trainer.PrintModel();
                Console.WriteLine($"Error= { trainer.Error(data) }");
            }
        }

        static void PrintHelp()
        {
            Console.WriteLine("Usage: hw1 [arguments]");
            Console.WriteLine("arguments:");
            Console.WriteLine("\t--input=FILENAME   \tInput file path");
            Console.WriteLine("\t--lambda=LAMBDA    \tRegularization coef");
            Console.WriteLine("\t--n=N              \tThe number of polynomial bases up to x^N");
            Console.WriteLine("\t--mode=MODE        \tlse or newton");
        }
    }
}
