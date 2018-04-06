using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Distributions
{
    public class UnivariateGaussianDistribution : IDistribution<double>
    {
        public double Mean { get; private set; }
        public double Variance { get; private set; }
        public double Sigma
        {
            get
            {
                return Math.Sqrt(Variance);
            }
            set
            {
                Variance = Math.Pow(value, 2);
            }
        }

        private const double TwoPi = Math.PI * 2; 
        private static Random _rnd;
        private static bool _generated;
        private static double _value;

        static UnivariateGaussianDistribution()
        {
            _rnd = new Random();
        }

        public UnivariateGaussianDistribution(double mean, double variance)
        {
            Mean = mean;
            Variance = variance;
            _generated = false;
        }

        public double Generate()
        {
            return Generate(Mean, Variance);
        }

        public IEnumerable<double> Generate(int n)
        {
            return Generate(n, Mean, Variance);
        }

        /// <summary>
        /// Implement Box-Muller Transform
        /// </summary>
        public static double Generate(double mu, double variance)
        {
            _generated = !_generated;
            if (!_generated)
            {
                return _value * Math.Sqrt(variance) + mu;
            }

            double u1 = _rnd.NextDouble();
            double u2 = _rnd.NextDouble();
            double r = Math.Sqrt(-2.0 * Math.Log(u1));
            double theta = TwoPi * u2;
            double z0 = r * Math.Cos(theta);
            double z1 = r * Math.Sin(theta);

            _value = z1;

            return z0 * Math.Sqrt(variance) + mu;
        }

        public static IEnumerable<double> Generate(int n, double mu, double variance)
        {
            var list = new List<double>();

            for (int i = 0; i < n; ++i)
            {
                list.Add(Generate(mu, variance));
            }

            return list;
        }
    }
}
