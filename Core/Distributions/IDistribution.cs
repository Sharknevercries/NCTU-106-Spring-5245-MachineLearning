using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Distributions
{
    public interface IDistribution<X>
    {
        X Generate();
        IEnumerable<X> Generate(int n);
    }
}
