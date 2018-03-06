using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLib
{
    public interface Trainer<X, Y>
    {
        void Train(List<(X, Y)> data);
        Y Predict(X data);
        IEnumerable<Y> Predict(IEnumerable<X> data);
    }
}
