using Heatmap.Morphs;
using Heatmap.Receivers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Heatmap
{
    public abstract class HeatmapAbstract
    {
        protected Size UnsampledSize { get; private set; } = new Size(1, 1);

        private Dictionary<Vector2, Tuple<float, Vector2>> HeatMap = new Dictionary<Vector2, Tuple<float, Vector2>>();

        private Vector2 ViewportMin = Vector2.Zero;
        private Vector2 ViewportMax = Vector2.One;
        private float MinValue = float.MaxValue;
        private float MaxValue = float.MinValue;
        private Func<Vector2, float> Function;
        private IMorph Morph;
        private IReceiver Receiver;

        private readonly object AddLock = new object();

        public event EventHandler<ProgressEventArgs> Progress;

        public HeatmapAbstract(Func<Vector2, float> function, IMorph morph, IReceiver receiver)
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

        public abstract void Calculate();

        protected void ClearValues()
        {
            HeatMap = new Dictionary<Vector2, Tuple<float, Vector2>>(UnsampledSize.Width * UnsampledSize.Height);
        }

        protected float GetValue(Vector2 position)
        {
            return Function(ViewportMin + (ViewportMax - ViewportMin) * position);
        }

        protected void AddValue(Vector2 position, Vector2 size, float value)
        {
            lock(AddLock)
                HeatMap.Add(position, Tuple.Create(value, size));

            if (value < MinValue)
                MinValue = value;

            if (value > MaxValue)
                MaxValue = value;

            UpdateProgress();
        }

        protected Vector2 PixelSpaceToUnitSpace(Vector2 position)
        {
            return position * Receiver.SampleSize;
        }

        protected Vector2 UnitSpaceToPixelSpace(Vector2 position)
        {
            return position / Receiver.SampleSize;
        }

        private void UpdateProgress()
        {
            float procentageDone = HeatMap.Count() / (float)(UnsampledSize.Width * UnsampledSize.Height);

            Progress?.Invoke(this, new ProgressEventArgs(procentageDone));
        }

        public void Commit()
        {
            float range = MaxValue - MinValue;

            if (range <= float.Epsilon)
                return;

            foreach (KeyValuePair<Vector2, Tuple<float, Vector2>> point in HeatMap)
            {
                Color color = Morph.GetColor((point.Value.Item1 - MinValue) / range);
                Receiver.Receive(point.Key, point.Value.Item2, color);
            }
        }
    }
}
