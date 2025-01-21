using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageLights.Show
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

    class Keyframe
    {
        public float Time;
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
