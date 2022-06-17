using BenchmarkDotNet.Attributes;
using Heatmap.Extensions;

namespace Heatmap.Benchmarks
{
    public class ClampingBenchmarks
    {
        [Params(-1d, 0d, 0.5d, 1d, Math.PI)]
        public double Value { get; set; }

        [Benchmark]
        public double MathBased() => Math.Max(0, Math.Min(Value, 1));

        [Benchmark]
        public double ExtensionBased() => Value.Clamp(0, 1);
    }
}
