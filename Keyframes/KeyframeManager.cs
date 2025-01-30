using GarageLights.Channels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageLights.Keyframes
{
    [DesignerCategory("Component")]
    [ToolboxItem(true)]
    class KeyframeManager : Component
    {
        private List<ShowKeyframe> keyframes;
        private Dictionary<string, Dictionary<int, List<TimedChannelKeyframe>>> keyframesByControllerAndAddress;
        
        public event EventHandler KeyframesChanged;

        public KeyframeManager() { }

        public KeyframeManager(IContainer container)
        {
            container?.Add(this);
        }

        public List<ShowKeyframe> Keyframes
        {
            get { return keyframes; }
            set
            {
                keyframesByControllerAndAddress = null;
                keyframes = value;
            }
        }

        public int GetChannelValue(string fullName, float t)
        {
            List<TimedChannelKeyframe> timedKeyframes = keyframes
                .Where(f => f.Channels != null && f.Channels.Any(kvp => kvp.Key == fullName))
                .Select(f => new TimedChannelKeyframe(f.Time, f.Channels[fullName]))
                .ToList();
            return timedKeyframes.Count > 0 ? timedKeyframes.Interpolate(t) : 0;
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

                    if (!channelsByFullName.ContainsKey(fullName)) { continue; }
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
            keyframes.Sort((a, b) => Math.Sign(a.Time - b.Time));  // TODO: remove when definitely safe
            keyframesByControllerAndAddress = null;
            KeyframesChanged?.Invoke(this, EventArgs.Empty);
        }

        public ShowKeyframe AddKeyframe(float t)
        {
            int i = 0;
            for (; i < keyframes.Count; i++)
            {
                if (t <= keyframes[i].Time) { break; }
            }

            if (i < keyframes.Count && t == keyframes[i].Time)
            {
                // Select an existing keyframe whose time matches exactly instead
                return keyframes[i];
            }

            var keyframe = new ShowKeyframe() { Time = t };
            if (i >= keyframes.Count)
            {
                keyframes.Add(keyframe);
            }
            else
            {
                keyframes.Insert(i, keyframe);
            }
            NotifyKeyframesChanged();
            return keyframe;
        }

        public void RemoveKeyframe(ShowKeyframe keyframe)
        {
            if (!keyframes.Contains(keyframe)) { return; }
            keyframes.Remove(keyframe);
            NotifyKeyframesChanged();
        }
    }
}
