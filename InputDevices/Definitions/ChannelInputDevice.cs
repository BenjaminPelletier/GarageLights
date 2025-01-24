using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageLights.InputDevices.Definitions
{
    class ChannelInputDevice
    {
        public SerialDmxReader DmxReader;
    }

    class SerialDmxReader
    {
        public string Port;
        public int FirstChannel = 0;
    }
}
