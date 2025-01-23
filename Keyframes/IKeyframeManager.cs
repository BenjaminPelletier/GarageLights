using GarageLights.Lights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageLights.Keyframes
{
    interface IKeyframeManager
    {
        List<ShowKeyframe> Keyframes { get; set; }

        ShowKeyframe ActiveKeyframe { get; set; }

        Dictionary<string, Dictionary<int, List<TimedChannelKeyframe>>> GetKeyframesByControllerAndAddress(IEnumerable<ChannelNode> nodes);

        event EventHandler ActiveKeyframeChanged;
    }
}
