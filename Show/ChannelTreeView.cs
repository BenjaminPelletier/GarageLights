using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GarageLights.Show
{
    internal class ChannelTreeView : TreeView
    {
        public ChannelTreeView()
        {
            DrawNode += ChannelTreeView_DrawNode;
        }

        private void ChannelTreeView_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            e.DrawDefault = true;
        }
    }
}
