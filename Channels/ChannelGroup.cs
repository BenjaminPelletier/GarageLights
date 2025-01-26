using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageLights.Channels
{
    internal class ChannelGroup
    {
        public string Controller;
        public int Address;
        public List<ChannelNode> Nodes;
    }
}
