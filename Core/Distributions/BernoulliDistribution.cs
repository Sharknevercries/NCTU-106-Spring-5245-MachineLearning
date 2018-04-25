using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Distributions
{
    public class BernoulliDistribution : IDistribution<int>
    {
        public double Mu { get; set; }

        public BernoulliDistribution() : this(0.5)
        {
        }

        public BernoulliDistribution(double mu)
        {
            Mu = mu;
        }

        public int Generate()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<int> Generate(int n)
        {
            throw new NotImplementedException();
        }
    }
}
