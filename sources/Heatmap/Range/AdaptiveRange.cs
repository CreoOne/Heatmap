using System.Collections.Generic;
using System.Linq;

namespace Heatmap.Range
{
    public sealed class AdaptiveRange : Range
    {
        public AdaptiveRange(IEnumerable<double> values) : base(values.Min(), values.Max()) { }
    }
}
