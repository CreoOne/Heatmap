using System.Numerics;
using System.Threading.Tasks;

namespace Heatmap.Samplers
{
    public interface ISampler
    {
        Task<float> GetAsync(Vector2 position);
    }
}
