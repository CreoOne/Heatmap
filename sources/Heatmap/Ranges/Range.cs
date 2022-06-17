using Heatmap.Extensions;
using System;

namespace Heatmap.Ranges
{
    public abstract class Range : IRange
    {
        private double Min { get; }
        private double Length { get; }

        public Range(double min, double max)
        {
            Min = Math.Min(min, max);
            Length = Math.Max(min, max) - Min;
        }

        public double GetValue(double value) => ((value - Min) / Length).Clamp(0d, 1d);
    }
}
