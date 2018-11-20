using Heatmap;
using Heatmap.Receivers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HeatmapSamples
{
    public partial class Simple : Form
    {
        private SimpleHeatmap Heatmap;
        private BitmapSimpleReceiver Receiver;
        private DateTime StartTime;
        private DateTime StopTime;

        public Simple()
        {
            InitializeComponent();

            Morph morph = new Morph(
                Color.White,
                Color.FromArgb(255, 239, 167), // light yellow
                Color.FromArgb(206, 168, 106), // calm orange
                Color.FromArgb(140, 66, 57), // prestigue red
                Color.FromArgb(46, 23, 54), // violet
                Color.Black
            );

            Receiver = new BitmapSimpleReceiver(pCanvas.ClientSize, new Size(1, 1));
            float diagonal = (float)Math.Sqrt(Math.Pow(pCanvas.ClientSize.Width, 2) + Math.Pow(pCanvas.ClientSize.Height, 2));
            Heatmap = new SimpleHeatmap((v) => (Vector2.Zero - v).Length() / diagonal, morph, Receiver);

            Heatmap.Progress += (o, e) =>
            {
                Text = string.Format("Progress {0:##0.0%}", e.ProcentageDone);
                StopTime = DateTime.UtcNow;

                Application.DoEvents();
            };
        }

        private void SimpleSynchronous_Shown(object sender, EventArgs methodEventArgs)
        {
            StartTime = DateTime.UtcNow;
            Heatmap.Calculate();

            if (pCanvas.Image != null)
                pCanvas.Image.Dispose();

            DateTime startTime = DateTime.UtcNow;
            Heatmap.Commit();
            pCanvas.Image = Receiver.Result;
            Text = string.Format("Calculation done in {0:####0.00}ms | Drawing done in {1:####0.00}ms", (StopTime - StartTime).TotalMilliseconds, (DateTime.UtcNow - startTime).TotalMilliseconds);
            pCanvas.Invalidate();
        }
    }
}
