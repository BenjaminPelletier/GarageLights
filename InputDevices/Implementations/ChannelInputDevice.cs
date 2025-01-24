using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageLights.InputDevices.Implementations
{
    static class ChannelInputDevice
    {
        public static Definitions.IChannelInputDevice Create(Definitions.ChannelInputDevice definition)
        {
            if (definition.DmxReader != null)
            {
                return new SerialDmxReader(definition.DmxReader);
            }
            else
            {
                throw new NotImplementedException("Cannot determine how to create ChannelInputDevice");
            }
        }
    }
}
