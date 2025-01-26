using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageLights.Channels
{
    enum ChannelVisibilityState
    {
        ToggleExpandedCollapsed
    }

    interface IChannelSelector
    {
        IEnumerable<ChannelNodeTreeNode> GetChannelNodeTreeNodes();

        event EventHandler SelectedChannelsChanged;

        void SetVisibilityState(string fullName, ChannelVisibilityState state);
    }

    static class IChannelSelectorExtensions
    {
        public static ChannelNodeTreeNode ClosestChannel(this IChannelSelector selector, float y)
        {
            ChannelNodeTreeNode closestNode = null;
            float dy = float.PositiveInfinity;
            foreach (ChannelNodeTreeNode channelNodeTreeNode in selector.GetVisibleChannelNodeTreeNodes())
            {
                var bounds = channelNodeTreeNode.Bounds;
                if (bounds.Top <= y && y <= bounds.Bottom)
                {
                    return channelNodeTreeNode;
                }
                float cdy = y < bounds.Top ? bounds.Top - y : y - bounds.Bottom;
                if (closestNode == null || cdy < dy)
                {
                    closestNode = channelNodeTreeNode;
                    dy = cdy;
                }
            }
            return closestNode;
        }

        public static IEnumerable<ChannelNodeTreeNode> GetVisibleChannelNodeTreeNodes(this IChannelSelector selector)
        {
            return selector.GetChannelNodeTreeNodes().Where(n => n.IsVisible);
        }

        public static IEnumerable<ChannelNodeTreeNode> GetCheckedChannelNodeTreeNodes(this IChannelSelector selector)
        {
            return selector.GetChannelNodeTreeNodes()
                .Where(n => n.Checked && n.Nodes.Count == 0);
        }
    }
}
