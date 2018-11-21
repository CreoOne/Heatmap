using Heatmap;
using Heatmap.Morphs;
using Heatmap.Receivers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HeatmapSamples
{
    public partial class Simple : Form
    {
        private SimpleHeatmap Heatmap;
        private BitmapGraphicsReceiver Receiver;
        private DateTime StartTime;
        private DateTime StopTime;

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

            Receiver = new BitmapGraphicsReceiver(pCanvas.ClientSize, new Size(1, 1));
            Heatmap = new SimpleHeatmap(CalculateFragment, morph, Receiver);

            int updateCount = 0;

            Heatmap.Progress += (o, e) =>
            {
                if (updateCount++ % 1000 == 0)
                {
                    Text = string.Format("Progress {0:##0.0%}", e.ProcentageDone);
                    Application.DoEvents();
                }

                StopTime = DateTime.UtcNow;
            };
        }

        private static float CalculateFragment(Vector2 vector)
        {
            return (Vector2.Zero - vector).Length();
        }

        private void SimpleSynchronous_Shown(object sender, EventArgs methodEventArgs)
        {
            StartTime = DateTime.UtcNow;

            if (pCanvas.Image != null)
                pCanvas.Image.Dispose();

            Heatmap.Calculate();
            DateTime startTime = DateTime.UtcNow;
            Heatmap.Commit();
            pCanvas.Image = Receiver.Result;
            Text = string.Format("Calculation done in {0:####0.00}ms | Drawing done in {1:####0.00}ms", (StopTime - StartTime).TotalMilliseconds, (DateTime.UtcNow - startTime).TotalMilliseconds);
            pCanvas.Invalidate();
        }
    }
}
