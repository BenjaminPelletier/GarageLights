using GarageLights.Lights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageLights.Controllers
{
    class WemoController : IController
    {
        private float latency;

        public WemoController(Wemo definition, float latency)
        {
            this.latency = latency;
        }

        public void ClearChannels(IEnumerable<int> addresses)
        {
            
        }

        public float GetLatency()
        {
            return latency;
        }

        public void SetChannels(Dictionary<int, int> addressValues)
        {
            
        }

        public void Dispose()
        {

        }
    }
}
