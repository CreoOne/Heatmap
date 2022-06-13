using System.Numerics;

namespace Heatmap.Primitives
{
    public readonly struct PositionedSample
    {
        public Vector2 Position { get; }
        public Vector2 Size { get; }
        public double Value { get; }

        public PositionedSample(Vector2 position, Vector2 size, double value)
        {
            Position = position;
            Size = size;
            Value = value;
        }
    }
}
