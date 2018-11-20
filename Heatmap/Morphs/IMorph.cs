using System.Drawing;

namespace Heatmap.Morphs
{
    public interface IMorph
    {
        Color GetColor(float position);
    }
}