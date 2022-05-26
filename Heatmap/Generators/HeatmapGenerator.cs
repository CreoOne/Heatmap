using Heatmap.Gradients;
using Heatmap.Primitives;
using Heatmap.Range;
using Heatmap.Receivers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Heatmap.Generators
{
    public sealed class HeatmapGenerator : IHeatmapGenerator
    {
        private Func<Vector2, float> Sampler { get; }
        private IGradient Gradient { get; }
        private IReceiver Receiver { get; }

        public HeatmapGenerator(Func<Vector2, float> sampler, IGradient gradient, IReceiver receiver)
        {
            Sampler = sampler ?? throw new ArgumentNullException(nameof(sampler));
            Gradient = gradient ?? throw new ArgumentNullException(nameof(gradient));
            Receiver = receiver ?? throw new ArgumentNullException(nameof(receiver));
        }

        public void Generate(Viewport viewport, IRangeFactory rangeFactory)
        {
            List<Fragment> fragments = new();
            var minValue = float.MaxValue;
            var maxValue = float.MinValue;
            var sampledResolution = CalculateResolution(Receiver.SampleSize);
            var sampleSize = PixelSpaceToUnitSpace(Vector2.One);

            foreach (int y in Enumerable.Range(0, sampledResolution.Height))
                foreach (int x in Enumerable.Range(0, sampledResolution.Width))
                {
                    Vector2 position = PixelSpaceToUnitSpace(new Vector2(x, y));
                    var viewPoint = viewport.From + (viewport.To - viewport.From) * position;
                    var sample = Sampler(viewPoint);

                    fragments.Add(new Fragment(position, sampleSize, sample));

                    minValue = Math.Min(minValue, sample);
                    maxValue = Math.Max(maxValue, sample);
                }

            var range = rangeFactory.Create(minValue, maxValue);

            foreach (Fragment fragment in fragments)
            {
                var rangedFragment = range.GetValue(fragment.Value);
                var color = Gradient.GetColor(rangedFragment);
                Receiver.Receive(fragment.Position, fragment.Size, color);
            }
        }

        private static (int Width, int Height) CalculateResolution(Vector2 sampleSize)
        {
            int width = (int)(1 / sampleSize.X);
            int height = (int)(1 / sampleSize.Y);
            return new(width, height);
        }

        private Vector2 PixelSpaceToUnitSpace(Vector2 position) => position * Receiver.SampleSize;
    }
}
