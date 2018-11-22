using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Heatmap.Extensions;
using Heatmap.Morphs;
using Heatmap.Receivers;

namespace Heatmap
{
    public class QuadTreeHeatmap : HeatmapAbstract
    {
        public QuadTreeHeatmap(Func<Vector2, float> function, IMorph morph, IReceiver receiver)
            : base(function, morph, receiver) { }

        public override Task CalculateAsync()
        {
            ClearValues();

            foreach(int regionNumber in Enumerable.Range(0, 4))
                CalculateRegion(regionNumber);

            if(!TooDeep(0))
                foreach (int regionNumber in Enumerable.Range(0, 4))
                    EnterRegion(regionNumber);

            return Task.Delay(1);
        }

        private void CalculateRegion(int regionNumber, int depth = 0)
        {
            Vector2 position = ProduceUnitPositionFromGlobalRegion(regionNumber, depth);
            float value = Task.Factory.StartNew(() => GetValue(position)).Result.Result;
            AddValue(position, new Vector2(DepthToPrecition(depth)), value);
        }

        private void EnterRegion(int regionNumber, int depth = 0)
        {
            int[] globalRegionNumbers = Enumerable.Range(0, 4)
                .Select(i => ProduceGlobalRegionNumber(regionNumber, i, depth + 1))
                .ToArray();

            foreach (int localRegionNumber in Enumerable.Range(1, 3))
                CalculateRegion(globalRegionNumbers[localRegionNumber], depth + 1);

            if(!TooDeep(depth))
                foreach (int localRegionNumber in Enumerable.Range(0, 4))
                    EnterRegion(globalRegionNumbers[localRegionNumber], depth + 1);
        }

        private bool TooDeep(int depth)
        {
            float precision = DepthToPrecition(depth + 2);
            Vector2 sampleSize = PixelSpaceToUnitSpace(Vector2.One);

            return precision <= sampleSize.X && precision <= sampleSize.Y;
        }

        private static int ProduceGlobalRegionNumber(int regionNumber, int childRegionNumber, int depth)
        {
            return regionNumber | childRegionNumber << (depth * 2);
        }

        private static Vector2 ProduceUnitPositionFromGlobalRegion(int globalRegion, int depth)
        {
            Vector2 result = Vector2.Zero;

            foreach(int index in Enumerable.Range(0, depth + 1))
            {
                int mask = 3 << (index * 2);
                int part = (globalRegion & mask) >> (index * 2);

                float precision = DepthToPrecition(index);
                float xShift = (part & 1) == 0 ? 0 : precision;
                float yShift = (part & 2) == 0 ? 0 : precision;

                result += new Vector2(xShift, yShift);
            }

            return result;
        }

        private static float DepthToPrecition(int depth)
        {
            return (float)(1 / Math.Pow(2, depth + 1));
        }
    }
}
