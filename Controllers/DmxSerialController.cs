using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GarageLights.Controllers
{
    internal class DmxSerialController : IController
    {
        static readonly TimeSpan UpdatePeriod = TimeSpan.FromMilliseconds(50);

        SerialPort port;
        float latency;
        TaskCompletionSource<bool> updateTcs;
        Dictionary<int, int> addressValues;

        public DmxSerialController(SerialDmx definition, float latency)
        {
            port = new SerialPort(definition.Port, 250000, Parity.None, 8, StopBits.Two);
            port.Open();
            this.latency = latency;
            addressValues = new Dictionary<int, int>();
            RunUpdateThread();
        }

        private void RunUpdateThread()
        {
            var tcs = new TaskCompletionSource<bool>();
            updateTcs = tcs;

            new Thread(() =>
            {
                try
                {
                    DateTime tNext = DateTime.UtcNow;
                    while (!tcs.Task.IsCanceled)
                    {
                        Update();
                        tNext += UpdatePeriod;
                        TimeSpan dt = tNext - DateTime.UtcNow;
                        while (dt < TimeSpan.Zero)
                        {
                            tNext += UpdatePeriod;
                            dt = tNext - DateTime.UtcNow;
                        }
                        Thread.Sleep(dt);
                    }
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
                finally
                {
                    tcs.TrySetResult(true);
                }
            })
            { IsBackground = true }.Start();
        }

        private void Update()
        {
            // Send a break (hold low for more than the length of a character)
            port.BaudRate = 125000;
            port.Write("\0");
            port.Flush();
            port.BaudRate = 250000;

            // Construct a frame of DMX data
            byte[] data;
            lock (addressValues)
            {
                int maxAddress = addressValues.Count >= 1 ? addressValues.Keys.Max() : 0;
                if (maxAddress >= 512)
                {
                    throw new InvalidOperationException("Attempted to update address " + maxAddress + " which exceeds 512");
                }
                data = new byte[maxAddress + 1];
                data[0] = 0;
                foreach (var kvp in addressValues)
                {
                    data[kvp.Key] = (byte)kvp.Value;
                }
            }

            // Write DMX frame to serial port
            port.Write(data, 0, data.Length);
            port.Flush();
        }

        private void ThrowForUpdateOutcome()
        {
            var task = updateTcs.Task;
            if (task.IsFaulted)
            {
                throw new InvalidOperationException("An error occurred while sending DMX updates to the serial port", task.Exception);
            }
            else if (task.IsCanceled)
            {
                throw new InvalidOperationException("Cannot interact with DmxSerialController after its Update loop has been stopped");
            }
            else if (task.IsCompleted)
            {
                throw new InvalidOperationException("DmxSerialController's Update loop unexpectedly stopped");
            }
        }

        public void SetChannels(Dictionary<int, int> addressValues)
        {
            ThrowForUpdateOutcome();
            lock (this.addressValues)
            {
                foreach (var kvp in addressValues)
                {
                    this.addressValues[kvp.Key] = kvp.Value;
                }
            }
        }

        public void ClearChannels(IEnumerable<int> addresses)
        {
            ThrowForUpdateOutcome();
            lock (addressValues)
            {
                foreach (int address in addresses)
                {
                    addressValues.Remove(address);
                }
            }
        }

        public float GetLatency()
        {
            ThrowForUpdateOutcome();
            return latency;
        }

        public void Dispose()
        {
            updateTcs.TrySetCanceled();
            updateTcs.Task.Wait();
            if (port.IsOpen) { port.Close(); }
            port.Dispose();
            updateTcs.TrySetCanceled();
        }
    }

    static class SerialPortExtensions
    {
        public static void Flush(this SerialPort port)
        {
            while (port.BytesToWrite > 0)
            {
                Thread.Sleep(1);
            }
        }
    }
}
