using System.Collections.Generic;

namespace Heatmap.Range
{
    public class AdaptiveRangeFactory : IRangeFactory
    {
        public IRange Create(IEnumerable<double> values) => new AdaptiveRange(values);
    }
}
