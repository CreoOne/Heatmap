using Heatmap.Primitives;
using System;

namespace Heatmap.Gradients
{
    public struct PositionedColor : IComparable<PositionedColor>
    {
        public float Position { get; }
        public RgbColor Color { get; }

        public PositionedColor(float position, RgbColor color)
        {
            Position = position;
            Color = color;
        }

        public int CompareTo(PositionedColor other) => Position.CompareTo(other.Position);
    }
}
