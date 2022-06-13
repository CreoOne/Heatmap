using System;

namespace Heatmap.Extensions
{
    public static class ClampExtension
    {
        public static TTarget Clamp<TTarget>(this TTarget self, TTarget min, TTarget max) where TTarget : IComparable<TTarget>
        {
            if (self.CompareTo(min) > 0)
            {
                if (self.CompareTo(max) < 0)
                    return self;

                else
                    return max;
            }

            return min;
        }
    }
}
