using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageLights.InputDevices.Definitions
{
    public class ChannelValuesChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Channel values (key = zero-based channel address, value = channel value)
        /// </summary>
        public readonly Dictionary<int, int> ChannelValues;

        public ChannelValuesChangedEventArgs(Dictionary<int, int> channelValues)
        {
            ChannelValues = channelValues;
        }
    }

    public class ChannelInputDeviceException : Exception
    {
        public ChannelInputDeviceException(string message) : base(message) { }
        public ChannelInputDeviceException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class ChannelInputDeviceErrorEventArgs
    {
        public readonly ChannelInputDeviceException Exception;

        public ChannelInputDeviceErrorEventArgs(ChannelInputDeviceException ex)
        {
            Exception = ex;
        }

        public ChannelInputDeviceErrorEventArgs(string message)
        {
            Exception = new ChannelInputDeviceException(message);
        }
    }

    interface IChannelInputDevice : IDisposable
    {
        event EventHandler<ChannelValuesChangedEventArgs> ChannelValuesChanged;
        event EventHandler<ChannelInputDeviceErrorEventArgs> Error;
    }
}
