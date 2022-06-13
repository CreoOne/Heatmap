using Heatmap.Primitives;
using Heatmap.Receivers;
using SkiaSharp;
using System.Collections.Concurrent;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Heatmap.SkiaSharp.Receivers
{
    public sealed class SkiaSharpReceiver : IReceiver
    {
        private ConcurrentBag<Fragment> Fragments { get; } = new();

        public void Receive(Fragment fragment) => Fragments.Add(fragment);

        public Task<Stream> GetPngStreamAsync(Resolution resolution)
        {
            using SKBitmap bitmap = new(resolution.Width, resolution.Height, SKColorType.Rgba8888, SKAlphaType.Unpremul);

            foreach (var fragment in Fragments)
            {
                var basePosition = fragment.Position * resolution;
                var pixelSize = fragment.Size * resolution;

                for (var offsetX = 0; offsetX < Math.Ceiling(pixelSize.X); offsetX++)
                    for (var offsetY = 0; offsetY < Math.Ceiling(pixelSize.Y); offsetY++)
                    {
                        var offset = new Vector2(offsetX, offsetY);
                        var shifted = basePosition + offset;

                        var shiftedX = (int)shifted.X;
                        var shiftedY = (int)shifted.Y;

                        if (shiftedX >= 0 && shiftedY >= 0 && shiftedX < resolution.Width && shiftedY < resolution.Height)
                            bitmap.SetPixel(shiftedX, shiftedY, ConvertColor(fragment.Color));
                    }
            }

            var data = bitmap.Encode(SKEncodedImageFormat.Png, 100);
            return Task.FromResult(data.AsStream(true));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        //private static SKColor ConvertColor(RgbColor color) => (uint)((/*color.Alpha*/255 << 24) | (color.Blue << 16) | (color.Green << 8) | color.Red);
        private static SKColor ConvertColor(RgbColor color) => new(color.Red, color.Green, color.Blue);
    }
}
