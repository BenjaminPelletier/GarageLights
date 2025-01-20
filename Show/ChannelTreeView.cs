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

        public IEnumerable<NodeBounds> GetVisibleNodes()
        {
            foreach (TreeNode topNode in this.Nodes)
            {
                foreach (TreeNode node in Descendants(topNode))
                {
                    if (!node.IsVisible) { continue; }
                    yield return new NodeBounds((ChannelNodeTreeNode)node, nodeBounds[node]);
                }
            }
        }

        private static IEnumerable<TreeNode> Descendants(TreeNode parent)
        {
            yield return parent;
            foreach (TreeNode child in parent.Nodes)
            {
                foreach (TreeNode descendant in Descendants(child))
                {
                    yield return descendant;
                }
            }
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
