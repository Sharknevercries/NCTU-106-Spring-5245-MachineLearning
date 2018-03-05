using CoreLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitTest
{
    public partial class PolynomialRegressionTrainerTest
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
        }

        public static IEnumerable<object[]> GetTrainDataset()
        {
            return GetDataset().Select(d => new object[] { d[0], d[1], d[2], d[3] });
        }
    }
}
