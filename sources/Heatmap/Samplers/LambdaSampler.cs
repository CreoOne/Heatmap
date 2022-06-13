using System;
using System.Numerics;
using System.Threading.Tasks;

namespace Heatmap.Samplers
{
    public sealed class LambdaSampler : ISampler
    {
        private Func<Vector2, double> Lambda { get; }

        public LambdaSampler(Func<Vector2, double> lambda)
        {
            Lambda = lambda;
        }

        public Task<double> GetAsync(Vector2 position) => Task.FromResult(Lambda(position));
    }
}
