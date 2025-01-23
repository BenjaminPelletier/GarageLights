using GarageLights.Lights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageLights.Keyframes
{
    class KeyframeManager : IKeyframeManager
    {
        List<ShowKeyframe> keyframes;
        Dictionary<string, Dictionary<int, List<TimedChannelKeyframe>>> keyframesByControllerAndAddress;
        ShowKeyframe activeKeyframe;

        public event EventHandler ActiveKeyframeChanged;
        public event EventHandler KeyframesChanged;

        public List<ShowKeyframe> Keyframes
        {
            get { return keyframes; }
            set
            {
                keyframesByControllerAndAddress = null;
                keyframes = value;
            }
        }

        public ShowKeyframe ActiveKeyframe
        {
            get { return activeKeyframe; }
            set
            {
                if (value != activeKeyframe)
                {
                    activeKeyframe = value;
                    // TODO: Seek to new keyframe unless audio is playing
                    ActiveKeyframeChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public Dictionary<string, Dictionary<int, List<TimedChannelKeyframe>>> GetKeyframesByControllerAndAddress(IEnumerable<ChannelNode> nodes)
        {
            if (keyframesByControllerAndAddress == null && keyframes != null)
            {
                keyframesByControllerAndAddress = OrganizeKeyframesByControllerAndAddress(keyframes, nodes);
            }
            return keyframesByControllerAndAddress;
        }

        private static Dictionary<string, Dictionary<int, List<TimedChannelKeyframe>>> OrganizeKeyframesByControllerAndAddress(List<ShowKeyframe> keyframes, IEnumerable<ChannelNode> channelNodes)
        {
            Dictionary<string, Channel> channelsByFullName = MapChannelNodes(channelNodes);

            var addressKeyframesByController = new Dictionary<string, Dictionary<int, List<TimedChannelKeyframe>>>();
            foreach (ShowKeyframe f in keyframes)
            {
                if (f.Channels == null) { continue; }
                foreach (var fullChannelNameAndKeyframe in f.Channels)
                {
                    string fullName = fullChannelNameAndKeyframe.Key;
                    var channelKeyframe = fullChannelNameAndKeyframe.Value;

                    Channel channel = channelsByFullName[fullName];

                    // Select output by controller
                    Dictionary<int, List<TimedChannelKeyframe>> keyframesByAddress;
                    if (!addressKeyframesByController.TryGetValue(channel.Controller, out keyframesByAddress))
                    {
                        keyframesByAddress = new Dictionary<int, List<TimedChannelKeyframe>>();
                        addressKeyframesByController[channel.Controller] = keyframesByAddress;
                    }

                    // Select output by address
                    List<TimedChannelKeyframe> channelKeyframes;
                    if (!keyframesByAddress.TryGetValue(channel.Address, out channelKeyframes))
                    {
                        channelKeyframes = new List<TimedChannelKeyframe>();
                        keyframesByAddress[channel.Address] = channelKeyframes;
                    }

                    // Add a new channel keyframe
                    channelKeyframes.Add(new TimedChannelKeyframe(f.Time, channelKeyframe));
                }
            }
            return addressKeyframesByController;
        }

        private static Dictionary<string, Channel> MapChannelNodes(IEnumerable<ChannelNode> channelNodes)
        {
            var channelsByFullName = new Dictionary<string, Channel>();
            foreach (ChannelNode node in channelNodes)
            {
                if (node.Group != null)
                {
                    foreach (var channelByName in MapChannelNodes(node.Group.Nodes))
                    {
                        Channel mappedChannel = channelByName.Value
                            .OffsetAddress(node.Group.Address)
                            .WithParentController(node.Group.Controller);
                        channelsByFullName[node.Name + "." + channelByName.Key] = mappedChannel;
                    }
                }
                else if (node.Channel != null)
                {
                    channelsByFullName[node.Name] = node.Channel;
                }
                else
                {
                    throw new NotImplementedException("ChannelNode '" + node.Name + "' was not a Group node nor Channel node");
                }
            }
            return channelsByFullName;
        }

        public void NotifyKeyframesChanged()
        {
            keyframesByControllerAndAddress = null;
            KeyframesChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
