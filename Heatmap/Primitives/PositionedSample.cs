using System.Numerics;

namespace Heatmap.Primitives
{
    public readonly struct PositionedSample
    {
        public Vector2 Position { get; }
        public Vector2 Size { get; }
        public float Value { get; }

        public PositionedSample(Vector2 position, Vector2 size, float value)
        {
            Position = position;
            Size = size;
            Value = value;
        }
    }
}
