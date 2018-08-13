using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HeatmapSamples
{
    public partial class FrontForm : Form
    {
        public FrontForm()
        {
            InitializeComponent();
        }

        private static void ShowDialog<TForm>() where TForm : Form, new()
        {
            using (Form form = new TForm())
                form.ShowDialog();
        }

        private void bSimpleSynchronous_Click(object sender, EventArgs e)
        {
            ShowDialog<Simple>();
        }
    }
}
