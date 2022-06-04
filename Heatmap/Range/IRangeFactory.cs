using System.Collections.Generic;

namespace Heatmap.Range
{
    public interface IRangeFactory
    {
        IRange Create(IEnumerable<float> values);
    }
}
