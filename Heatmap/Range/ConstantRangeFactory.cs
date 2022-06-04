using System.Collections.Generic;

namespace Heatmap.Range
{
    public class ConstantRangeFactory : IRangeFactory
    {
        public float Min { get; }
        public float Max{ get; }

        public ConstantRangeFactory(float min, float max)
        {
            Min = min;
            Max = max;
        }

        public IRange Create(IEnumerable<float> _) => new ConstantRange(Min, Max);
    }
}
