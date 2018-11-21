using System.Drawing;
using System.Numerics;

namespace Heatmap.Extensions
{
    public static class Vector2ToSizeExtension
    {
        public static Size ToSize(this Vector2 self)
        {
            return new Size((int)self.X, (int)self.Y);
        }
    }
}
