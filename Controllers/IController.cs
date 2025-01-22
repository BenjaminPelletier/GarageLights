using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageLights.Controllers
{
    internal interface IController
    {
        /// <summary>
        /// Set the specified addresses (keys) to the specified values (values).
        /// </summary>
        /// <param name="addressValues">Keys: Addresses, Values: Values</param>
        void SetChannels(Dictionary<int, int> addressValues);

        void ClearChannels(IEnumerable<int> addresses);

        /// <summary>
        /// Get the latency for this controller.
        /// </summary>
        /// <returns>Number of seconds between commanding addresses to values and those values being observed on the device.</returns>
        float GetLatency();
    }
}
