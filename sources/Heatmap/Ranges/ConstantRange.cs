using System;

namespace Heatmap.Ranges
{
    public sealed class ConstantRange : Range
    {
        public ConstantRange(double min, double max) : base(Math.Min(min, max), Math.Max(min, max)) { }
    }
}
