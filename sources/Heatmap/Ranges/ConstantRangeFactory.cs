using System.Collections.Generic;

namespace Heatmap.Ranges
{
    public class ConstantRangeFactory : IRangeFactory
    {
        public double Min { get; }
        public double Max { get; }

        public ConstantRangeFactory(double min, double max)
        {
            Min = min;
            Max = max;
        }

        public IRange Create(IEnumerable<double> _) => new ConstantRange(Min, Max);
    }
}
