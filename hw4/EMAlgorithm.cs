using Core.DataType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HW4
{
    public class MNISTEMAlgorithm
    {
        public const int Category = 10;
        public const int Dimension = 28 * 28;

        public const double Alpha1 = 1e-8;
        public const double Alpha2 = 1e-8;

        public IEnumerable<int> Cluster(IEnumerable<Image> data)
        {
            var X = data.ToArray();

            double[] pi = new double[Category];
            var bern = new Core.Distributions.BernoulliDistribution[Category, Dimension];
            // Responsibility [i, j] given x_i for j group
            double[,] z = new double[X.Length, Category];
            var rnd = new Random();

            // Initialize
            for (int i = 0; i < Category; ++i)
            {
                pi[i] = 1.0 / Category;
                double sum = 0;
                for(int j = 0; j < Dimension; ++j)
                {
                    bern[i, j] = new Core.Distributions.BernoulliDistribution(rnd.NextDouble());
                    sum += bern[i, j].Mu * bern[i, j].Mu;
                }
                for (int j = 0; j < Dimension; ++j)
                {
                    bern[i, j].Mu /= Math.Sqrt(sum);
                }
            }
            double prevLikelihood = double.MinValue;
            int iteration = 1;
            // EM iterations
            while (true)
            {
                Console.WriteLine($"Iter: { iteration++ }");

                // Expectation
                for (int i = 0; i < X.Length; ++i)
                {
                    double maxZ = double.MinValue;
                    for(int j = 0; j < Category; ++j)
                    {
                        z[i, j] = Math.Log(pi[j]);
                        for (int k = 0; k < Dimension; ++k)
                        {
                            //z[i, j] *= 1.22;
                            z[i, j] += X[i].PixelBin[k] * Math.Log(bern[j, k].Mu) + (1 - X[i].PixelBin[k]) * Math.Log(1.0 - bern[j, k].Mu);
                        }
                        maxZ = Math.Max(maxZ, z[i, j]);
                    }
                    for (int j = 0; j < Category; ++j)
                    {
                        z[i, j] = Math.Exp(z[i, j] - maxZ);
                    }
                    double sum = Array2DExtensions.RowSum(z, i);
                    for (int j = 0; j < Category; ++j)
                    {
                        z[i, j] /= sum;
                    }
                }

                // Maximization
                for (int i = 0; i < Category; ++i)
                {
                    // Dirichlet prior smoothing
                    double effectiveNumber = Array2DExtensions.ColumnSum(z, i);

                    pi[i] =  (effectiveNumber + Alpha2) / (X.Length + Category * Alpha2);

                    for(int j = 0; j < Dimension; ++j)
                    {
                        double numerator = 0;
                        for(int k = 0; k < X.Length; ++k)
                        {
                            numerator += z[k, i] * X[k].PixelBin[j];
                        }
                        bern[i, j] = new Core.Distributions.BernoulliDistribution((numerator + Alpha1) / (effectiveNumber + Dimension * Alpha1));
                    }
                }

                double likelihood = 0;
                for (int i = 0; i < X.Length; ++i)
                {
                    for(int j = 0; j < Category; ++j)
                    {
                        double v = Math.Log(pi[j]);
                        for (int k = 0; k < Dimension; ++k)
                        {
                            v += X[i].PixelBin[k] * Math.Log(bern[j, k].Mu) + (1 - X[i].PixelBin[k]) * Math.Log(1 - bern[j, k].Mu);
                        }
                        v *= z[i, j];
                        likelihood += v;
                    }
                }

                // Likelihood converge!
                if (likelihood - prevLikelihood < 1e-9)
                {
                    break;
                }

                prevLikelihood = likelihood;
                Console.WriteLine($"Likelihood: { likelihood }");
            }

            var list = new List<int>();
            for (int i = 0; i < X.Length; ++i)
            {
                int maxCategory = -1;
                double maxValue = -1e9;
                for (int j = 0; j < Category; ++j)
                {
                    if (maxValue < z[i, j])
                    {
                        maxValue = z[i, j];
                        maxCategory = j;
                    }
                }
                list.Add(maxCategory);
            }

            return list;
        }
    }

    static class Array2DExtensions
    {
        public static double RowSum(this double[,] value, int index)
        {
            double result = 0;
            for (int i = 0; i <= value.GetUpperBound(1); i++)
            {
                result += value[index, i];
            }
            return result;
        }

        public static double ColumnSum(this double[,] value, int index)
        {
            double result = 0;
            for (int i = 0; i <= value.GetUpperBound(0); i++)
            {
                result += value[i, index];
            }
            return result;
        }
    }
}
