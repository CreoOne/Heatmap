using System;
using System.Numerics;

namespace Heatmap.Primitives
{
    public readonly struct Viewport
    {
        public Vector2 From { get; }
        public Vector2 To { get; }

        public Viewport(Vector2 from, Vector2 to)
        {
            From = new Vector2(Math.Min(from.X, to.X), Math.Min(from.Y, to.Y));
            To = new Vector2(Math.Max(from.X, to.X), Math.Max(from.Y, to.Y));
        }
    }
}
