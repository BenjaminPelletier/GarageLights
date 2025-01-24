using GarageLights.InputDevices.Definitions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GarageLights.InputDevices.UI
{
    partial class ChannelInputDeviceSelector : Form
    {
        private ChannelInputDevice channelInputDevice;

        public ChannelInputDeviceSelector()
        {
            InitializeComponent();
        }

        public ChannelInputDevice ChannelInputDevice
        {
            get { return channelInputDevice; }
            set
            {
                if (value == null)
                {
                    tabControl1.SelectedTab = tpNone;
                }
                else if (value.DmxReader != null)
                {
                    tabControl1.SelectedTab = tpSerialDmxReader;
                    cbPort.Text = value.DmxReader.Port;
                    nudFirstChannel.Value = value.DmxReader.FirstChannel;
                }
                else
                {
                    tabControl1.SelectedTab = tpNone;
                }
            }
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void bOk_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tpSerialDmxReader)
            {
                string port = cbPort.Text;
                if (!SerialPort.GetPortNames().Contains(port))
                {
                    tsslError.Text = "Serial port '" + port + "' does not exist";
                    SelectSerialDmxReader();
                    cbPort.Text = "";
                    return;
                }
                int firstChannel = (int)nudFirstChannel.Value;
                channelInputDevice = new ChannelInputDevice() { DmxReader = new SerialDmxReader() { Port = port, FirstChannel = firstChannel } };
            }
            else
            {
                channelInputDevice = null;
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if (e.TabPage == tpSerialDmxReader)
            {
                SelectSerialDmxReader();
            }
        }

        private void SelectSerialDmxReader()
        {
            cbPort.Items.Clear();
            cbPort.Items.AddRange(SerialPort.GetPortNames());
        }
    }
}
