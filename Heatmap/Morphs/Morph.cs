using System;
using System.Drawing;

namespace Heatmap.Morphs
{
    public sealed class Morph : IMorph
    {
        private const float MinColorEpsilon = 1 / 255f;
        private const float MaxColorEpsilon = 254 / 255f;
        private Color[] Palette;
        private float PartSize;

        public Morph(params Color[] palette)
        {
            Palette = palette ?? throw new ArgumentNullException(nameof(palette));

            if (palette.Length < 2)
                throw new ArgumentException();

            PartSize = 1 / (float)(palette.Length - 1);
        }

        public Color GetColor(float position)
        {
            int index = (int)(position / PartSize);
            float offset = (position - index * PartSize) / PartSize;

            Color first = Palette[index];

            if (offset < MinColorEpsilon)
                return first;

            Color second = Palette[index + 1];

            if (offset > MaxColorEpsilon)
                return second;

            return Lerp(first, second, offset);
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
