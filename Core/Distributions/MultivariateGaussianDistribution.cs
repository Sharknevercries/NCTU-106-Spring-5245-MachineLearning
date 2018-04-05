using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Distributions
{
    public class MultivariateGaussianDistribution : IDistribution<IEnumerable<double>>
    {
        public Matrix Mean { get; private set; }
        public Matrix Precision { get; private set; }

        public MultivariateGaussianDistribution(IEnumerable<double> mean, Matrix precision) : this(new Matrix(mean, false), precision)
        {
        }

        public MultivariateGaussianDistribution(Matrix mean, Matrix precision)
        {
            if (mean.M != 1 || mean.N == 0) throw new ArgumentException();
            Mean = new Matrix(mean);
            Precision = new Matrix(precision);
        }

        public IEnumerable<double> Generate()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IEnumerable<double>> Generate(int n)
        {
            throw new NotImplementedException();
        }

        public void PrintInfo()
        {
            Console.WriteLine("Mean:");
            Mean.PrettyPrint();
            Console.WriteLine("Precision:");
            Precision.PrettyPrint();
        }
    }
}
