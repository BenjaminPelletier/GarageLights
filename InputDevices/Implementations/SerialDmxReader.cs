using GarageLights.InputDevices.Definitions;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GarageLights.InputDevices.Implementations
{
    /// <summary>
    /// Interface to DMX reader using DMXReaderSketch
    /// </summary>
    class SerialDmxReader : IChannelInputDevice
    {
        public event EventHandler<ChannelValuesChangedEventArgs> ChannelValuesChanged;
        public event EventHandler<ChannelInputDeviceErrorEventArgs> Error;
        public event EventHandler<DmxReaderMessageEventArgs> Message;

        SerialPort port;
        int firstChannel;
        TaskCompletionSource<bool> listenTcs;

        public SerialDmxReader(Definitions.SerialDmxReader definition)
        {
            firstChannel = definition.FirstChannel;
            port = new SerialPort(definition.Port, 115200, Parity.None, 8, StopBits.One);
            port.ReadTimeout = 1000;
            port.Open();
            RunListenThread();
        }

        private void RunListenThread()
        {
            var tcs = new TaskCompletionSource<bool>();
            listenTcs = tcs;

            new Thread(() =>
            {
                try
                {
                    while (!tcs.Task.IsCanceled)
                    {
                        string line = port.ReadLine();
                        if (line[0] == '!')
                        {
                            Error?.Invoke(this, new ChannelInputDeviceErrorEventArgs("Error reported from device: " + line.Substring(1)));
                            continue;
                        }
                        else if (line[1] == '@')
                        {
                            Message?.Invoke(this, new DmxReaderMessageEventArgs(line.Substring(1)));
                            continue;
                        }

                        string[] cols = line.Trim().Split(' ');
                        if (cols.Any(c => c.Length != 2))
                        {
                            Error?.Invoke(this, new ChannelInputDeviceErrorEventArgs("Invalid data: '" + line + "'"));
                            continue;
                        }
                        if (cols.Length <= firstChannel) { continue; }

                        int[] values;
                        try
                        {
                            values = cols.Skip(firstChannel).Select(c => int.Parse(c, System.Globalization.NumberStyles.HexNumber)).ToArray();
                        }
                        catch (FormatException ex)
                        {
                            Error?.Invoke(this, new ChannelInputDeviceErrorEventArgs(new ChannelInputDeviceException("Invalid value in data: " + ex.ToString(), ex)));
                            continue;
                        }

                        Dictionary<int, int> valueDict = Enumerable.Range(0, values.Length).ToDictionary(i => i, i => values[i]);
                        ChannelValuesChanged?.Invoke(this, new ChannelValuesChangedEventArgs(valueDict));
                    }
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                    Error?.Invoke(this, new ChannelInputDeviceErrorEventArgs(new ChannelInputDeviceException("There was a " + ex.GetType().Name + " while listening to the SerialDmxReader device: " + ex.ToString(), ex)));
                }
                finally
                {
                    tcs.TrySetResult(true);
                }
            })
            { IsBackground = true }.Start();
        }

        public void Dispose()
        {
            listenTcs.TrySetCanceled();
            listenTcs.Task.Wait();
            if (port.IsOpen) { port.Close(); }
            port.Dispose();
        }
    }

    public class DmxReaderMessageEventArgs : EventArgs
    {
        public readonly string Message;

        public DmxReaderMessageEventArgs(string message)
        {
            Message = message;
        }
    }
}
