using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLib
{
    public interface Trainer
    {
        void Train(List<(double, double)> data);
    }
}
