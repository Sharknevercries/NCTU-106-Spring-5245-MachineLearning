using Core.DataType;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HW2
{
    public class GaussianMNISTTrainer : MNISTTrainner
    {
        private IEnumerable<Image> _data;

        public GaussianMNISTTrainer(IEnumerable<Image> trainData)
        {
            Train(trainData);
        }

        /// <summary>
        /// [i, j] = p( pixel_i | label_j ) under gaussian
        /// </summary>
        private IDistribution[,] _likelyhood;
        private double[] _prior;

        public override int Predict(Image image)
        {
            return Predict(image, out var posterior);
        }

        public override int Predict(Image image, out double[] posterior)
        {
            posterior = new double[LabelCount];

            for (int i = 0; i < posterior.Length; ++i)
            {
                posterior[i] = 0.0;
            }

            for (int i = 0; i < posterior.Length; ++i)
            {
                posterior[i] += _prior[i];

                for (int j = 0; j < ImageSize; ++j)
                {
                    posterior[i] += _likelyhood[j, i].PDFLogAt(image.Pixel[j]);
                }
            }

            return posterior.ToList().IndexOf(posterior.Max());
        }

        public override void Train(IEnumerable<Image> data)
        {
            _data = data;
            _likelyhood = new IDistribution[ImageSize, LabelCount];
            _prior = new double[LabelCount];

            for (int i = 0; i < LabelCount; ++i)
            {
                _prior[i] = Math.Log(1.0 * Utils.PseudoCount(_data.Count(d => d.Label == i)) / _data.Count());

                for (int j = 0; j < ImageSize; ++j)
                {
                    var temp = _data.Where(d => d.Label == i).Select(d => (double)d.Pixel[j]).ToList();
                    
                    if (temp.Count == 0)
                    {
                        _likelyhood[j, i] = new UniformDistribution(0, 255);
                    }
                    else
                    {
                        _likelyhood[j, i] = new GaussianDistribution(temp);
                    }                    
                }
            }
        }
    }
}