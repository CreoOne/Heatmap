using System;

namespace Heatmap.Range
{
    public sealed class ConstantRange : Range
    {
        public ConstantRange(float min, float max) : base(Math.Min(min, max), Math.Max(min, max)) { }
    }
}
