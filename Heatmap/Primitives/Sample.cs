using System.Numerics;

namespace Heatmap.Primitives
{
    public readonly struct Sample
    {
        public Vector2 Position { get; }
        public float Value { get; }

        public Sample(Vector2 position, float value)
        {
            Position = position;
            Value = value;
        }
    }
}
