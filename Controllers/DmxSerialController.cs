using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageLights.Controllers
{
    internal class DmxSerialController : IController
    {
        public DmxSerialController(string port)
        {
            // TODO
        }

        public void SetChannel(int address, int value)
        {
            throw new NotImplementedException();
        }
    }
}
