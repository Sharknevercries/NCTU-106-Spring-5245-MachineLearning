using Core;
using Core.Interfaces;
using Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HW4
{
    public class LogisticRegressionTrainer : ITrainer<(double, double), double>
    {
        public Matrix Weight { get; private set; }
        public bool IsShowingTrainingProcess { get; set; }
        public int Iteration { get; private set; }

        public LogisticRegressionTrainer()
        {
            Weight = null;
            Iteration = 0;
            IsShowingTrainingProcess = false;
        }

        public double Error(((double, double), double) data)
        {
            throw new NotImplementedException();
        }

        public double Error(IEnumerable<((double, double), double)> data)
        {
            throw new NotImplementedException();
        }

        public double Predict((double, double) data)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<double> Predict(IEnumerable<(double, double)> data)
        {
            throw new NotImplementedException();
        }

        public void PrintModel()
        {
            Console.WriteLine($"Iter: { Iteration } ========");
            Console.Write("w = ");
            for (int i = 0; i < Weight.N; ++i)
            {
                Console.Write(" " + Weight[i, 0]);
            }
            Console.WriteLine();
        }

        public void Train(IEnumerable<((double, double), double)> data)
        {
            var A = GetDesignMatrix(data.Select(d => d.Item1));
            var Y = new Matrix(data.Select(d => d.Item2));
            Weight = new Matrix(A.M, 1);

            ConfusionMatrix cm = new ConfusionMatrix();
            Iteration = 0;
            while (true)
            {
                // TODO: Newton Hessian to enhance convergence
                var gradient = A.GetTranspose() * (1.0 / (1.0 + Exp(-(A * Weight))) - Y);
                var D = new Matrix(A.N, A.N);
                var z = A * Weight;
                for (int i = 0; i < A.N; ++i)
                {
                    var zi = z[i, 0];
                    D[i, i] = Sigmoid(zi) * (1 - Sigmoid(zi));
                }
                var heissianMatrix = A.GetTranspose() * D * A;
                
                // Some data may let the w to be very huge. (Why?)
                if (Matrix.TryGetInverse(in heissianMatrix, out var inverseHessianMatrix))
                {
                    Weight -= inverseHessianMatrix * gradient;
                }
                else
                {
                    Weight -= 0.01 * gradient;
                }

                // TODO: How to ensure convergence?
                if (gradient.L1Norm() < -1e9)
                {
                    // Converge
                    break;
                }

                cm = new ConfusionMatrix(
                    z.GetColArray(0).Select(v => v >= 0 ? 1 : 0),
                    Y.GetColArray(0).Select(v => (int)v));

                if (IsShowingTrainingProcess)
                {
                    PrintModel();
                    cm.Print();
                }
                ++Iteration;
            }
            cm.Print();
        }

        private static Matrix GetDesignMatrix(IEnumerable<(double, double)> points)
        {
            var x = points.ToList();
            Matrix A = new Matrix(x.Count, 3);
            for (int i = 0; i < A.N; ++i)
            {
                A[i, 0] = 1;
                A[i, 1] = x[i].Item1;
                A[i, 2] = x[i].Item2;
            }
            return A;
        }

        private static double Sigmoid(double v)
        {
            return 1.0 / (1.0 + Math.Exp(v));
        }

        private static Matrix Exp(Matrix m)
        {
            Matrix ret = new Matrix(m);
            for (int i = 0; i < ret.N; ++i)
            {
                for (int j = 0; j < ret.M; ++j)
                {
                    ret[i, j] = Math.Exp(ret[i, j]);
                }
            }
            return ret;
        }
    }
}
