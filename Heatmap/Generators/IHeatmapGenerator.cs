using Heatmap.Gradients;
using Heatmap.Primitives;
using Heatmap.Range;
using Heatmap.Receivers;
using System.Numerics;
using System.Threading.Tasks;

namespace Heatmap.Generators
{
    public interface IHeatmapGenerator
    {
        Task SampleAsync(Viewport viewport, Vector2 resolution);
        void Push(IRange range, IGradient gradient, IReceiver receiver);
    }
}
