using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLib
{
    public interface ITrainer<X, Y>
    {
        void Train(List<(X, Y)> data);
        Y Predict(X data);
        IEnumerable<Y> Predict(IEnumerable<X> data);
        double Error((X, Y) data);
        double Error(List<(X, Y)> data);
        void PrintModel();
    }
}
