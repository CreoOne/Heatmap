using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Heatmap.Gradients
{
    public class LinearGradient : IGradient
    {
        private PositionedColor[] Colors { get; }

        public LinearGradient(IEnumerable<PositionedColor> positionedColors)
        {
            Colors = positionedColors.OrderBy(positionedColor => positionedColor).ToArray();
        }

        public Color GetColor(float position)
        {
            throw new NotImplementedException();
        }
    }
}
