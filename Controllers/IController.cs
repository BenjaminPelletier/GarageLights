using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageLights.Controllers
{
    internal interface IController
    {
        void SetChannel(int address, int value);
    }
}
