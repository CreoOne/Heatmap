using Heatmap.Primitives;
using System.Numerics;
using System.Threading.Tasks;

namespace Heatmap.Receivers
{
    public interface IReceiver
    {
        Task ReceiveAsync(Vector2 position, Vector2 size, RgbColor color);
    }
}
