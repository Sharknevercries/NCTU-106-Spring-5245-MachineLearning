﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace HW2
{
    public class BetaDistribution : IDistribution
    {
        public BetaDistribution() : this(1, 1)
        {
        }

        public BetaDistribution(int alpha, int beta)
        {
            if (alpha <= 0 || beta <= 0) throw new ArgumentException();
            Alpha = alpha;
            Beta = beta;
        }

        public int Alpha { get; private set; }
        public int Beta { get; private set; }

        public double PDFAt(double value)
        {
            if (value > 1 || value < 0) throw new ArgumentException();
            return Math.Pow(value, Alpha - 1) * Math.Pow(1 - value, Beta - 1) * ComputeGammaFunction(Alpha, Beta);
        }

        public double PDFLogAt(double value)
        {
            throw new NotImplementedException();
        }

        public double Mode()
        {
            return (double)(Alpha - 1) / (Alpha + Beta - 2);
        }

        /// <summary>
        /// Compute beta-distribution latter part
        /// </summary>
        /// <param name="alpha"></param>
        /// <param name="beta"></param>
        /// <returns>gamma(alpha + beta) / (gamma(alpha) * gamma(beta))</returns>
        public double ComputeGammaFunction(int alpha, int beta)
        {
            // TODO: This computation is not accurate.
            double ret = 1;

            for (int i = alpha + beta - 1; i >= 2; --i)
                ret *= i;

            for (int i = alpha - 1; i >= 2; --i)
                ret /= i;

            for (int i = beta - 1; i >= 2; --i)
                ret /= i;

            return ret;
        }

        public void Summary()
        {
            Console.WriteLine($"Alpha = { Alpha }, Beta = { Beta }");
            Console.WriteLine($"Mode = { Mode() }, pdf = { PDFAt(Mode()) }");
        }
    }
}
