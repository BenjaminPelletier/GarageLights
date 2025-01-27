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
        private IChannelSelector channelSelector;

        private IChannelInputDevice channelInputDevice;

        private Dictionary<int, int> lastChannelValues;
        private ThrottledUiCall<bool> inputChannelValuesUpdated;

        public ControlPanel()
        {
            InitializeComponent();
            inputChannelValuesUpdated = new ThrottledUiCall<bool>(this, ControlPanel_InputChannelValuesUpdated);
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

        public IChannelInputDevice ChannelInputDevice
        {
            set
            {
                if (channelInputDevice != null)
                {
                    channelInputDevice.ChannelValuesChanged -= channelInputDevice_ChannelValuesChanged;
                }
                lastChannelValues = null;
                tsbWrite.Enabled = false;
                channelInputDevice = value;
                if (channelInputDevice != null)
                {
                    channelInputDevice.ChannelValuesChanged += channelInputDevice_ChannelValuesChanged;
                }
            }
        }

        private void showNavigator_ActiveKeyframeChanged(object sender, EventArgs e)
        {
            if (tsbRecordStop.Visible)
            {
                tsbRecordStop.Visible = false;
                tsbRecordStart.Visible = true;
            }
            tsbRemoveKeyframe.Enabled = showNavigator.ActiveKeyframe != null;
            tsbMoveKeyframe.Enabled = showNavigator.ActiveKeyframe != null;
            tsbWrite.Enabled = showNavigator.ActiveKeyframe != null;
            tsbEraseChecked.Enabled = showNavigator.ActiveKeyframe != null;
        }

        private void channelSelector_SelectedChannelsChanged(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        private void channelInputDevice_ChannelValuesChanged(object sender, ChannelValuesChangedEventArgs e)
        {
            lastChannelValues = e.ChannelValues;
            inputChannelValuesUpdated.Trigger(true);
        }

        private void ControlPanel_InputChannelValuesUpdated(bool _)
        {
            tsbWrite.Enabled = lastChannelValues != null;
            if (tsbRecordStop.Visible)
            {
                WriteInputValuesToSelectedChannels();
            }
        }

        private void tsbRemoveKeyframe_Click(object sender, EventArgs e)
        {
            if (showNavigator != null && showNavigator.ActiveKeyframe != null)
            {
                keyframeManager.RemoveKeyframe(showNavigator.ActiveKeyframe);
            }
        }

        private void tsbAddKeyframe_Click(object sender, EventArgs e)
        {
            if (audioPlayer == null || !audioPlayer.IsAudioLoaded || keyframeManager == null || showNavigator == null) { return; }
            ShowKeyframe keyframe = keyframeManager.AddKeyframe(audioPlayer.AudioPosition);
            showNavigator.ActiveKeyframe = keyframe;
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
            if (audioPlayer != null && audioPlayer.IsAudioLoaded)
            {
                audioPlayer.AudioPosition = 0;
            }
        }

        private void tsbMoveKeyframe_Click(object sender, EventArgs e)
        {
            if (showNavigator.ActiveKeyframe != null && audioPlayer != null)
            {
                showNavigator.ActiveKeyframe.Time = audioPlayer.AudioPosition;
                keyframeManager.NotifyKeyframesChanged();
            }
        }

        private void tsbWrite_Click(object sender, EventArgs e)
        {
            WriteInputValuesToSelectedChannels();
        }

        private void WriteInputValuesToSelectedChannels() {
            if (lastChannelValues == null || channelSelector == null || audioPlayer == null) { return; }

            // Get the keyframe to modify
            ShowKeyframe keyframe = showNavigator.ActiveKeyframe;
            if (keyframe == null)
            {
                // No active keyframe; add a new one
                keyframe = keyframeManager.AddKeyframe(audioPlayer.AudioPosition);
                showNavigator.ActiveKeyframe = keyframe;
            }

            // Get the channels for this keyframe
            Dictionary<string, ChannelKeyframe> channels = keyframe.Channels;
            if (channels == null)
            {
                channels = new Dictionary<string, ChannelKeyframe>();
                keyframe.Channels = channels;
            }

            // Modify each selected channel
            ChannelNodeTreeNode[] checkedNodes = channelSelector.GetCheckedChannelNodeTreeNodes().ToArray();
            bool keyframesChanged = false;
            foreach (var kvp in lastChannelValues)
            {
                if (kvp.Key < checkedNodes.Length)
                {
                    string fullName = checkedNodes[kvp.Key].FullName;
                    if (!channels.ContainsKey(fullName))
                    {
                        channels[fullName] = new ChannelKeyframe();
                        keyframesChanged = true;
                    }
                    if (channels[fullName].Value != kvp.Value)
                    {
                        channels[fullName].Value = kvp.Value;
                        // TODO: use specific interpolation method
                        keyframesChanged = true;
                    }
                }
            }
            if (keyframesChanged)
            {
                keyframeManager.NotifyKeyframesChanged();
            }
        }

        private void tsbRecordStart_Click(object sender, EventArgs e)
        {
            tsbRecordStart.Visible = false;
            tsbRecordStop.Visible = true;
        }

        private void tsbRecordStop_Click(object sender, EventArgs e)
        {
            tsbRecordStop.Visible = false;
            tsbRecordStart.Visible = true;
        }

        private void tsbEraseChecked_Click(object sender, EventArgs e)
        {
            if (showNavigator == null || showNavigator.ActiveKeyframe == null) { return; }

            bool keyframesUpdated = false;
            foreach (ChannelNodeTreeNode node in channelSelector.GetCheckedChannelNodeTreeNodes())
            {
                string fullName = node.FullName;
                if (showNavigator.ActiveKeyframe.Channels.ContainsKey(fullName))
                {
                    showNavigator.ActiveKeyframe.Channels.Remove(fullName);
                    keyframesUpdated = true;
                }
            }
            if (keyframesUpdated)
            {
                keyframeManager.NotifyKeyframesChanged();
            }
        }
    }
}
