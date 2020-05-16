using Heatmap.Gradients;
using Heatmap.Options;
using Heatmap.Primitives;
using Heatmap.Range;
using System.Collections.Generic;
using System.Drawing;

namespace Heatmap
{
    public interface IHeatmapGenerator
    {
        Bitmap Generate(IEnumerable<Sample> samples, IGradient gradient, IRange range, Viewport viewport, HeatmapGeneratorOptions options);
    }
}
