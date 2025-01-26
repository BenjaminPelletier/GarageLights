using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageLights.Channels
{
    internal class Channel
    {
        public string Controller;
        public int Address;

        public Channel OffsetAddress(int addressOffset)
        {
            return new Channel()
            {
                Controller = this.Controller,
                Address = this.Address + addressOffset
            };
        }

        public Channel WithParentController(string parentController)
        {
            if (Controller != null) { return this; }
            return new Channel()
            {
                Controller = parentController,
                Address = this.Address
            };
        }
    }
}
