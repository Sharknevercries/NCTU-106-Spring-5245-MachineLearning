using System;

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

                    var generator = new UnivariateGaussianDataGenerator(mu, sigma);
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
                    throw new NotImplementedException();
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
            Console.WriteLine("\t--a=A                    \tPolynomial basis precision");
            Console.WriteLine("\t--w=[w0, w1, ..., wn]    \tPolynomial basis w");
        }
    }
}
