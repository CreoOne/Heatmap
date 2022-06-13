using System.Drawing;
using System.Numerics;

namespace Heatmap.Extensions
{
    public static class SizeToVector2Extension
    {
        public static Vector2 ToVector2(this Size self) => new Vector2(self.Width, self.Height);
    }
}
