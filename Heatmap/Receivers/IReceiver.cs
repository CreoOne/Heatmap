using Heatmap.Primitives;
using System.Numerics;

namespace Heatmap.Receivers
{
    public interface IReceiver
    {
        Vector2 SampleSize { get; }

        void Receive(Vector2 position, Vector2 size, RgbColor color);
    }
}
