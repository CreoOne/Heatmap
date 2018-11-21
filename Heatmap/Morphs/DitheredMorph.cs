using System;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;

namespace Heatmap.Morphs
{
    public class DitheredMorph : IMorph
    {
        private Color[] Palette;
        private RandomNumberGenerator Random;
        private float PartSize;

        public DitheredMorph(RandomNumberGenerator randomNumberGenerator, params Color[] palette)
        {
            Palette = palette ?? throw new ArgumentNullException(nameof(palette));
            Random = randomNumberGenerator ?? throw new ArgumentNullException(nameof(randomNumberGenerator));

            PartSize = 1 / (float)(palette.Length - 1);
        }

        public Color GetColor(float position)
        {
            int index = (int)(position / PartSize);
            float offset = (position - index * PartSize) / PartSize;

            if(GetRandomFloat() >= offset)
                return Palette[index];

            return Palette[index + 1];
        }

        private float GetRandomFloat()
        {
            byte[] bytes = new byte[4];
            Random.GetBytes(bytes);

            ushort number = (ushort)bytes
                .Select((b, i) => b << (i * 8))
                .DefaultIfEmpty()
                .Sum();
            
            return number / (float)ushort.MaxValue;
        }
    }
}
