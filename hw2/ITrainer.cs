using System;
using System.Collections.Generic;
using System.Text;

namespace HW2
{
    public interface ITrainer<X, Y>
    {
        void Train(IEnumerable<X> data);
        Y Predict(X image);
        Y Predict(X image, out double[] posterior);
    }
}
