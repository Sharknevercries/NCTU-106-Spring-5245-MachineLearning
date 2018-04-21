using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Interfaces
{
    public interface ITrainer<X, Y>
    {
        void Train(IEnumerable<(X, Y)> data);
        Y Predict(X data);
        IEnumerable<Y> Predict(IEnumerable<X> data);
        double Error((X, Y) data);
        double Error(IEnumerable<(X, Y)> data);
        void PrintModel();
    }
}
