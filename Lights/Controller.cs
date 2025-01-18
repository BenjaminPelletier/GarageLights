using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageLights.Lights
{
    internal class Controller
    {
        public string Name;
        public SerialDmx SerialDmx;
        public Wemo Wemo;
    }

    internal class SerialDmx
    {
        public string Port;
    }

    internal class Wemo
    {
        public string IpAddress;
    }
}
