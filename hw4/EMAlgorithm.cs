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

        public IEnumerable<int> Cluster(IEnumerable<Image> data)
        {
            var X = data.ToList();

            double[] pi = new double[Category];
            var bern = new Core.Distributions.BernoulliDistribution[Category, Dimension];
            // Responsibility [i, j] given x_i for j group
            double[,] z = new double[X.Count, Category];
            var rnd = new Random();

            // Initialize
            for (int i = 0; i < Category; ++i)
            {
                pi[i] = rnd.Next();
                for(int j = 0; j < Dimension; ++j)
                {
                    bern[i, j] = new Core.Distributions.BernoulliDistribution(rnd.NextDouble());
                }
            }
            double sum = pi.Sum();
            for (int i = 0; i < Category; ++i)
            {
                pi[i] /= sum;
            }

            double prevLikelihood = 0;
            // EM iterations
            while (true)
            {
                // Expectation
                for (int i = 0; i < X.Count; ++i)
                {
                    for(int j = 0; j < Category; ++j)
                    {
                        z[i, j] = pi[j];
                        for (int k = 0; k < Dimension; ++k)
                        {
                            z[i, j] *= Math.Pow(bern[j, k].Mu, X[i].PixelBin[k]) * Math.Pow(1.0 - bern[j, k].Mu, 1 - X[i].PixelBin[k]);
                        }
                        sum += z[i, j];
                    }
                    sum = Array2DExtensions.ColumnSum(z, i);
                    for (int j = 0; j < Category; ++j)
                    {
                        z[i, j] /= sum;
                    }
                }

                // Maximization
                for (int i = 0; i < Category; ++i)
                {
                    double effectiveNumber = Array2DExtensions.ColumnSum(z, i);

                    pi[i] =  effectiveNumber / X.Count;

                    for(int j = 0; j < Dimension; ++j)
                    {
                        double numerator = 0;
                        for(int k = 0; k < X.Count; ++k)
                        {
                            numerator += z[k, i] * X[k].PixelBin[j];
                        }
                        bern[i, j] = new Core.Distributions.BernoulliDistribution(numerator / effectiveNumber);
                    }
                }

                double likelihood = 0;
                for (int i = 0; i < X.Count; ++i)
                {
                    for(int j = 0; j < Category; ++j)
                    {
                        double v = Math.Log(pi[j]);
                        for(int k = 0; k < Dimension; ++k)
                        {
                            v += X[i].PixelBin[k] * Math.Log(bern[j, k].Mu) + (1.0 - X[i].PixelBin[k]) * (1 - Math.Log(bern[j, k].Mu));
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
            }

            var list = new List<int>();
            for (int i = 0; i < X.Count; ++i)
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
