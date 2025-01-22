using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageLights.Keyframes
{
    enum KeyframeStyle
    {
        Linear,
        Step,
    }

    class ChannelKeyframe
    {
        public int Value;
        public KeyframeStyle Style = KeyframeStyle.Linear;
    }

    class ShowKeyframe
    {
        public float Time;

        /// <summary>
        /// Key: Full name of channel
        /// Value: Value for that channel at this keyframe
        /// </summary>
        public Dictionary<string, ChannelKeyframe> Channels;
    }

    class TimedChannelKeyframe
    {
        public float Time;
        public ChannelKeyframe Keyframe;

        public TimedChannelKeyframe(float time, ChannelKeyframe keyframe)
        {
            Time = time;
            Keyframe = keyframe;
        }
    }
}
