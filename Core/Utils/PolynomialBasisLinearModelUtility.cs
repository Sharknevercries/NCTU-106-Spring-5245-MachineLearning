using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Utils
{
    public static class PolynomialBasisLinearModelUtility
    {
        public static Matrix GetDesignMatrix(int n, double x)
        {
            return GetDesignMatrix(n, new List<double>() { x });
        }

        public static Matrix GetDesignMatrix(int n, IEnumerable<double> x)
        {
            var t = x.ToList();
            Matrix m = new Matrix(t.Count, n);

            for (int i = 0; i < m.N; ++i)
            {
                for (int j = 0; j < m.M; ++j)
                {
                    m[i, j] = Math.Pow(t[i], j);
                }
            }

            return m;
        }
    }
}
