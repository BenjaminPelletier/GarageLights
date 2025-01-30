using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GarageLights.Channels
{
    internal class ChannelNodeTreeNode : TreeNode, IChannelElement
    {
        ChannelNode channelNode;
        public event EventHandler SelectedChanged;

        public ChannelNodeTreeNode(ChannelNode node) : base(node.Name)
        {
            channelNode = node;
        }

        public static ChannelNodeTreeNode FromChannelNode(ChannelNode channelNode, ChannelNode parent = null)
        {
            if (channelNode.Group != null)
            {
                TreeNode[] children = channelNode.Group.Nodes
                    .Select(child => new ChannelNodeTreeNode(child))
                    .ToArray();
                ChannelNodeTreeNode result = new ChannelNodeTreeNode(channelNode);
                result.Nodes.AddRange(children);
                return result;
            }
            else
            {
                return new ChannelNodeTreeNode(channelNode);
            }
        }

        public ChannelNode ChannelNode { get { return channelNode; } }

        public string FullName
        {
            get
            {
                string name = channelNode.Name;
                if (Parent != null)
                {
                    name = ((ChannelNodeTreeNode)Parent).FullName + "." + name;
                }
                return name;
            }
        }

        public IEnumerable<IChannelElement> Children
        {
            get
            {
                foreach (TreeNode n in Nodes)
                {
                    yield return (IChannelElement)n;
                }
            }
        }

        public bool Visible { get { return IsVisible; } }

        public bool Selected
        {
            get { return Checked; }
            set
            {
                Checked = value;
                SelectedChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public bool Expanded
        {
            get { return IsExpanded; }
            set
            {
                if (IsExpanded)
                {
                    Collapse();
                }
                else
                {
                    Expand();
                }
            }
        }
    }
}
