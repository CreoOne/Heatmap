using System.Numerics;

namespace Heatmap.Primitives
{
    public readonly struct Sample
    {
        public Vector2 Position { get; }
        public double Value { get; }

        public Sample(Vector2 position, double value)
        {
            Position = position;
            Value = value;
        }
    }
}
