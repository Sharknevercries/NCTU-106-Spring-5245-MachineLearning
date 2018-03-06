using CoreLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitTest
{
    public partial class PolynomialRegressionTest
    {
        public static IEnumerable<object[]> GetDataset()
        {
            yield return new object[]
            {
                1,
                0.0,
                new List<(double, double)>()
                {
                    (0.0, 1.0),
                    (1.0, 3.0),
                    (2.0, 5.0),
                    (3.0, 7.0),
                },
                new List<(double, double)>()
                {
                    (0, 1),
                    (2, 5),
                    (9, 19),
                },
            };
            yield return new object[]
            {
                2,
                0.0,
                new List<(double, double)>()
                {
                    (0.0, 1.0),
                    (1.0, 11.0),
                    (2.0, 27.0),
                    (3.0, 49.0),
                },
                new List<(double, double)>()
                {
                    (0.0, 1.0),
                    (2.0, 27.0),
                    (9.0, 307.0),
                },
            };
        }

        public static IEnumerable<object[]> GetPolynomialRegressionDataset()
        {
            return GetDataset().Select(d => new object[] { d[0], d[1], d[2], d[3] });
        }

        public static IEnumerable<object[]> GetNewtonMethodDataset()
        {
            return GetDataset().Select(d => new object[] { d[0], d[2], d[3] });
        }
    }
}
