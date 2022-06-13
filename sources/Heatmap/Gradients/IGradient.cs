using Heatmap.Primitives;

namespace Heatmap.Gradients
{
    public interface IGradient
    {
        RgbColor GetColor(double position);
    }
}
