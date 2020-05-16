using System;
using System.Drawing;

namespace Heatmap.Gradients
{
    public struct PositionedColor : IComparable<PositionedColor>
    {
        public float Position { get; }
        public Color Color { get; }

        public PositionedColor(float position, Color color)
        {
            Position = position;
            Color = color;
        }

        public int CompareTo(PositionedColor other)
        {
            return Position.CompareTo(other.Color);
        }
    }
}
