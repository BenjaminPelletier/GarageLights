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

        public ChannelNodeTreeNode ClosestChannelNodeTreeNode(float y)
        {
            ChannelNodeTreeNode closestNode = null;
            float dy = float.PositiveInfinity;
            foreach (ChannelNodeTreeNode channelNodeTreeNode in GetChannelNodeTreeNodes())
            {
                var bounds = channelNodeTreeNode.Bounds;
                if (bounds.Top <= y && y <= bounds.Bottom)
                {
                    return channelNodeTreeNode;
                }
                if (bounds.Bottom < 0 || bounds.Top > ClientSize.Height) { continue; }
                float cdy = y < bounds.Top ? bounds.Top - y : y - bounds.Bottom;
                if (closestNode == null || cdy < dy)
                {
                    closestNode = channelNodeTreeNode;
                    dy = cdy;
                }
            }
            return closestNode;
        }

        private IEnumerable<ChannelNodeTreeNode> GetChannelNodeTreeNodes(TreeNode parent = null)
        {
            if (parent == null)
            {
                foreach (TreeNode node in Nodes)
                {
                    foreach (ChannelNodeTreeNode descendant in GetChannelNodeTreeNodes(node))
                    {
                        yield return descendant;
                    }
                }
            }
            else
            {
                var channelTreeNode = (ChannelNodeTreeNode)parent;
                yield return channelTreeNode;
                foreach (TreeNode child in parent.Nodes)
                {
                    foreach (ChannelNodeTreeNode descendant in GetChannelNodeTreeNodes(child))
                    {
                        yield return descendant;
                    }
                }
            }
        }

        public IEnumerable<NodeBounds> GetVisibleNodes()
        {
            foreach (TreeNode topNode in Nodes)
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
