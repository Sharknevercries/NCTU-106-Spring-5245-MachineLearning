using CoreLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreLib
{
    /// <summary>
    /// Implement rLSE 
    /// </summary>
    public class PolynomialRegressionTrainer : Trainer
    {
        public int PolynomialBasesNumber { get; private set; }
        public double Lambda { get; private set; }

        public PolynomialRegressionTrainer() : this(1)
        {
        }

        public PolynomialRegressionTrainer(int n) : this(n, 0)
        {
        }

        public PolynomialRegressionTrainer(int n, double lambda)
        {
            PolynomialBasesNumber = n;
            Lambda = lambda;
        }

        /// <summary>
        /// Try to find a polynomial function with order n to fit the data using rLSE
        /// </summary>
        /// <param name="data"></param>
        public void Train(List<Tuple<double, double>> data)
        {
            var A = GetDesignMatrix(PolynomialBasesNumber, data.Select(d => d.Item1).ToList());
            

        }

        private static Matrix GetDesignMatrix(int n, List<double> x)
        {
            Matrix m = new Matrix(x.Count, n);

            for (int i = 0; i < m.N; ++i)
            {
                for (int j = 0; j < m.M; ++j)
                {
                    m[i, j] = Math.Pow(x[i], j);
                }
            }

            return m;
        }
    }
}
