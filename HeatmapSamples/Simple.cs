using Heatmap;
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
            Heatmap = new SimpleHeatmap((v) => 1.4f - (v - new Vector2(0.5f)).Length(), morph);
            Heatmap.SetResultSize(pCanvas.ClientSize.Width, pCanvas.ClientSize.Height);
            Heatmap.SetSampleSize(10);

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
            pCanvas.Image = Heatmap.GetResultCurrentStateAsync();
            Text = string.Format("Calculation done in {0:####0.00}ms | Drawing done in {1:####0.00}ms", (StopTime - StartTime).TotalMilliseconds, (DateTime.UtcNow - startTime).TotalMilliseconds);
            pCanvas.Invalidate();
        }
    }
}
