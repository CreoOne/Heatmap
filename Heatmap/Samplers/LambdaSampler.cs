using System;
using System.Numerics;
using System.Threading.Tasks;

namespace Heatmap.Samplers
{
    public sealed class LambdaSampler : ISampler
    {
        private Func<Vector2, float> Lambda { get; }

        public LambdaSampler(Func<Vector2, float> lambda)
        {
            Lambda = lambda;
        }

        public Task<float> GetAsync(Vector2 position) => Task.FromResult(Lambda(position));
    }
}
