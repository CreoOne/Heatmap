using Heatmap.Generators;
using Heatmap.Gradients;
using Heatmap.Primitives;
using Heatmap.Range;
using Heatmap.Receivers;
using Heatmap.Samplers;
using System.Threading.Tasks;

namespace Heatmap
{
    public interface IDefaultHeatmapBuilder
    {
        Task GenerateAsync();
        DefaultHeatmapBuilder SetGradient(IGradient gradient);
        DefaultHeatmapBuilder SetHeatmapGenerator(IHeatmapGenerator heatmapGenerator);
        DefaultHeatmapBuilder SetRangeFactory(IRangeFactory rangeFactory);
        DefaultHeatmapBuilder SetReceiver(IReceiver receiver);
        DefaultHeatmapBuilder SetSampler(ISampler sampler);
        DefaultHeatmapBuilder SetSamplingResolution(Resolution resolution);
        DefaultHeatmapBuilder SetViewport(Viewport viewport);
    }
}