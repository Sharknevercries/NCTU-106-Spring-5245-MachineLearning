using Core;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HW1
{
    /// <summary>
    /// Implement rLSE 
    /// </summary>
    public class PolynomialRegressionTrainer : ITrainer<double, double>
    {
        public int PolynomialBasesNumber { get; private set; }
        public double Lambda { get; private set; }

        private Matrix _weight;

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
        public void Train(List<(double, double)> data)
        {
            var A = GetDesignMatrix(PolynomialBasesNumber, data.Select(d => d.Item1).ToList());
            var b = new Matrix(A.N, 1);
            for(int i = 0; i < A.N; ++i)
            {
                b[i, 0] = data[i].Item2;
            }
            _weight = new Matrix(A.N, 1);

            b = A.GetTranspose() * b;

            Matrix.SolveByLUDecomposition(A.GetTranspose() * A + Lambda * Matrix.GetDiagnoalMatrix(A.M), b, out _weight);
        }

        public double Predict(double data)
        {
            double sum = 0, p = 1;
            for(int i = 0; i < _weight.N;++i)
            {
                sum += _weight[i, 0] * p;
                p *= data;
            }
            return sum;
        }

        public IEnumerable<double> Predict(IEnumerable<double> data)
        {
            return data.Select(d => Predict(d));
        }

        private static Matrix GetDesignMatrix(int n, IEnumerable<double> x)
        {
            var t = x.ToList();
            Matrix m = new Matrix(t.Count, n + 1);

            for (int i = 0; i < m.N; ++i)
            {
                for (int j = 0; j < m.M; ++j)
                {
                    m[i, j] = Math.Pow(t[i], j);
                }
            }

            return m;
        }

        public double Error((double, double) data)
        {
            return Math.Pow(Predict(data.Item1) - data.Item2, 2);
        }

        public double Error(List<(double, double)> data)
        {
            return data.Select(d => Error(d)).Sum();
        }

        public void PrintModel()
        {
            for (int i = PolynomialBasesNumber; i >= 0; --i)
            {
                Console.Write($"{ _weight[i, 0] }x^{ i }\t");
            }
            Console.WriteLine();
        }
    }
}
