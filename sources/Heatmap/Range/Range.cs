using Heatmap.Extensions;

namespace Heatmap.Range
{
    public abstract class Range : IRange
    {
        private double Min { get; }
        private double Max { get; }
        private double Length { get; }

        public Range(double min, double max)
        {
            Min = min;
            Max = max;
            Length = Max - Min;
        }

        public double GetValue(double value) => ((value - Min) / Length).Clamp(Min, Max);
    }
}
