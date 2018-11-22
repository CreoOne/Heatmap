using Heatmap.Morphs;
using Heatmap.Receivers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace Heatmap
{
    public class SimpleHeatmap
    {
        private Size UnsampledSize = new Size(1, 1);

        private Dictionary<Vector2, float> HeatMap = new Dictionary<Vector2, float>();

        private Vector2 ViewportMin = Vector2.Zero;
        private Vector2 ViewportMax = Vector2.One;
        private float MinValue = float.MaxValue;
        private float MaxValue = float.MinValue;
        private Func<Vector2, float> Function;
        private IMorph Morph;
        private IReceiver Receiver;

        public event EventHandler<ProgressEventArgs> Progress;

        public SimpleHeatmap(Func<Vector2, float> function, IMorph morph, IReceiver receiver)
        {
            Function = function ?? throw new ArgumentNullException(nameof(function));
            Morph = morph ?? throw new ArgumentNullException(nameof(morph));
            Receiver = receiver ?? throw new ArgumentNullException(nameof(receiver));

            RecalculateUnsampledSize();
        }

        public void SetViewport(Vector2 min, Vector2 max)
        {
            ViewportMin = new Vector2(Math.Min(min.X, max.X), Math.Min(min.Y, max.Y));
            ViewportMax = new Vector2(Math.Max(min.X, max.X), Math.Max(min.Y, max.Y));
        }

        private void RecalculateUnsampledSize()
        {
            int unsampledWidth = (int)Math.Ceiling(1 / Receiver.SampleSize.X);
            int unsampledHeight = (int)Math.Ceiling(1 / Receiver.SampleSize.Y);
            UnsampledSize = new Size(unsampledWidth, unsampledHeight);
        }

        public async Task CalculateAsync()
        {
            float pointsDone = 0;
            float pointsOverall = UnsampledSize.Width * UnsampledSize.Height;
            HeatMap = new Dictionary<Vector2, float>(UnsampledSize.Width * UnsampledSize.Height);

            foreach (int y in Enumerable.Range(0, UnsampledSize.Height))
                foreach (int x in Enumerable.Range(0, UnsampledSize.Width))
                {
                    Vector2 position = new Vector2(x, y) * Receiver.SampleSize;
                    float value = await Task.Factory.StartNew(() => Function(ViewportMin + (ViewportMax - ViewportMin) * position));
                    HeatMap.Add(position, value);

                    if (value < MinValue)
                        MinValue = value;

                    if (value > MaxValue)
                        MaxValue = value;

                    Progress?.Invoke(this, new ProgressEventArgs(++pointsDone / pointsOverall));
                }
        }

        public void Commit()
        {
            float range = MaxValue - MinValue;

            if (range <= float.Epsilon)
                return;

            foreach (KeyValuePair<Vector2, float> point in HeatMap)
            {
                Color color = Morph.GetColor((point.Value - MinValue) / range);
                Receiver.Receive(point.Key, Receiver.SampleSize, color);
            }
        }
    }
}
