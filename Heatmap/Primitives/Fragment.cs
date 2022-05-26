using System.Numerics;

namespace Heatmap.Primitives
{
    public readonly struct Fragment
    {
        public Vector2 Position { get; }
        public Vector2 Size { get; }
        public float Value { get; }

        public Fragment(Vector2 position, Vector2 size, float value)
        {
            Position = position;
            Size = size;
            Value = value;
        }
    }
}
