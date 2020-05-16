using Heatmap.Morphs;
using Heatmap.Receivers;
using System;
using System.Linq;
using System.Numerics;

namespace Heatmap
{
    public class HeatmapGenerator : HeatmapAbstract
    {
        public HeatmapGenerator(Func<Vector2, float> function, IMorph morph, IReceiver receiver)
            : base(function, morph, receiver) { }

        public override void Calculate()
        {
            ClearValues();

            foreach (int y in Enumerable.Range(0, UnsampledSize.Height))
                foreach (int x in Enumerable.Range(0, UnsampledSize.Width))
                {
                    Vector2 position = PixelSpaceToUnitSpace(new Vector2(x, y));
                    float value = GetValue(position);
                    AddValue(position, PixelSpaceToUnitSpace(Vector2.One), value);
                }
        }
    }
}
