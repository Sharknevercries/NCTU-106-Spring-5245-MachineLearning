using System;

namespace HW2
{
    public class UniformDistribution : IDistribution
    {
        public double A { get; private set; }
        public double B { get; private set; }

        public UniformDistribution(double a, double b)
        {
            A = a;
            B = b;
        }

        public double PDFAt(double value)
        {
            return 1.0 / (B - A);
        }

        public double PDFLogAt(double value)
        {
            return Math.Log(PDFAt(value));
        }
    }
}