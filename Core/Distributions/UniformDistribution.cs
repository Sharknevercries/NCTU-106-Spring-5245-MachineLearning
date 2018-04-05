using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Distributions
{
    public class UniformDistribution : IDistribution<double>
    {
        public double From { get; private set; }
        public double To { get; private set; }
        private static Random _rnd;

        static UniformDistribution()
        {
            _rnd = new Random();
        }

        public UniformDistribution(double from, double to)
        {
            if (from > to) throw new ArgumentException();
            From = from;
            To = to;
        }

        public static double Generate(double from, double to)
        {
            return _rnd.NextDouble() * (to - from) + from;
        }

        public static IEnumerable<double> Generate(int n, double from, double to)
        {
            var list = new List<double>();

            for (int i = 0; i < n; ++i)
            {
                list.Add(Generate(from, to));
            }

            return list;
        }

        public double Generate()
        {
            return Generate(From, To);
        }

        public IEnumerable<double> Generate(int n)
        {
            return Generate(n, From, To);
        }
    }
}
