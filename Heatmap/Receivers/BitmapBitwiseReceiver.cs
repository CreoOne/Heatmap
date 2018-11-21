using Heatmap.Extensions;
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

        public void Receive(Vector2 position, Vector2 size, Color color)
        {
            Size bitmapSpaceSize = (size * BitmapSize.ToVector2()).Round().ToSize();
            int index = PositionToIndex(position);

            foreach (int y in Enumerable.Range(0, bitmapSpaceSize.Height))
                foreach (int x in Enumerable.Range(0, bitmapSpaceSize.Width))
                {
                    if (Math.Round(position.X * BitmapSize.Width) + x >= BitmapSize.Width)
                        break;

                    int offset = y * BitmapSize.Width * 4 + x * 4;

                    if (index + offset >= Bytes.Length)
                        return;

                    Bytes[index + offset] = color.B;
                    Bytes[index + offset + 1] = color.G;
                    Bytes[index + offset + 2] = color.R;
                    Bytes[index + offset + 3] = color.A;
                }
        }

        private int PositionToIndex(Vector2 position)
        {
            int stride = BitmapSize.Width * 4;
            return (int)(Math.Round(position.Y * BitmapSize.Height * stride + position.X * stride));
        }

        public Bitmap ProduceBitmap()
        {
            Bitmap result = new Bitmap(BitmapSize.Width, BitmapSize.Height, PixelFormat.Format32bppArgb);
            BitmapData resultData = result.LockBits(new Rectangle(0, 0, BitmapSize.Width, BitmapSize.Height), ImageLockMode.WriteOnly, result.PixelFormat);

            System.Runtime.InteropServices.Marshal.Copy(Bytes, 0, resultData.Scan0, Bytes.Length);
            result.UnlockBits(resultData);

            return result;
        }
    }
}
