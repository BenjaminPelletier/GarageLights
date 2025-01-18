using GarageLights.Lights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GarageLights.Show
{
    internal class ChannelNodeTreeNode : TreeNode
    {
        ChannelNode parent;
        ChannelNode channelNode;

        public ChannelNodeTreeNode(ChannelNode node, ChannelNode parent) : base(node.Name)
        {
            this.parent = parent;
            channelNode = node;
        }

        public static ChannelNodeTreeNode FromChannelNode(ChannelNode channelNode, ChannelNode parent = null)
        {
            if (channelNode.Group != null)
            {
                TreeNode[] children = channelNode.Group.Nodes
                    .Select(child => new ChannelNodeTreeNode(child, parent))
                    .ToArray();
                ChannelNodeTreeNode result = new ChannelNodeTreeNode(channelNode, parent);
                result.Nodes.AddRange(children);
                return result;
            }
            else
            {
                return new ChannelNodeTreeNode(channelNode, parent);
            }
        }
    }
}
