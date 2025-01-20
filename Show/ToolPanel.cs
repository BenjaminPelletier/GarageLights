using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GarageLights.Show
{
    public partial class ToolPanel : UserControl
    {
        public event EventHandler Play;
        public event EventHandler Stop;

        public ToolPanel()
        {
            InitializeComponent();
        }

        private void bPlay_Click(object sender, EventArgs e)
        {
            Play?.Invoke(this, EventArgs.Empty);
        }

        private void bStop_Click(object sender, EventArgs e)
        {
            Stop?.Invoke(this, EventArgs.Empty);
        }
    }
}
