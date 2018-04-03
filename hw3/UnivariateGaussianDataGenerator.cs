using System;
using System.Collections.Generic;
using System.Text;

namespace HW3
{
    public class UnivariateGaussianDataGenerator : IDataGenerator<double>
    {
        public double Mu { get; private set; }
        public double Sigma { get; private set; }

        private const double TwoPi = Math.PI * 2; 
        private static Random _rnd;
        private static bool _generated;
        private static double _value;

        static UnivariateGaussianDataGenerator()
        {
            _rnd = new Random();
        }

        public UnivariateGaussianDataGenerator(double mu, double sigma)
        {
            Mu = mu;
            Sigma = sigma;
            _generated = false;
        }

        public double Generate()
        {
            return Generate(Mu, Sigma);
        }

        public IEnumerable<double> Generate(int n)
        {
            return Generate(n, Mu, Sigma);
        }

        /// <summary>
        /// Implement Box-Muller Transform
        /// </summary>
        public static double Generate(double mu, double sigma)
        {
            _generated = !_generated;
            if (!_generated)
            {
                return _value * sigma + mu;
            }

            double u1 = _rnd.NextDouble();
            double u2 = _rnd.NextDouble();
            double r = Math.Sqrt(-2.0 * Math.Log(u1));
            double theta = TwoPi * u2;
            double z0 = r * Math.Cos(theta);
            double z1 = r * Math.Sin(theta);

            _value = z1;

            return z0 * sigma + mu;
        }

        public static IEnumerable<double> Generate(int n, double mu, double sigma)
        {
            var list = new List<double>();

            for (int i = 0; i < n; ++i)
            {
                list.Add(Generate(mu, sigma));
            }

            return list;
        }
    }
}
