using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Heatmap.Extensions
{
    public static class Vector2CeilingExtension
    {
        public static Vector2 Ceiling(this Vector2 self)
        {
            return new Vector2((float)Math.Ceiling(self.X), (float)Math.Ceiling(self.Y));
        }
    }
}
