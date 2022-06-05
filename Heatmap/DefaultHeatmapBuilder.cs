using Heatmap.Generators;
using Heatmap.Gradients;
using Heatmap.Primitives;
using Heatmap.Range;
using Heatmap.Receivers;
using Heatmap.Samplers;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace Heatmap
{
    public class DefaultHeatmapBuilder
    {
        private ISampler Sampler { get; set; }

        public DefaultHeatmapBuilder SetSampler(ISampler sampler)
        {
            Sampler = sampler;
            return this;
        }

        private Viewport Viewport { get; set; } = Viewport.FromTwoPoints(new Vector2(-1), new Vector2(1));

        public DefaultHeatmapBuilder SetViewport(Viewport viewport)
        {
            Viewport = viewport;
            return this;
        }

        private Vector2 Resolution { get; set; } = new Vector2(100, 100);

        public DefaultHeatmapBuilder SetSamplingResolution(Vector2 resolution)
        {
            Resolution = resolution;
            return this;
        }

        private IRangeFactory RangeFactory { get; set; } = new AdaptiveRangeFactory();

        public DefaultHeatmapBuilder SetRangeFactory(IRangeFactory rangeFactory)
        {
            RangeFactory = rangeFactory;
            return this;
        }

        private IGradient Gradient { get; set; } = new LinearGradient(new RgbColor(255, 255, 255), new RgbColor(0, 0, 0)); // black hot

        public DefaultHeatmapBuilder SetGradient(IGradient gradient)
        {
            Gradient = gradient;
            return this;
        }

        private IReceiver Receiver { get; set; }

        public DefaultHeatmapBuilder SetReceiver(IReceiver receiver)
        {
            Receiver = receiver;
            return this;
        }

        private IHeatmapGenerator HeatmapGenerator { get; set; } = new DefaultHeatmapGenerator();

        public DefaultHeatmapBuilder SetHeatmapGenerator(IHeatmapGenerator heatmapGenerator)
        {
            HeatmapGenerator = heatmapGenerator;
            return this;
        }

        public async Task GenerateAsync()
        {
            await HeatmapGenerator.SampleAsync(Sampler, Viewport, Resolution);
            var values = HeatmapGenerator.GetPositionedSamples().Select(sample => sample.Value);
            HeatmapGenerator.Push(RangeFactory.Create(values), Gradient, Receiver);
        }
    }
}
