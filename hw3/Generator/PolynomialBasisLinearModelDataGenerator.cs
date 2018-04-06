using Core;
using Core.Distributions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HW3.Generator
{
    public class PolynomialBasisLinearModelDistribution
    {
        public int N { get; private set; }
        public IEnumerable<double> W { get; private set; }
        public IDistribution<double> Noise { get; private set; }

        public PolynomialBasisLinearModelDistribution(int n, IEnumerable<double> w, IDistribution<double> noise)
        {
            if (w.Count() != n) throw new ArgumentException();
            N = n;
            W = new List<double>(w);
            Noise = noise;
        }

        /// <summary>
        /// y = phi_x * w + noise, noise ~ x
        /// </summary>
        public static double Generate(int nBasis, IEnumerable<double> w, IDistribution<double> noise, double x)
        {
            if (w.Count() != nBasis) throw new ArgumentException();

            // 1 * nBasis
            var phiX = Utilities.PolynomialBasisLinearModelUtility.GetDesignMatrix(nBasis, x);
            // nBasis * 1
            var wm = new Matrix(w, false);

            return (phiX * wm)[0, 0] + noise.Generate();
        }

        public static IEnumerable<double> Generate(int n, int nBasis, IEnumerable<double> w, IDistribution<double> noise, IEnumerable<double> x)
        {
            var list = new List<double>();
            var xList = new List<double>(x);

            for (int i = 0; i < n; ++i)
            {
                list.Add(Generate(nBasis, w, noise, xList[i]));
            }

            return list;
        }

        public double Generate(double x)
        {
            return Generate(N, W, Noise, x);
        }

        public IEnumerable<double> Generate(int n, IEnumerable<double> x)
        {
            return Generate(n, N, W, Noise, x);
        }
    }
}
