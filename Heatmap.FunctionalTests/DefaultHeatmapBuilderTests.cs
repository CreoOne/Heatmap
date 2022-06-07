using Heatmap.Gradients;
using Heatmap.Primitives;
using Heatmap.Samplers;
using Heatmap.SkiaSharp.Receivers;
using System.Numerics;
using Xunit.Abstractions;

namespace Heatmap.FunctionalTests
{
    public class DefaultHeatmapBuilderTests
    {
        public ITestOutputHelper TestOutputHelper { get; }

        public DefaultHeatmapBuilderTests(ITestOutputHelper testOutputHelper)
        {
            TestOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task GivenRastriginFunctionWhenGetPngStreamAsyncThenCorrectImageOutput()
        {
            // Rastrigin
            // https://en.wikipedia.org/wiki/Rastrigin_function
            static double Func(Vector2 position) => 10 * 2 + Math.Pow(position.X, 2) + Math.Pow(position.Y, 2) - 10 * Math.Cos(2 * Math.PI * position.X) - 10 * Math.Cos(2 * Math.PI * position.Y);

            var sampler = new LambdaSampler(Func);
            var receiver = new SkiaSharpReceiver();

            // Viridis
            // https://bids.berkeley.edu/events/better-default-colormap-matplotlib
            var gradient = new LinearGradient(
                new RgbColor(68, 1, 84),
                new RgbColor(65, 68, 135),
                new RgbColor(42, 120, 142),
                new RgbColor(34, 168, 132),
                new RgbColor(122, 209, 81),
                new RgbColor(253, 231, 37)
            );

            await new DefaultHeatmapBuilder()
                .SetSampler(sampler)
                .SetReceiver(receiver)
                .SetViewport(Viewport.FromTwoPoints(new Vector2(-5.12f), new Vector2(5.12f)))
                .SetSamplingResolution(new Vector2(800, 800))
                .SetGradient(gradient)
                .GenerateAsync();

            using var stream = await receiver.GetPngStreamAsync(1000, 1000);
            AssertPng.Equal("Images/Rastrigin.png", stream, TestOutputHelper);
        }
    }
}
