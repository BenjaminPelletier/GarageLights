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

    class Keyframe
    {
        public float Time;
        public int Value;
        public KeyframeStyle Style = KeyframeStyle.Linear;
    }
}
