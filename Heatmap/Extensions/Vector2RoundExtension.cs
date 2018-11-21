using System;
using System.Numerics;

namespace Heatmap.Extensions
{
    public static class Vector2RoundExtension
    {
        public static Vector2 Round(this Vector2 self)
        {
            return new Vector2((float)Math.Round(self.X), (float)Math.Round(self.Y));
        }
    }
}
