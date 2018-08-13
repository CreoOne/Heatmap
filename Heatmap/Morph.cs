using System;
using System.Drawing;
using System.Numerics;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Heatmap
{
    public class Morph
    {
        private const float MinColorEpsilon = 1 / 255f;
        private const float MaxColorEpsilon = 254 / 255f;
        private Color[] Palette;
        private float PartSize;

        private const int CacheResolution = 1000;
        private Dictionary<int, Color> Cache = new Dictionary<int, Color>(CacheResolution);

        public Morph(params Color[] palette)
        {
            Palette = palette ?? throw new ArgumentNullException(nameof(palette));

            if (palette.Length < 2)
                throw new ArgumentException();

            PartSize = 1 / (float)(palette.Length - 1);
        }

        public Color GetColor(float position)
        {
            int key = (int)(Math.Max(0, Math.Min(1, position)) * CacheResolution);

            if (Cache.ContainsKey(key))
                return Cache[key];

            int index = (int)(position / PartSize);
            float offset = (position - index * PartSize) / PartSize;

            Color first = Palette[index];

            if (offset < MinColorEpsilon)
                return Add(key, first);

            Color second = Palette[index + 1];

            if (offset > MaxColorEpsilon)
                return Add(key, second);

            return Add(key, Lerp(first, second, offset));
        }

        private Color Add(int key, Color color)
        {
            if (Cache.ContainsKey(key))
                return color;

            Cache.Add(key, color);
            return color;
        }

        private Color Lerp(Color q, Color r, float position)
        {
            return Color.FromArgb(
                (int)Lerp(q.A, r.A, position),
                (int)Lerp(q.R, r.R, position),
                (int)Lerp(q.G, r.G, position),
                (int)Lerp(q.B, r.B, position)
            );
        }

        private float Lerp(float q, float r, float position)
        {
            return q + (r - q) * position;
        }
    }
}
