using System.Numerics;

namespace Heatmap.Primitives
{
    public readonly struct Fragment
    {
        public Vector2 Position { get; }
        public Vector2 Size { get; }
        public RgbColor Color { get; }

        public Fragment(Vector2 position, Vector2 size, RgbColor color)
        {
            Position = position;
            Size = size;
            Color = color;
        }

        internal static Fragment FromPositionedSample(PositionedSample positionedSample, RgbColor color) => new(positionedSample.Position, positionedSample.Size, color);
    }
}
