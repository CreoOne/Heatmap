using Heatmap.Primitives;
using Heatmap.Receivers;
using SkiaSharp;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace Heatmap.SkiaSharp.Receivers
{
    public sealed class SkiaSharpReceiver : IReceiver
    {
        private ConcurrentBag<Fragment> Fragments { get; } = new();

        public void Receive(Fragment fragment) => Fragments.Add(fragment);

        public Task<Stream> GetPngStreamAsync(int width, int height)
        {
            using SKBitmap bitmap = new(width, height, SKColorType.Rgba8888, SKAlphaType.Unpremul);
            
            foreach (var fragment in Fragments)
            {
                var halfFragment = fragment.Size / 2f;
                var x = (int)((fragment.Position.X - halfFragment.X) * width);
                var y = (int)((fragment.Position.Y - halfFragment.Y) * height);

                var pixelsX = (int)Math.Ceiling(fragment.Size.X * width);
                var pixelsY = (int)Math.Ceiling(fragment.Size.Y * height);

                for (var offsetX = 0; offsetX < pixelsX; offsetX++)
                    for (var offsetY = 0; offsetY < pixelsY; offsetY++)
                        bitmap.SetPixel(x + offsetX, y + offsetY, ConvertColor(fragment.Color));
            }

            var data = bitmap.Encode(SKEncodedImageFormat.Png, 100);
            return Task.FromResult(data.AsStream(true));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        //private static SKColor ConvertColor(RgbColor color) => (uint)((/*color.Alpha*/255 << 24) | (color.Blue << 16) | (color.Green << 8) | color.Red);
        private static SKColor ConvertColor(RgbColor color) => new(color.Red, color.Green, color.Blue);
    }
}
