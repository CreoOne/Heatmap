using System.Collections.Generic;

namespace Heatmap.Range
{
    public interface IRangeFactory
    {
        IRange Create(IEnumerable<double> values);
    }
}
