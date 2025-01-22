using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageLights.Keyframes
{
    interface IKeyframeManger
    {
        List<ShowKeyframe> Keyframes { get; }

        ShowKeyframe ActiveKeyframe { get; set; }

        Dictionary<string, Dictionary<int, List<TimedChannelKeyframe>>> KeyframesByControllerAndAddress { get; }

        event EventHandler ActiveKeyframeChanged;
    }
}
