using System;
using System.Collections.Generic;
using System.Text;

namespace HW2
{
    public interface IDistribution
    {
        double PDFAt(double value);
        double PDFLogAt(double value);
    }
}
