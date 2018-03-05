using CoreLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace UnitTest
{
    public partial class PolynomialRegressionTrainerTest
    {
        [Theory]
        [MemberData(nameof(GetTrainDataset))]
        public void Test(int n, double lambda, List<(double, double)> trainingData, List<(double, double)> testData)
        {
            var model = new PolynomialRegressionTrainer(n, lambda);

            model.Train(trainingData);

            Assert.Equal(testData.Select(d => d.Item2).ToList(), model.Predict(testData.Select(d => d.Item1).ToList()));
        } 
    }
}
