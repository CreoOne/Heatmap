using Heatmap.Gradients;
using Heatmap.Primitives;
using Heatmap.Receivers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;

namespace Heatmap
{
    public abstract class HeatmapAbstract
    {
        protected Size UnsampledSize { get; private set; } = new Size(1, 1);

        private List<PatchHolder> HeatMap = new List<PatchHolder>();

        private Viewport Viewport { get; set; }
        private float MinValue = float.MaxValue;
        private float MaxValue = float.MinValue;
        private Func<Vector2, float> Function;
        private IGradient Gradient;
        private IReceiver Receiver;

        public HeatmapAbstract(Func<Vector2, float> function, IGradient gradient, IReceiver receiver)
        {
            Function = function ?? throw new ArgumentNullException(nameof(function));
            Gradient = gradient ?? throw new ArgumentNullException(nameof(gradient));
            Receiver = receiver ?? throw new ArgumentNullException(nameof(receiver));

            RecalculateUnsampledSize();
        }

        public void SetViewport(Viewport viewport) => Viewport = viewport;

        private void RecalculateUnsampledSize()
        {
            int unsampledWidth = (int)Math.Ceiling(1 / Receiver.SampleSize.X);
            int unsampledHeight = (int)Math.Ceiling(1 / Receiver.SampleSize.Y);
            UnsampledSize = new Size(unsampledWidth, unsampledHeight);
        }

        public abstract void Calculate();

        protected void ClearValues()
        {
            HeatMap = new List<PatchHolder>(UnsampledSize.Width * UnsampledSize.Height);
        }

        protected float GetValue(Vector2 position) => Function(Viewport.From + (Viewport.To - Viewport.From) * position);

        protected void AddValue(Vector2 position, Vector2 size, float value)
        {
            HeatMap.Add(new PatchHolder(position, size, value));

            if (value < MinValue)
                MinValue = value;

            if (value > MaxValue)
                MaxValue = value;
        }

        protected Vector2 PixelSpaceToUnitSpace(Vector2 position)
        {
            return position * Receiver.SampleSize;
        }

        protected Vector2 UnitSpaceToPixelSpace(Vector2 position)
        {
            return position / Receiver.SampleSize;
        }

        public void Commit()
        {
            float range = MaxValue - MinValue;

            if (range <= float.Epsilon)
                return;

            foreach (PatchHolder patch in HeatMap)
            {
                var color = Gradient.GetColor((patch.Value - MinValue) / range);
                Receiver.Receive(patch.Position, patch.Size, color);
            }
        }

        private struct PatchHolder
        {
            public Vector2 Position { get; private set; }
            public Vector2 Size { get; private set; }
            public float Value { get; private set; }

            public PatchHolder(Vector2 position, Vector2 size, float value)
            {
                Position = position;
                Size = size;
                Value = value;
            }
        }
    }
}
