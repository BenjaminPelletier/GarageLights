using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageLights.Keyframes
{
    internal static class Interpolation
    {
        public static int Interpolate(this List<TimedChannelKeyframe> keyframes, float t)
        {
            // Short-circuit if we're before the start of keyframes or after the end
            if (t < keyframes[0].Time)
            {
                return 0;
            }
            if (t >= keyframes[keyframes.Count - 1].Time)
            {
                return keyframes[keyframes.Count - 1].Keyframe.Value;
            }

            // Find the keyframe pair we're in between
            int i = 1;  // Index of second keyframe to interpolate between
            while (i <= keyframes.Count - 2)
            {
                if (t <= keyframes[i].Time) { break; }
                i++;
            }

            // Interpolate within keyframe pair
            if (keyframes[i].Keyframe.Style == KeyframeStyle.Step)
            {
                return keyframes[i - 1].Keyframe.Value;
            }
            else if (keyframes[i].Keyframe.Style == KeyframeStyle.Linear)
            {
                float t0 = keyframes[i - 1].Time;
                int v0 = keyframes[i - 1].Keyframe.Value;
                float t1 = keyframes[i].Time;
                int v1 = keyframes[i].Keyframe.Value;
                return (int)(v0 + (v1 - v0) * (t - t0) / (t1 - t0));
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
