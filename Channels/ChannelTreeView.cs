using GarageLights.Channels;
using GarageLights.Lights;
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
    internal class ChannelTreeView : TreeView, IChannelSelector
    {
        public EventHandler NodeLayoutChanged;
        Dictionary<TreeNode, Rectangle> nodeBounds = new Dictionary<TreeNode, Rectangle>();

        public ChannelTreeView()
        {
            DrawNode += ChannelTreeView_DrawNode;
        }

        private void ChannelTreeView_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            nodeBounds[e.Node] = e.Bounds;
            e.DrawDefault = true;
            NodeLayoutChanged?.Invoke(this, EventArgs.Empty);
        }

        private IEnumerable<ChannelNodeTreeNode> GetChannelNodeTreeNodes(TreeNode parent, Func<ChannelNodeTreeNode, bool> filter = null)
        {
            if (parent == null)
            {
                foreach (TreeNode node in Nodes)
                {
                    foreach (ChannelNodeTreeNode descendant in GetChannelNodeTreeNodes(node, filter))
                    {
                        yield return descendant;
                    }
                }
            }
            else
            {
                var channelTreeNode = (ChannelNodeTreeNode)parent;
                if (filter == null || filter(channelTreeNode))
                {
                    yield return channelTreeNode;
                }
                foreach (TreeNode child in parent.Nodes)
                {
                    foreach (ChannelNodeTreeNode descendant in GetChannelNodeTreeNodes(child, filter))
                    {
                        yield return descendant;
                    }
                }
            }
        }

        public IEnumerable<ChannelNodeTreeNode> GetChannels()
        {
            return GetChannelNodeTreeNodes(null);
        }

        public IEnumerable<ChannelNodeTreeNode> GetCheckedChannels()
        {
            return GetChannelNodeTreeNodes(null, n => n.Checked);
        }

        public IEnumerable<ChannelNodeTreeNode> GetVisibleChannels()
        {
            return GetChannelNodeTreeNodes(null, n => n.IsVisible);
        }

        public IEnumerable<NodeBounds> GetVisibleNodes()
        {
            return GetChannelNodeTreeNodes(null, n => n.IsVisible)
                .Select(n => new NodeBounds(n, nodeBounds[n]));
        }
    }

    internal class NodeBounds
    {
        public ChannelNodeTreeNode Node;
        public Rectangle Bounds;

        public NodeBounds(ChannelNodeTreeNode node, Rectangle bounds)
        {
            Node = node;
            Bounds = bounds;
        }
    }

    internal class NodeLayoutChangedEventArgs : EventArgs
    {
        public NodeBounds NodeBounds;

        public NodeLayoutChangedEventArgs(NodeBounds nodeBounds)
        {
            NodeBounds = nodeBounds;
        }
    }
}
