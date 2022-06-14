using BenchmarkDotNet.Attributes;
using Heatmap.Gradients;
using Heatmap.Primitives;

namespace Heatmap.Benchmarks
{
    public class GradientsBenchmark
    {
        [ParamsSource(nameof(GradientSource))]
        public IGradient Gradient { get; set; }

        public static IEnumerable<IGradient> GradientSource()
        {
            var red = new RgbColor(255, 0, 0);
            var black = new RgbColor(0, 0, 0);
            var green = new RgbColor(0, 255, 0);
            var white = new RgbColor(255, 255, 255);
            var blue = new RgbColor(0, 0, 255);

            var linear = new LinearGradient(red, black, green, white, blue);
            yield return linear;
            var positioned = new PositionedGradient(new PositionedColor(0, red), new PositionedColor(0.25d, black), new PositionedColor(0.5d, green), new PositionedColor(0.75d, white), new PositionedColor(1f, blue));
            yield return positioned;
            yield return new PreCachedGradient(linear);
            yield return new PreCachedGradient(positioned);
        }

        [Benchmark]
        public RgbColor GradientRetrieve()
        {
            RgbColor result = default;

            for (double position = 0; position <= 1; position += 1e-7d)
                result = Gradient.GetColor(position);

            return result;
        }
    }
}
