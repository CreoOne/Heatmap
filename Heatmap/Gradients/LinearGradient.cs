using Heatmap.Extensions;
using Heatmap.Primitives;
using System;

namespace Heatmap.Gradients
{
    public class LinearGradient : IGradient
    {
        private const float MinColorEpsilon = 1 / 255f;
        private const float MaxColorEpsilon = 254 / 255f;
        private readonly float PartSize;

        private RgbColor[] Colors { get; }

        public LinearGradient(params RgbColor[] colors)
        {
            if (colors.Length < 2)
                throw new ArgumentException($"Not enough colors, at least two expected, {colors.Length} given", nameof(colors));

            Colors = colors;
            PartSize = 1f / (Colors.Length - 1);
        }

        public RgbColor GetColor(float position)
        {
            position = position.Clamp(0, 1);

            int index = (int)(position / PartSize);
            float offset = (position - index * PartSize) / PartSize;

            var first = Colors[index];

            if (offset < MinColorEpsilon)
                return first;

            var second = Colors[index + 1];

            if (offset > MaxColorEpsilon)
                return second;

            return RgbColor.Lerp(first, second, offset);
        }
    }
}
