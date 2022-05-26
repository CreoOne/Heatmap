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
    }
}
