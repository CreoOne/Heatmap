using System.Collections.Generic;

namespace Heatmap.Range
{
    public class AdaptiveRangeFactory : IRangeFactory
    {
        public IRange Create(IEnumerable<float> values) => new AdaptiveRange(values);
    }
}
