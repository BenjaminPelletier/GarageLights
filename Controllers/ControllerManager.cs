using GarageLights.Keyframes;
using GarageLights.Lights;
using GarageLights.Show;
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
                    int value = Interpolate(tController, addressKeyframes);
                    addressValues[address] = value;
                }
                controller.SetChannels(addressValues);
            }
        }

        private static int Interpolate(float t, List<TimedChannelKeyframe> keyframes)
        {
            // Short-circuit if we're before the start of keyframes or after the end
            if (t < keyframes[0].Time)
            {
                return 0;
            }
            if (t >= keyframes[keyframes.Count - 1].Time)
            {
                return keyframes[keyframes.Count - 1].Keyframe.Value;
            }

            // Find the keyframe pair we're in between
            int i = 1;  // Index of second keyframe to interpolate between
            while (i <= keyframes.Count - 2)
            {
                if (t <= keyframes[i].Time) { break; }
                i++;
            }

            // Interpolate within keyframe pair
            if (keyframes[i].Keyframe.Style == KeyframeStyle.Step)
            {
                return keyframes[i - 1].Keyframe.Value;
            }
            else if (keyframes[i].Keyframe.Style == KeyframeStyle.Linear)
            {
                float t0 = keyframes[i - 1].Time;
                int v0 = keyframes[i - 1].Keyframe.Value;
                float t1 = keyframes[i].Time;
                int v1 = keyframes[i].Keyframe.Value;
                return (int)(v0 + (v1 - v0) * (t - t0) / (t1 - t0));
            }
            else
            {
                throw new NotImplementedException();
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
