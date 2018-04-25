using Core.DataType;
using HW2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HW2
{
    public class FrequencyMNISTTrainer : MNISTTrainner
    {
        public FrequencyMNISTTrainer(IEnumerable<Image> trainData)
        {
            Train(trainData);
        }

        private IEnumerable<Image> _data;
        /// <summary>
        /// [i, j, k] = p( pixel_i = bin_j | label_k ) in log scale
        /// </summary>
        private double[,,] _likelyhood;
        /// <summary>
        /// [i] = p( label_i ) in log scale
        /// </summary>
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
                    posterior[i] += _likelyhood[j, image.PixelBin[j], i];
                }
            }

            return posterior.ToList().IndexOf(posterior.Max());
        }

        public override void Train(IEnumerable<Image> data)
        {
            _data = data;
            _prior = new double[LabelCount];
            _likelyhood = new double[ImageSize, BinCount, LabelCount];

            for (int i = 0; i < LabelCount; ++i)
            {
                var labelInUniverse = _data.Where(d => d.Label == i).ToList();

                _prior[i] = Math.Log((double)Utils.PseudoCount(labelInUniverse.Count()) / _data.Count());

                for (int j = 0; j < ImageSize; ++j)
                {
                    for (int k = 0; k < BinCount; ++k)
                    {
                        int pixelBinGivenLabelInUniverseCount = Utils.PseudoCount(labelInUniverse.Count(d => d.PixelBin[j] == k));

                        _likelyhood[j, k, i] = Math.Log((double)pixelBinGivenLabelInUniverseCount / labelInUniverse.Count());
                    }
                }
            }
        }
    }
}
