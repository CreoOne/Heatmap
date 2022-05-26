using Heatmap.Primitives;
using System.Collections.Generic;
using System.Linq;

namespace Heatmap.Gradients
{
    public class LinearGradient : IGradient
    {
        private const float MinColorEpsilon = 1 / 255f;
        private const float MaxColorEpsilon = 254 / 255f;
        private float PartSize;

        private PositionedColor[] Colors { get; }

        public LinearGradient(IEnumerable<PositionedColor> positionedColors)
        {
            Colors = positionedColors.OrderBy(positionedColor => positionedColor).ToArray();
        }

        public RgbColor GetColor(float position)
        {
            int index = (int)(position / PartSize);
            float offset = (position - index * PartSize) / PartSize;

            var first = Colors[index];

            if (offset < MinColorEpsilon)
                return first.Color;

            var second = Colors[index + 1];

            if (offset > MaxColorEpsilon)
                return second.Color;

            return Lerp(first.Color, second.Color, offset);
        }

        private static RgbColor Lerp(RgbColor q, RgbColor r, float position) => new(
                (byte)Lerp(q.Red, r.Red, position),
                (byte)Lerp(q.Green, r.Green, position),
                (byte)Lerp(q.Blue, r.Blue, position)
            );

        private static float Lerp(float q, float r, float position) => q + (r - q) * position;
    }
}
