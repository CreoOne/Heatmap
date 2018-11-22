﻿using Heatmap.Morphs;
using Heatmap.Receivers;
using System;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace Heatmap
{
    public class SimpleHeatmap : HeatmapAbstract
    {
        public SimpleHeatmap(Func<Vector2, float> function, IMorph morph, IReceiver receiver)
            : base(function, morph, receiver) { }

        public override async Task CalculateAsync()
        {
            ClearValues();

            foreach (int y in Enumerable.Range(0, UnsampledSize.Height))
                foreach (int x in Enumerable.Range(0, UnsampledSize.Width))
                {
                    Vector2 position = PixelSpaceToUnitSpace(new Vector2(x, y));
                    float value = await GetValue(position);
                    AddValue(position, PixelSpaceToUnitSpace(Vector2.One), value);
                }
        }
    }
}
