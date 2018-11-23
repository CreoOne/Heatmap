using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Heatmap.Morphs;
using Heatmap.Receivers;

namespace Heatmap
{
    public class QuadTreeHeatmap : HeatmapAbstract
    {
        private int MaxDepth;
        private float[] Precisions;

        public QuadTreeHeatmap(Func<Vector2, float> function, IMorph morph, IReceiver receiver)
            : base(function, morph, receiver)
        {
            CalculateMaxDepth();
            PrecalculatePrecisionTable();
        }

        private void CalculateMaxDepth()
        {
            Vector2 sampleSize = PixelSpaceToUnitSpace(Vector2.One);
            int depth = 0;

            do
            {
                sampleSize *= 2;
                depth++;
            }
            while (sampleSize.X < 1 || sampleSize.Y < 1);

            MaxDepth = depth - 1;
        }

        private void PrecalculatePrecisionTable()
        {
            Precisions = Enumerable.Range(0, MaxDepth + 1).Select(i => DepthToPrecision(i)).ToArray();
        }

        public override void Calculate()
        {
            ClearValues();

            foreach(int regionNumber in Enumerable.Range(0, 4))
                CalculateRegion(regionNumber);

            if(!TooDeep(0))
                foreach (int regionNumber in Enumerable.Range(0, 4))
                    EnterRegion(regionNumber);
        }

        private void CalculateRegion(int regionNumber, int depth = 0)
        {
            Vector2 position = ProduceUnitPositionFromGlobalRegion(regionNumber, depth);
            float value = GetValue(position);
            AddValue(position, new Vector2(Precisions[depth]), value);
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
            return depth + 1 >= MaxDepth;
        }

        private static int ProduceGlobalRegionNumber(int regionNumber, int childRegionNumber, int depth)
        {
            return regionNumber | childRegionNumber << (depth * 2);
        }

        private Vector2 ProduceUnitPositionFromGlobalRegion(int globalRegion, int depth)
        {
            Vector2 result = Vector2.Zero;

            foreach(int index in Enumerable.Range(0, depth + 1))
            {
                int mask = 3 << (index * 2);
                int part = (globalRegion & mask) >> (index * 2);

                float precision = Precisions[index];
                float xShift = (part & 1) == 0 ? 0 : precision;
                float yShift = (part & 2) == 0 ? 0 : precision;

                result += new Vector2(xShift, yShift);
            }

            return result;
        }

        private static float DepthToPrecision(int depth)
        {
            return (float)Math.Pow(2, -(depth + 1));
        }
    }
}
