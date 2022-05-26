using Heatmap.Extensions;
using Heatmap.Primitives;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Numerics;

namespace Heatmap.Receivers
{
    public class BitmapBitwiseReceiver : IReceiver
    {
        public Vector2 SampleSize { get; private set; }

        private Size BitmapSize;
        private byte[] Bytes;

        public BitmapBitwiseReceiver(Size bitmapSize, Size sampleSize)
        {
            BitmapSize = bitmapSize;
            SampleSize = sampleSize.ToVector2() / bitmapSize.ToVector2();
            Bytes = new byte[bitmapSize.Width * 4 * bitmapSize.Height];
        }

        public void Receive(Vector2 position, Vector2 size, RgbColor color)
        {
            Size bitmapSpaceSize = (size * BitmapSize.ToVector2()).Ceiling().ToSize();
            int index = PositionToIndex(position);

            foreach (int y in Enumerable.Range(0, bitmapSpaceSize.Height))
                foreach (int x in Enumerable.Range(0, bitmapSpaceSize.Width))
                {
                    if (Math.Round(position.X * BitmapSize.Width) + x >= BitmapSize.Width)
                        break;

                    int offset = (y * BitmapSize.Width + x) * 4;

                    if (index + offset >= Bytes.Length)
                        return;

                    Bytes[index + offset] = color.Blue;
                    Bytes[index + offset + 1] = color.Green;
                    Bytes[index + offset + 2] = color.Red;
                }
        }

        private int PositionToIndex(Vector2 position)
        {
            return (int)(Math.Round(position.Y * BitmapSize.Height * BitmapSize.Width + position.X * BitmapSize.Width) * 4);
        }

        public Bitmap ProduceBitmap()
        {
            Bitmap result = new Bitmap(BitmapSize.Width, BitmapSize.Height, PixelFormat.Format24bppRgb);
            BitmapData resultData = result.LockBits(new Rectangle(0, 0, BitmapSize.Width, BitmapSize.Height), ImageLockMode.WriteOnly, result.PixelFormat);

            System.Runtime.InteropServices.Marshal.Copy(Bytes, 0, resultData.Scan0, Bytes.Length);
            result.UnlockBits(resultData);

            return result;
        }
    }
}
