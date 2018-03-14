using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreLib
{
    /// <summary>
    /// Implement newton to fit LSE
    /// </summary>
    public class NewtonMethodTrainer : ITrainer<double, double>
    {
        public int PolynomialBasesNumber { get; private set; }

        private Matrix _weight;

        public NewtonMethodTrainer(int n)
        {
            PolynomialBasesNumber = n;
        }

        public double Predict(double data)
        {
            double sum = 0, p = 1;
            for (int i = 0; i < _weight.N; ++i)
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

        public void Train(List<(double, double)> data)
        {
            var A = GetDesignMatrix(PolynomialBasesNumber, data.Select(d => d.Item1).ToList());
            var b = new Matrix(A.N, 1);
            for (int i = 0; i < A.N; ++i)
            {
                b[i, 0] = data[i].Item2;
            }

            Matrix x0 = new Matrix(A.M, 1), x1;
            var temp = 2 * A.GetTranspose() * A;
            var gradientA = temp * x0 - 2 * A.GetTranspose() * b;
            Matrix.TryGetInverse(in temp, out Matrix inversedHession);

            x1 = x0 - inversedHession * gradientA;

            _weight = new Matrix(x1);
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
