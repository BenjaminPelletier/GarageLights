using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageLights.Channels
{
    interface IChannelSelector
    {
        IEnumerable<ChannelNodeTreeNode> GetChannels();
        IEnumerable<ChannelNodeTreeNode> GetVisibleChannels();
        IEnumerable<ChannelNodeTreeNode> GetCheckedChannels();
    }

    static class IChannelSelectorExtensions
    {
        public static ChannelNodeTreeNode ClosestChannel(this IChannelSelector selector, float y)
        {
            ChannelNodeTreeNode closestNode = null;
            float dy = float.PositiveInfinity;
            foreach (ChannelNodeTreeNode channelNodeTreeNode in selector.GetVisibleChannels())
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
    }
}
