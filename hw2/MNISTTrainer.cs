using HW2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HW2
{
    public class MNISTTrainer
    {
        public MNISTTrainer(IEnumerable<Image> trainData)
        {
            _data = trainData;
        }

        private IEnumerable<Image> _data;

        public int Predict(Image image)
        {
            double[] posterior = CalculatePosterior(image);

            return posterior.ToList().IndexOf(posterior.Max());
        }

        public double[] CalculatePosterior(Image image)
        {
            double[] posterior = new double[10];
            for (int i = 0; i < 10; ++i)
            {
                posterior[i] = 0.0;
            }

            double likelyhoodMarginal = Math.Log(GetMarginal(image));

            for (int i = 0; i < 10; ++i)
            {
                var category = _data.Where(d => d.Label == i).ToList();

                int categoryCount = category.Count();
                if (categoryCount == 0)
                    categoryCount = 1;

                posterior[i] += Math.Log((double)categoryCount / _data.Count());
                int total = image.N * image.M;
                for (int j = 0; j < total; ++j)
                {
                    int n = category.Count(d => d.PixelBin[j] == image.PixelBin[j]);

                    if (n == 0)
                        n = 1;

                    posterior[i] -= Math.Log((double)n / category.Count());
                }
                posterior[i] -= likelyhoodMarginal;
            }

            return posterior;
        }

        private double GetMarginal(Image image)
        {
            int n = _data.Count(d => Image.FeatureEqual(d, image));

            if (n == 0)
                n = 1;

            return (double)n / _data.Count();
        }
    }
}
