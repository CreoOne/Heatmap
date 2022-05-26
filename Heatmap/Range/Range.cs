using System;

namespace Heatmap.Range
{
    public abstract class Range : IRange
    {
        private float Min { get; }
        private float Max { get; }
        private float Length { get; }

        public Range(float min, float max)
        {
            Min = min;
            Max = max;
            Length = Max - Min;
        }

        public float GetValue(float value) => Clamp((value - Min) / Length, Min, Max);

        private float Clamp(float value, float min, float max) => MathF.Max(min, MathF.Min(value, max));
    }
}
