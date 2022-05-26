using Heatmap.Primitives;
using Heatmap.Range;

namespace Heatmap.Generators
{
    public interface IHeatmapGenerator
    {
        void Generate(Viewport viewport, IRangeFactory range);
    }
}
