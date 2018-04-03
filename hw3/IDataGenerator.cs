using System;
using System.Collections.Generic;
using System.Text;

namespace HW3
{
    public interface IDataGenerator<X>
    {
        X Generate();
        IEnumerable<X> Generate(int n);
    }
}
