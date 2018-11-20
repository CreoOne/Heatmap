using System;
using System.Drawing;
using System.Linq;

namespace Heatmap.Morphs
{
    public class CachedMorph : IMorph
    {
        private int CacheResolution;
        private Color[] Cache;

        public CachedMorph(int cacheResolution, IMorph morph)
        {
            CacheResolution = cacheResolution;
            Cache = GenerateCache(cacheResolution, morph);
        }

        private static Color[] GenerateCache(int cacheResolution, IMorph morph)
        {
            return Enumerable
                .Range(0, cacheResolution)
                .Select(i => morph.GetColor(i / (float)cacheResolution))
                .ToArray();
        }

        public Color GetColor(float position)
        {
            int key = (int)(Math.Max(0, Math.Min(CacheResolution - 1, position * (CacheResolution - 1))));

            return Cache[key];
        }
    }
}
