using System;
using System.Numerics;

namespace Heatmap.Primitives
{
    public readonly struct Viewport
    {
        public Vector2 Min { get; }
        public Vector2 Max { get; }

        private Viewport(Vector2 min, Vector2 max)
        {
            Min = min;
            Max = max;
        }

        public Vector2 GetViewPoint(Vector2 unitPosition) => Min + (Max - Min) * unitPosition;

        public static Viewport FromTwoPoints(Vector2 from, Vector2 to)
        {
            var min = new Vector2(Math.Min(from.X, to.X), Math.Min(from.Y, to.Y));
            var max = new Vector2(Math.Max(from.X, to.X), Math.Max(from.Y, to.Y));
            return new Viewport(min, max);
        }

        public static Viewport FromCenterAndSize(Vector2 center, Vector2 size)
        {
            var halfSize = size / 2f;
            var min = center - halfSize;
            var max = center + halfSize;
            return new Viewport(min, max);
        }
    }
}
