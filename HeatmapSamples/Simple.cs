using Heatmap;
using Heatmap.Morphs;
using Heatmap.Receivers;
using System;
using System.Drawing;
using System.Numerics;
using System.Windows.Forms;

namespace HeatmapSamples
{
    public partial class Simple : Form
    {
        private HeatmapAbstract Heatmap;
        private BitmapBitwiseReceiver Receiver;

        public Simple()
        {
            InitializeComponent();

            IMorph morph = new CachedMorph(1000, new Morph(
                Color.White,
                Color.FromArgb(255, 239, 167), // light yellow
                Color.FromArgb(206, 168, 106), // calm orange
                Color.FromArgb(140, 66, 57), // prestigue red
                Color.FromArgb(46, 23, 54), // violet
                Color.Black
            ));

            Receiver = new BitmapBitwiseReceiver(pCanvas.ClientSize, new Size(20, 20));
            Heatmap = new HeatmapGenerator(CalculateFragment, morph, Receiver);
        }

        private static float CalculateFragment(Vector2 vector)
        {
            return (Vector2.Zero - vector).Length();
        }

        private void UpdateBitmap()
        {
            Heatmap.Commit();
            Bitmap bitmap = Receiver.ProduceBitmap();

            if (!ReferenceEquals(bitmap, pCanvas.Image))
                pCanvas.Image?.Dispose();

            pCanvas.Image = bitmap;
            pCanvas.Refresh();
        }

        private void SimpleSynchronous_Shown(object sender, EventArgs methodEventArgs)
        {
            Heatmap.Calculate();
            UpdateBitmap();
        }
    }
}
