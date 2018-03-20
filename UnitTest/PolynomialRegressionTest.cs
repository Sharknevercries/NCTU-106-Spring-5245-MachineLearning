using HW1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace UnitTest
{
    public partial class PolynomialRegressionTest
    {
        [Theory]
        [MemberData(nameof(GetPolynomialRegressionDataset))]
        public void Test(int n, double lambda, List<(double, double)> trainingData, List<(double, double)> testData)
        {
            var model = new PolynomialRegressionTrainer(n, lambda);

            model.Train(trainingData);

            foreach (var d in testData)
            {
                Assert.Equal(d.Item2, model.Predict(d.Item1), 9);
            }
        }

        [Theory]
        [MemberData(nameof(GetNewtonMethodDataset))]
        public void NewtonMethod(int n, List<(double, double)> trainingData, List<(double, double)> testData)
        {
            var model = new NewtonMethodTrainer(n);

            model.Train(trainingData);

            foreach(var d in testData)
            {
                Assert.Equal(d.Item2, model.Predict(d.Item1), 9);
            }
        }
    }
}
