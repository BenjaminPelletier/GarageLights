using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageLights.Audio
{
    public class AudioViewChangedEventArgs : EventArgs
    {
        public float LeftTime { get; }
        public float RightTime { get; }

        public AudioViewChangedEventArgs(float leftTime, float rightTime)
        {
            LeftTime = leftTime;
            RightTime = rightTime;
        }
    }

    public class AudioPositionChangedEventArgs : EventArgs
    {
        public float AudioPosition { get; }

        public AudioPositionChangedEventArgs(float audioPosition)
        {
            AudioPosition = audioPosition;
        }
    }
}
