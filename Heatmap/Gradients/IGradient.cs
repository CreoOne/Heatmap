using System.Drawing;

namespace Heatmap.Gradients
{
    public interface IGradient
    {
        Color GetColor(float position);
    }
}
