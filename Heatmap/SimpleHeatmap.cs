using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;

namespace Heatmap
{
    public class SimpleHeatmap
    {
        private Size ResultSize = new Size(1, 1);
        private Size UnsampledSize = new Size(1, 1);
        private int SampleSize = 1;

        private Dictionary<Vector2, float>HeatMap = new Dictionary<Vector2, float>();

        private Vector2 ViewportMin = Vector2.Zero;
        private Vector2 ViewportMax = Vector2.One;
        private float MinValue = float.MaxValue;
        private float MaxValue = float.MinValue;
        private Func<Vector2, float> Function;
        private Morph Morph;

        public event EventHandler<ProgressEventArgs> Progress;

        public SimpleHeatmap(Func<Vector2, float> function, Morph morph)
        {
            Function = function ?? throw new ArgumentNullException(nameof(function));
            Morph = morph ?? throw new ArgumentNullException(nameof(morph));
        }

        public void SetViewport(Vector2 min, Vector2 max)
        {
            ViewportMin = new Vector2(Math.Min(min.X, max.X), Math.Min(min.Y, max.Y));
            ViewportMax = new Vector2(Math.Max(min.X, max.X), Math.Max(min.Y, max.Y));
        }

        public void SetResultSize(int width, int height)
        {
            if (width < 0)
                throw new ArgumentOutOfRangeException(nameof(width));

            if (height < 0)
                throw new ArgumentOutOfRangeException(nameof(height));

            ResultSize = new Size(width, height);
            RecalculateUnsampledSize();
        }

        public void SetSampleSize(int size)
        {
            if (size < 1)
                throw new ArgumentOutOfRangeException(nameof(size));

            SampleSize = size;
            RecalculateUnsampledSize();
        }

        private void RecalculateUnsampledSize()
        {
            int unsampledWidth = (int)Math.Ceiling(ResultSize.Width / (decimal)SampleSize);
            int unsampledHeight = (int)Math.Ceiling(ResultSize.Height / (decimal)SampleSize);
            UnsampledSize = new Size(unsampledWidth, unsampledHeight);
        }

        public void Calculate()
        {
            float pointsDone = 0;
            float pointsOverall = UnsampledSize.Width * UnsampledSize.Height;
            HeatMap = new Dictionary<Vector2, float>();
            Vector2 size = new Vector2(ResultSize.Width, ResultSize.Height);

            foreach (int y in Enumerable.Range(0, UnsampledSize.Height))
                foreach (int x in Enumerable.Range(0, UnsampledSize.Width))
                {
                    Vector2 position = new Vector2(x, y) * SampleSize;
                    float value = Function(ViewportMin + (ViewportMax - ViewportMin) * (position / size));
                    HeatMap.Add(position, value);

                    if (value < MinValue)
                        MinValue = value;

                    if (value > MaxValue)
                        MaxValue = value;

                    Progress?.Invoke(this, new ProgressEventArgs(++pointsDone / pointsOverall));
                }
        }

        public Bitmap GetResultCurrentStateAsync()
        {
            Bitmap result = new Bitmap(ResultSize.Width, ResultSize.Height);

            using (Graphics context = Graphics.FromImage(result))
            {
                context.Clear(Color.Transparent);

                if (HeatMap.Count < 0)
                    return result;

                float range = MaxValue - MinValue;

                if (range <= float.Epsilon)
                    return result;

                foreach (int y in Enumerable.Range(0, UnsampledSize.Height))
                    foreach (int x in Enumerable.Range(0, UnsampledSize.Width))
                    {
                        Vector2 position = new Vector2(x, y) * SampleSize;

                        if (!HeatMap.ContainsKey(position))
                            continue;

                        using (SolidBrush brush  = new SolidBrush(Morph.GetColor((HeatMap[position] - MinValue) / range)))
                            context.FillRectangle(brush, x * SampleSize, (ResultSize.Height - SampleSize) - y * SampleSize, SampleSize, SampleSize);
                    }

                return result;
            }
        }
    }
}
