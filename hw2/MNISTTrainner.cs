using System;
using System.Collections.Generic;
using System.Text;

namespace HW2
{
    public abstract class MNISTTrainner : ITrainer<Image, int>
    {
        protected const int ImageSize = 28 * 28;
        protected const int LabelCount = 10;
        protected const int BinCount = 32;

        public abstract int Predict(Image image);
        public abstract int Predict(Image image, out double[] posterior);
        public abstract void Train(IEnumerable<Image> data);
    }
}
