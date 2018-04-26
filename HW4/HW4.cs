using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Core;
using Core.Distributions;
using Core.Interfaces;

namespace HW4
{
    public class HW4
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                PrintHelp();
            }
            else
            {
                string part = "";
                foreach (var arg in args)
                {
                    if (arg.StartsWith("--part="))
                    {
                        part = arg.Substring(7);
                    }
                }

                if (part == "lr")
                {
                    double mx1 = default, my1 = default, vx1 = default, vy1 = default;
                    double mx2 = default, my2 = default, vx2 = default, vy2 = default;
                    int n = default;
                    foreach (var arg in args)
                    {
                        if (arg.StartsWith("--n="))
                        {
                            n = int.Parse(arg.Substring(4));
                        }
                        else if (arg.StartsWith("--mx1="))
                        {
                            mx1 = double.Parse(arg.Substring(6));
                        }
                        else if (arg.StartsWith("--vx1="))
                        {
                            vx1 = double.Parse(arg.Substring(6));
                        }
                        else if (arg.StartsWith("--my1="))
                        {
                            my1 = double.Parse(arg.Substring(6));
                        }
                        else if (arg.StartsWith("--vy1="))
                        {
                            vy1 = double.Parse(arg.Substring(6));
                        }
                        else if (arg.StartsWith("--mx2="))
                        {
                            mx2 = double.Parse(arg.Substring(6));
                        }
                        else if (arg.StartsWith("--vx2="))
                        {
                            vx2 = double.Parse(arg.Substring(6));
                        }
                        else if (arg.StartsWith("--my2="))
                        {
                            my2 = double.Parse(arg.Substring(6));
                        }
                        else if (arg.StartsWith("--vy2="))
                        {
                            vy2 = double.Parse(arg.Substring(6));
                        }
                    }

                    Matrix w = new Matrix(5, 1);
                    var dx1 = GeneratePoints(n, mx1, vx1, my1, vy1);
                    var dy1 = Enumerable.Repeat<double>(0, n);
                    var dx2 = GeneratePoints(n, mx2, vx2, my2, vy2);
                    var dy2 = Enumerable.Repeat<double>(1, n);

                    var trainer = new LogisticRegressionTrainer();
                    trainer.IsShowingTrainingProcess = true;
                    trainer.Train(dx1.Concat(dx2).Zip(dy1.Concat(dy2), (a, b) => (a, b)));

                    trainer.PrintModel();
                }
                else if (part == "em")
                {
                    string trainDataPath = default;
                    string trainLabelPath = default;
                    string testDataPath = default;
                    string testLabelPath = default;
                    string output = "result";

                    foreach (var arg in args)
                    {
                        if (arg.StartsWith("--train="))
                        {
                            trainDataPath = arg.Substring(8);
                        }
                        else if (arg.StartsWith("--train_label="))
                        {
                            trainLabelPath = arg.Substring(14);
                        }
                        else if (arg.StartsWith("--test="))
                        {
                            testDataPath = arg.Substring(7);
                        }
                        else if (arg.StartsWith("--test_label="))
                        {
                            testLabelPath = arg.Substring(13);
                        }
                        else if (arg.StartsWith("--output="))
                        {
                            output = arg.Substring(9);
                        }
                    }

                    var train = Core.Datasets.MNIST.GetDataset(trainDataPath, trainLabelPath, 128);

                    FileStream fs = new FileStream(output, FileMode.Create);
                    StreamWriter sw = new StreamWriter(fs);
                    TextWriter stdout = Console.Out;

                    var trainer = new MNISTEMAlgorithm();
                    var ret = trainer.Cluster(train);
                    
                    var cm = new Core.Utils.ConfusionMatrix(ret, train.Select(v => v.Label), 10);
                    cm.Print();
                    Console.SetOut(sw);
                    cm.Print();
                }
            }
        }

        private static IEnumerable<(double, double)> GeneratePoints(int n, double mx, double vx, double my, double vy)
        {
            var list = new List<(double, double)>();
            for(int i = 0; i < n; ++i)
            {
                list.Add((UnivariateGaussianDistribution.Generate(mx, vx), UnivariateGaussianDistribution.Generate(my, vy)));
            }
            return list;
        }

        private static void PrintHelp()
        {
            Console.WriteLine("Usage: hw4 [arguments]");
            Console.WriteLine("arguments:");
            Console.WriteLine("\t--part=PART              \tLogistic regression(lr) or EM(em)");
            Console.WriteLine("\t================Logistic regression================");
            Console.WriteLine("\t--n=N                    \tNumber of points to be generated by each distribution.");
            Console.WriteLine("\t--mx1=MX1                \tN(MX1, VX1)");
            Console.WriteLine("\t--vx1=VX1                \tN(MX1, VX1)");
            Console.WriteLine("\t--my1=MY1                \tN(MY1, VY1)");
            Console.WriteLine("\t--vy1=VY1                \tN(MY1, VY1)");
            Console.WriteLine("\t--mx2=MX2                \tN(MX2, VX2)");
            Console.WriteLine("\t--vx2=VX2                \tN(MX2, VX2)");
            Console.WriteLine("\t--my2=MY2                \tN(MY2, VY2)");
            Console.WriteLine("\t--vy2=VY2                \tN(MY2, VY2)");
        }
    }
}
