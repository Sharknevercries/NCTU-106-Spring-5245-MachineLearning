using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HW2
{
    public class GaussianDistribution : IDistribution
    {
        public double Mu { get; private set; }
        public double Variance { get; private set; }
        
        public GaussianDistribution(IEnumerable<double> data)
        {
            Mu = data.Average();
            Variance = data.Average(d => Math.Pow(d - Mu, 2));
        }

        public double PDFAt(double value)
        {
            if (Variance == 0)
            {
                if (Mu == value)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 1.0 / Math.Sqrt(2.0 * Math.PI * Variance) * Math.Exp(-1.0 / 2.0 * (Math.Pow(value - Mu, 2) / Variance));
            }            
        }

        public double PDFLogAt(double value)
        {
            if (Variance == 0)
            {
                if (Mu == value)
                {
                    return Math.Log(1);
                }
                else
                {
                    return Math.Log(1e-9);
                }
            }
            else
            {
                return Math.Log(1.0 / Math.Sqrt(2.0 * Math.PI * Variance)) - (1.0 / 2.0 * (Math.Pow(value - Mu, 2) / Variance));
            }
        }
    }
}
