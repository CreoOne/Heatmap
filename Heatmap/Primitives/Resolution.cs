using System.Numerics;

namespace Heatmap.Primitives
{
    public readonly struct Resolution
    {
        public int Width { get; }
        public int Height { get; }

        public Resolution(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public static implicit operator Vector2(Resolution resolution) => new(resolution.Width, resolution.Height);
    }
}
