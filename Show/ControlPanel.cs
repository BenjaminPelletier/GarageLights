using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GarageLights.Audio;
using GarageLights.Keyframes;
using GarageLights.InputDevices.Definitions;
using GarageLights.Channels;
using GarageLights.UI;

namespace GarageLights.Show
{
    internal partial class ControlPanel : UserControl
    {
        private AudioPlayer audioPlayer;
        private KeyframeManager keyframeManager;
        private ShowNavigator showNavigator;
        private ShowManipulator showManipulator;
        private IChannelSelector channelSelector;

        public ControlPanel()
        {
            InitializeComponent();
        }

        public AudioPlayer AudioPlayer
        {
            set
            {
                audioPlayer = value;
            }
        }

        public KeyframeManager KeyframeManager
        {
            set
            {
                keyframeManager = value;
            }
        }

        public ShowNavigator ShowNavigator
        {
            set
            {
                if (showNavigator != null)
                {
                    showNavigator.ActiveKeyframeChanged -= showNavigator_ActiveKeyframeChanged;
                }
                showNavigator = value;
                showNavigator.ActiveKeyframeChanged += showNavigator_ActiveKeyframeChanged;
                showNavigator_ActiveKeyframeChanged(this, EventArgs.Empty);
            }
        }

        public ShowManipulator ShowManipulator
        {
            set
            {
                if (showManipulator != null)
                {
                    showManipulator.RecordingChanged -= showManipulator_RecordingChanged;
                    showManipulator.ChannelInputAvailableChanged -= showManipulator_ChannelInputAvailableChanged;
                }
                showManipulator = value;
                showManipulator.RecordingChanged += showManipulator_RecordingChanged;
                showManipulator.ChannelInputAvailableChanged += showManipulator_ChannelInputAvailableChanged;
            }
        }

        public IChannelSelector ChannelSelector
        {
            set
            {
                if (channelSelector != null)
                {
                    channelSelector.SelectedChannelsChanged -= channelSelector_SelectedChannelsChanged;
                }
                channelSelector = value;
                channelSelector.SelectedChannelsChanged += channelSelector_SelectedChannelsChanged;
            }
        }

        private void showNavigator_ActiveKeyframeChanged(object sender, EventArgs e)
        {
            if (InvokeRequired) { Invoke((Action<object, EventArgs>)showNavigator_ActiveKeyframeChanged, new object[] { sender, e }); return; }

            tsbRemoveKeyframe.Enabled = showNavigator.ActiveKeyframe != null;
            tsbMoveKeyframe.Enabled = showNavigator.ActiveKeyframe != null;
            tsbWrite.Enabled = showNavigator.ActiveKeyframe != null;
            tsbEraseChecked.Enabled = showNavigator.ActiveKeyframe != null;
        }

        private void channelSelector_SelectedChannelsChanged(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void showManipulator_RecordingChanged(object sender, FeatureAvailabilityEventArgs e)
        {
            if (InvokeRequired) { Invoke((Action<object, FeatureAvailabilityEventArgs>)showManipulator_RecordingChanged, new object[] { sender, e }); return; }

            tsbRecordStart.Visible = !e.Enabled;
            tsbRecordStop.Visible = e.Enabled;
        }

        private void showManipulator_ChannelInputAvailableChanged(object sender, FeatureAvailabilityEventArgs e)
        {
            if (InvokeRequired) { Invoke((Action<object, FeatureAvailabilityEventArgs>)showManipulator_ChannelInputAvailableChanged, new object[] { sender, e }); return; }

            tsbWrite.Enabled = e.Enabled;
            tsbRecordStart.Enabled = e.Enabled;
            tsbRecordStop.Enabled = e.Enabled;
        }

        private void tsbRemoveKeyframe_Click(object sender, EventArgs e)
        {
            showManipulator.RemoveActiveKeyframe();
        }

        private void tsbAddKeyframe_Click(object sender, EventArgs e)
        {
            showManipulator.AddBlankKeyframeAtAudioPosition();
        }

        private void tsbPreviousKeyframe_Click(object sender, EventArgs e)
        {
            showNavigator.SeekKeyframe(false);
        }

        private void tsbNextKeyframe_Click(object sender, EventArgs e)
        {
            showNavigator.SeekKeyframe(true);
        }

        private void tsbGoToBeginning_Click(object sender, EventArgs e)
        {
            showNavigator.GoToBeginning();
        }

        private void tsbMoveKeyframe_Click(object sender, EventArgs e)
        {
            showManipulator.MoveActiveKeyframeToAudioPosition();
        }

        private void tsbWrite_Click(object sender, EventArgs e)
        {
            showManipulator.WriteInputValuesToSelectedChannels();
        }

        private void tsbRecordStart_Click(object sender, EventArgs e)
        {
            showManipulator.Recording = true;
        }

        private void tsbRecordStop_Click(object sender, EventArgs e)
        {
            showManipulator.Recording = false;
        }

        private void tsbEraseChecked_Click(object sender, EventArgs e)
        {
            showManipulator.EraseCheckedChannels();
        }
    }
}
