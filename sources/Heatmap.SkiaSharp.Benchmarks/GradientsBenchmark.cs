using BenchmarkDotNet.Attributes;
using Heatmap.Gradients;
using Heatmap.Primitives;
using Heatmap.Samplers;
using Heatmap.SkiaSharp.Receivers;
using System.Numerics;

namespace Heatmap.SkiaSharp.Benchmarks
{
    public class GradientsBenchmark
    {
        private SkiaSharpReceiver? Receiver { get; set; }

        [IterationSetup]
        public async void IterationSetup()
        {
            static double Func(Vector2 position) => 10 * 2 + Math.Pow(position.X, 2) + Math.Pow(position.Y, 2) - 10 * Math.Cos(2 * Math.PI * position.X) - 10 * Math.Cos(2 * Math.PI * position.Y);

            var sampler = new LambdaSampler(Func);
            Receiver = new SkiaSharpReceiver();

            var gradient = new PreCachedGradient(new LinearGradient(
                new RgbColor(68, 1, 84),
                new RgbColor(65, 68, 135),
                new RgbColor(42, 120, 142),
                new RgbColor(34, 168, 132),
                new RgbColor(122, 209, 81),
                new RgbColor(253, 231, 37)
            ));

            await new DefaultHeatmapBuilder()
                    .SetSampler(sampler)
                    .SetReceiver(Receiver)
                    .SetViewport(Viewport.FromTwoPoints(new Vector2(-5.12f), new Vector2(5.12f)))
                    .SetSamplingResolution(new Resolution(800, 800))
                    .SetGradient(gradient)
                    .GenerateAsync();
        }

        [Benchmark]
        public async Task<Stream> GetPngStreamAsync()
        {
            using var stream = await Receiver!.GetPngStreamAsync(new Resolution(1000, 1000));
            return stream;
        }
    }
}
