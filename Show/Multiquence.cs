using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GarageLights.Show
{
    internal partial class Multiquence : UserControl
    {
        private Project project;

        public Multiquence()
        {
            InitializeComponent();
        }

        public Project Project
        {
            get => Project;
            set
            {
                project = value;

                tvChannels.Nodes.Clear();
                tvChannels.Nodes.AddRange(
                    project.ChannelNodes
                        .Select(n => ChannelNodeTreeNode.FromChannelNode(n))
                        .ToArray()
                );
            }
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void bStop_Click(object sender, EventArgs e)
        {

        }
    }
}
