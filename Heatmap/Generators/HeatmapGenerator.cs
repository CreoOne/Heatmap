using Heatmap.Gradients;
using Heatmap.Primitives;
using Heatmap.Range;
using Heatmap.Receivers;
using Heatmap.Samplers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace Heatmap.Generators
{
    public sealed class HeatmapGenerator : IHeatmapGenerator
    {
        private ISampler Sampler { get; }
        private ConcurrentBag<Fragment> Fragments { get; } = new();

        public HeatmapGenerator(ISampler sampler)
        {
            Sampler = sampler ?? throw new ArgumentNullException(nameof(sampler));
        }

        public async Task SampleAsync(Viewport viewport, Vector2 resolution)
        {
            Fragments.Clear();

            var sampleSize = new Vector2(1f) / resolution;

            foreach (int y in Enumerable.Range(0, (int)resolution.Y))
                foreach (int x in Enumerable.Range(0, (int)resolution.X))
                {
                    Vector2 unitPosition = new Vector2(x, y) / resolution;
                    var viewPoint = viewport.GetViewPoint(unitPosition);
                    var sample = await Sampler.GetAsync(viewPoint);

                    Fragments.Add(new Fragment(unitPosition, sampleSize, sample));
                }   
        }

        public async Task PushAsync(IRange range, IGradient gradient, IReceiver receiver)
        {
            foreach (Fragment fragment in GetFragments())
            {
                var rangedFragment = range.GetValue(fragment.Value);
                var color = gradient.GetColor(rangedFragment);
                await receiver.ReceiveAsync(fragment.Position, fragment.Size, color);
            }
        }

        public IEnumerable<Fragment> GetFragments() => Fragments.ToArray();
    }
}
