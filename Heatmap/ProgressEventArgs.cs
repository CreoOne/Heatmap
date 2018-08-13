using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heatmap
{
    public class ProgressEventArgs
    {
        public float ProcentageDone { get; private set; }

        public ProgressEventArgs(float procentageDone)
        {
            ProcentageDone = procentageDone;
        }
    }
}
