using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core;
using Core.Distributions;

namespace hw4
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

                    var A = GetDesignMatrix(dx1.Concat(dx2));
                    var Y = new Matrix(dy1.Concat(dy2).ToList());

                    int iter = 1;
                    while (true)
                    {
                        // TODO: Newton Hessian to enhance convergence
                        var a1 = -(A * w);
                        var a2 = Exp(a1);
                        var a3 = (1.0 / (1.0 + a2)) - Y;
                        var gradient = A.GetTranspose() * a3;

                        //var gradient = A.GetTranspose() * (1.0 / (1.0 + Exp(-(A * w))) - Y);

                        if (gradient.L1Norm() < -1e9)
                        {
                            // Converge
                            break;
                        }

                        // TODO: How to ensure convergence?
                        w -= 0.01 * gradient;
                        Console.WriteLine($"Iter: { iter } ========");
                        Console.WriteLine($"w = [{ w[0, 0] }, { w[1, 0] }, { w[2, 0] }]");

                        var judge = A * w;
                        PrintConfusionMatrix(judge, Y);
                        ++iter;
                    }
                }
                else if (part == "em")
                {
                }
            }
        }

        private static void PrintConfusionMatrix(Matrix judge, Matrix groundTruth)
        {
            int n = groundTruth.N;
            int tn = 0, tp = 0, fp = 0, fn = 0;

            for (int i = 0; i < n; ++i)
            {
                int u = judge[i, 0] >= 0 ? 1 : 0;
                int v = (int) groundTruth[i, 0];
                if (u == 1 && v == 1)
                    ++tp;
                else if (u == 1 && v == 0)
                    ++fp;
                else if (u == 0 && v == 1)
                    ++fn;
                else if (u == 0 && v == 0)
                    ++tn;
            }

            Console.WriteLine("\t\tActual");
            Console.WriteLine("\t\t1\t0");
            Console.WriteLine($"Judge\t1\t{ tp }\t{ fp }");
            Console.WriteLine($"\t0\t{ fn }\t{ tn }");
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

        private static Matrix Exp(Matrix m)
        {
            Matrix ret = new Matrix(m);
            for (int i = 0; i < ret.N; ++i)
            {
                for(int j = 0; j < ret.M; ++j)
                {
                    ret[i, j] = Math.Exp(ret[i, j]);
                }
            }
            return ret;
        }

        private static Matrix GetDesignMatrix(IEnumerable<(double, double)> points)
        {
            var x = points.ToList();
            Matrix A = new Matrix(x.Count, 5);
            for (int i = 0; i < A.N; ++i)
            {
                A[i, 0] = 1;
                A[i, 1] = x[i].Item1;
                A[i, 2] = x[i].Item2;
                A[i, 3] = Math.Pow(x[i].Item1, 2);
                A[i, 4] = Math.Pow(x[i].Item2, 2);
            }
            return A;
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
