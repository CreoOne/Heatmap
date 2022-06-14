using Heatmap.Extensions;
using Heatmap.Primitives;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Heatmap.Gradients
{
    public sealed class PreCachedGradient : IGradient
    {
        private IGradient Gradient { get; }
        private RgbColor[] Cache { get; }
        private uint Resolution { get; }

        public PreCachedGradient(IGradient gradient, uint resolution = 255)
        {
            Gradient = gradient;
            Resolution = resolution;
            Cache = GenerateCache(gradient, resolution);
        }

        private static RgbColor[] GenerateCache(IGradient gradient, uint resolution)
        {
            var result = new RgbColor[resolution];

            for(int index = 0; index < resolution; index++)
                result[index] = gradient.GetColor(index / (double)resolution);

            return result;
        }

        public RgbColor GetColor(double position) => Cache[(uint)(position.Clamp(0, 1) * Resolution)];

        public override string ToString() => $"PreCached {Gradient}";
    }
}
