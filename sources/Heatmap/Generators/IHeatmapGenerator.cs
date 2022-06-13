using Heatmap.Gradients;
using Heatmap.Primitives;
using Heatmap.Range;
using Heatmap.Receivers;
using Heatmap.Samplers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Heatmap.Generators
{
    public interface IHeatmapGenerator
    {
        Task SampleAsync(ISampler sampler, Viewport viewport, Resolution resolution);
        void Push(IRange range, IGradient gradient, IReceiver receiver);
        IEnumerable<PositionedSample> GetPositionedSamples();
    }
}
