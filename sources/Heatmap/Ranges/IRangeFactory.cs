using System.Collections.Generic;

namespace Heatmap.Ranges
{
    public interface IRangeFactory
    {
        IRange Create(IEnumerable<double> values);
    }
}
