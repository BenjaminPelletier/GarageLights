using GarageLights.Keyframes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageLights.Controllers
{
    class ControllerManager : IDisposable
    {
        Dictionary<string, IController> controllers;

        public ControllerManager(IEnumerable<Controller> controllerDefinitions)
        {
            controllers = controllerDefinitions.ToDictionary(def => def.Name, def => MakeController(def));
        }

        public void WriteValues(float t, Dictionary<string, Dictionary<int, List<TimedChannelKeyframe>>> keyframes)
        {
            foreach (var controllerAndAddressKeyframes in keyframes) {
                string controllerName = controllerAndAddressKeyframes.Key;
                var keyframesByAddress = controllerAndAddressKeyframes.Value;
                if (!controllers.ContainsKey(controllerName))
                {
                    throw new InvalidOperationException("ControllerManager unaware of controller '" + controllerName + "'");
                }
                IController controller = controllers[controllerName];
                float latency = controller.GetLatency();
                float tController = Math.Max(t - latency, 0);

                var addressValues = new Dictionary<int, int>();
                foreach (var addressAndKeyframes in keyframesByAddress)
                {
                    int address = addressAndKeyframes.Key;
                    List<TimedChannelKeyframe> addressKeyframes = addressAndKeyframes.Value;
                    int value = addressKeyframes.Interpolate(tController);
                    addressValues[address] = value;
                }
                controller.SetChannels(addressValues);
            }
        }

        private static IController MakeController(Controller controllerDefinition)
        {
            try
            {
                if (controllerDefinition.SerialDmx != null)
                {
                    return new DmxSerialController(controllerDefinition.SerialDmx, controllerDefinition.Latency);
                }
                if (controllerDefinition.Wemo != null)
                {
                    return new WemoController(controllerDefinition.Wemo, controllerDefinition.Latency);
                }
            }
            catch (Exception ex)
            {
                Debug.Print("Error making '" + controllerDefinition.Name + "' controller: " + ex.ToString());
                return new DummyController(controllerDefinition.Latency);
            }

            throw new NotImplementedException("Could not determine how to make '" + controllerDefinition.Name + "' controller");
        }

        public void Dispose()
        {
            foreach (var kvp in controllers)
            {
                kvp.Value.Dispose();
            }
            controllers.Clear();
        }
    }
}
