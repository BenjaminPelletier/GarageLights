using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GarageLights.Channels
{
    internal class ChannelTreeView : TreeView, IChannelSelector
    {
        public EventHandler NodeLayoutChanged;
        Dictionary<TreeNode, Rectangle> nodeBounds = new Dictionary<TreeNode, Rectangle>();

        public event EventHandler SelectedChannelsChanged;

        public ChannelTreeView()
        {
            DrawNode += ChannelTreeView_DrawNode;
            BeforeCheck += ChannelTreeView_BeforeCheck;
            AfterCheck += ChannelTreeView_AfterCheck;
        }

        private void ChannelTreeView_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            nodeBounds[e.Node] = e.Bounds;
            e.DrawDefault = true;
            NodeLayoutChanged?.Invoke(this, EventArgs.Empty);
        }

        private void ChannelTreeView_BeforeCheck(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Action != TreeViewAction.Unknown)
            {
                // Update children
                foreach (TreeNode child in e.Node.Nodes)
                {
                    child.Checked = !e.Node.Checked;
                }
            }
        }

        private int pendingAfterCheck = 0;
        private void ChannelTreeView_AfterCheck(object sender, TreeViewEventArgs e)
        {
            pendingAfterCheck++;
            if (e.Action != TreeViewAction.Unknown)
            {
                // Uncheck parent if all child are unchecked
                if (e.Node.Checked && e.Node.Parent != null && !e.Node.Parent.Checked)
                {
                    e.Node.Parent.Checked = true;
                }

                // Check parent if any children are checked
                if (!e.Node.Checked && e.Node.Parent != null && e.Node.Parent.Checked)
                {
                    bool uncheckParent = true;
                    foreach (TreeNode child in e.Node.Parent.Nodes)
                    {
                        if (child.Checked)
                        {
                            uncheckParent = false;
                            break;
                        }
                    }
                    if (uncheckParent)
                    {
                        e.Node.Parent.Checked = false;
                    }
                }
            }
            pendingAfterCheck--;
            if (pendingAfterCheck == 0)
            {
                SelectedChannelsChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private IEnumerable<ChannelNodeTreeNode> GetChannelNodeTreeNodes(TreeNode parent)
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

        public IEnumerable<ChannelNodeTreeNode> GetChannelNodeTreeNodes()
        {
            return GetChannelNodeTreeNodes(null);
        }
    }
}
