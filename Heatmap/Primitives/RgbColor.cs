using System;

namespace Heatmap.Primitives
{
    public readonly struct RgbColor
    {
        public byte Red { get; }
        public byte Green { get; }
        public byte Blue { get; }

        public RgbColor(byte red, byte green, byte blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }

        public static RgbColor Lerp(RgbColor q, RgbColor r, float position) => new(
                Lerp(q.Red, r.Red, position),
                Lerp(q.Green, r.Green, position),
                Lerp(q.Blue, r.Blue, position)
            );

        private static byte Lerp(byte q, byte r, float position) => (byte)MathF.Round(q + (r - q) * position);

        public override string ToString() => $"[Red:{Red} Green:{Green} Blue:{Blue}]";
    }
}
