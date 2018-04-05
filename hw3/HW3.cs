using Core;
using HW3.Generator;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HW3
{
    class HW3
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

                if (part == "se")
                {
                    // Sequential estimate the mean and variance from the data given from the univariate gaussian data generator
                    double mu = default;
                    double sigma = default;

                    foreach (var arg in args)
                    {
                        if (arg.StartsWith("--mu="))
                        {
                            mu = double.Parse(arg.Substring(5));
                        }
                        else if (arg.StartsWith("--sig="))
                        {
                            sigma = double.Parse(arg.Substring(6));
                        }
                    }

                    var generator = new Core.Distributions.UnivariateGaussianDistribution(mu, sigma);
                    double sumOfSquareDiff = 0;
                    double sampleMu = 0;
                    double sampleVariance = 0;
                    double populationVariance = 0;
                    bool isConverge = false;

                    int iter = 0;
                    Console.WriteLine($"Iter { iter++ }, Esti_Mu: { sampleMu }, Esti_Var: { sampleVariance }");

                    while (!isConverge) { 
                        double x = generator.Generate();

                        double nextSampleMu = sampleMu + (x - sampleMu) / iter;
                        double nextSumOfSquareDiff = sumOfSquareDiff + (x - sampleMu) * (x - nextSampleMu);
                        double nextSampleVariance = iter > 1 ? nextSumOfSquareDiff / (iter - 1) : 0;

                        if (Math.Abs(nextSampleMu - sampleMu) < 1e-6 && Math.Abs(nextSampleVariance - sampleVariance) < 1e-6)
                            isConverge = true;

                        sumOfSquareDiff = nextSumOfSquareDiff;
                        sampleVariance = nextSampleVariance;
                        populationVariance = nextSampleVariance / iter;
                        sampleMu = nextSampleMu;

                        Console.WriteLine($"Iter { iter++ }, with new data: { x }, Esti_Mu: { sampleMu }, Esti_Var: { sampleVariance }");
                    }
                }
                else if (part == "blr")
                {
                    // Baysian linear regression
                    int nBasis = default;
                    double a = default;
                    double b = default;
                    var w = new List<double>();

                    foreach (var arg in args)
                    {
                        if (arg.StartsWith("--n="))
                        {
                            nBasis = int.Parse(arg.Substring(4));
                        }
                        else if (arg.StartsWith("--a="))
                        {
                            a = double.Parse(arg.Substring(4));
                        }
                        else if (arg.StartsWith("--b="))
                        {
                            b = double.Parse(arg.Substring(4));
                        }
                        else if (arg.StartsWith("--w="))
                        {
                            var s = arg.Substring(4);
                            var ss = s.Split(new char[] { '[', ']', ',' });
                            foreach(var str in ss)
                            {
                                if (double.TryParse(str, out double ret))
                                {
                                    w.Add(ret);
                                }
                            }
                        }
                    }

                    bool isConverge = false;
                    var prior = new Core.Distributions.MultivariateGaussianDistribution(Enumerable.Repeat(0.0, nBasis), b * Matrix.GetDiagnoalMatrix(nBasis));
                    var polynomialBasisLinearModel = new PolynomialBasisLinearModelDistribution(nBasis, a, w);
                    var uniformModel = new Core.Distributions.UniformDistribution(-10.0, 10.0);
                    prior.PrintInfo();
                    int iter = 1;

                    while (!isConverge)
                    {
                        Console.WriteLine($"Iter: { iter++ } =================");
                        var x = uniformModel.Generate();
                        var y = polynomialBasisLinearModel.Generate(1, new List<double>() { x });

                        var X = Utility.PolynomialBasisLinearModelUtility.GetDesignMatrix(nBasis, 2);
                        var Y = new Matrix(y, false);
                        var posteriorPrecision = a * X.GetTranspose() * X + prior.Precision;

                        if (!Matrix.TryGetInverse(in posteriorPrecision, out var posteriorCovariance))
                        {
                            // Maybe try pseudo-inverse ?
                            throw new NotImplementedException();
                        }

                        var posteriorMean = posteriorCovariance * (prior.Precision * prior.Mean + a * X.GetTranspose() * Y);

                        Console.WriteLine($"New data: ({ x }, { y.ToArray()[0] }) ");

                        prior = new Core.Distributions.MultivariateGaussianDistribution(posteriorMean, posteriorPrecision);
                        prior.PrintInfo();
                    }
                }
            }
        }

        private static void PrintHelp()
        {
            Console.WriteLine("Usage: hw3 [arguments]");
            Console.WriteLine("arguments:");
            Console.WriteLine("\t--part=PART              \tSequential estimate(se) or Baysian linear regression(blr)");
            Console.WriteLine("\t--mu=MU                  \tUnivariate gaussian dist. mean");
            Console.WriteLine("\t--sig=SIG                \tUnivariate gaussian dist. sigma");
            Console.WriteLine("\t--n=N                    \tPolynomial basis number");
            Console.WriteLine("\t--a=A                    \tPolynomial basis linear model noise ~ N(0, a) precision");
            Console.WriteLine("\t--w=[w0,w1,...,wn]    \tPolynomial basis linear model w");
            Console.WriteLine("\t--b=B                    \tBaysian linear regression prior N(0, b^-1I)");
        }
    }
}
