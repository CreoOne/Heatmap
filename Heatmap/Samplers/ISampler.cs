using System.Numerics;
using System.Threading.Tasks;

namespace Heatmap.Samplers
{
    public interface ISampler
    {
        Task<double> GetAsync(Vector2 position);
    }
}
