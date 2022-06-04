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
            throw new NotImplementedException();

            using SKBitmap bitmap = new(width, height, SKColorType.Rgba8888, SKAlphaType.Unpremul);
            IntPtr pixelsAddr = bitmap.GetPixels();

            unsafe
            {
                uint* ptr = (uint*)pixelsAddr.ToPointer();

                var color = new RgbColor(0, 0, 0);

                for (int y = 0; y < height; y++)
                    for (int x = 0; x < width; x++)
                        *ptr++ = ColorToUint(color);
            }

            var data = bitmap.Encode(SKEncodedImageFormat.Png, 100);
            return Task.FromResult(data.AsStream(true));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static uint ColorToUint(RgbColor color) => (uint)((/*color.Alpha*/255 << 24) | (color.Blue << 16) | (color.Green << 8) | color.Red);
    }
}
