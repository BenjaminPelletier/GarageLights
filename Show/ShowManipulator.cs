using GarageLights.Audio;
using GarageLights.Channels;
using GarageLights.InputDevices.Definitions;
using GarageLights.Keyframes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageLights.Show
{
    class ShowManipulator
    {
        AudioPlayer audioPlayer;
        KeyframeManager keyframeManager;
        ShowNavigator showNavigator;
        IChannelSelector channelSelector;
        private IChannelInputDevice channelInputDevice;

        private Dictionary<int, int> lastChannelValues;
        private bool recording = false;

        public event EventHandler<FeatureAvailabilityEventArgs> RecordingChanged;
        public event EventHandler<FeatureAvailabilityEventArgs> ChannelInputAvailableChanged;

        public ShowManipulator(AudioPlayer audioPlayer, KeyframeManager keyframeManager, ShowNavigator showNavigator)
        {
            this.audioPlayer = audioPlayer;
            this.keyframeManager = keyframeManager;
            this.showNavigator = showNavigator;
            showNavigator.ActiveKeyframeChanged += showNavigator_ActiveKeyframeChanged;
        }

        public IChannelSelector ChannelSelector
        {
            set
            {
                channelSelector = value;
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
                channelInputDevice = value;
                lastChannelValues = null;
                ChannelInputAvailableChanged?.Invoke(this, new FeatureAvailabilityEventArgs(false));
                if (channelInputDevice != null)
                {
                    channelInputDevice.ChannelValuesChanged += channelInputDevice_ChannelValuesChanged;
                }
            }
        }

        public bool Recording
        {
            get { return recording; }
            set
            {
                if (value != recording)
                {
                    recording = value;
                    RecordingChanged?.Invoke(this, new FeatureAvailabilityEventArgs(recording));
                }
            }
        }

        private void showNavigator_ActiveKeyframeChanged(object sender, EventArgs e)
        {
            Recording = false;
        }

        private void channelInputDevice_ChannelValuesChanged(object sender, ChannelValuesChangedEventArgs e)
        {
            bool newInputAvailable = lastChannelValues == null;
            lastChannelValues = e.ChannelValues;
            ChannelInputAvailableChanged?.Invoke(this, new FeatureAvailabilityEventArgs(true));
            if (recording)
            {
                WriteInputValuesToSelectedChannels();
            }
        }

        public void WriteInputValuesToSelectedChannels()
        {
            if (lastChannelValues == null || channelSelector == null) { return; }

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

        public void EraseCheckedChannels()
        {
            if (showNavigator == null || showNavigator.ActiveKeyframe == null || channelSelector == null) { return; }

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

        public void RemoveActiveKeyframe()
        {
            if (showNavigator.ActiveKeyframe != null)
            {
                keyframeManager.RemoveKeyframe(showNavigator.ActiveKeyframe);
            }
        }

        public void AddBlankKeyframeAtAudioPosition()
        {
            ShowKeyframe keyframe = keyframeManager.AddKeyframe(audioPlayer.AudioPosition);
            showNavigator.ActiveKeyframe = keyframe;
        }

        public void MoveActiveKeyframeToAudioPosition()
        {
            if (showNavigator.ActiveKeyframe != null)
            {
                showNavigator.ActiveKeyframe.Time = audioPlayer.AudioPosition;
                keyframeManager.NotifyKeyframesChanged();
            }
        }
    }

    class FeatureAvailabilityEventArgs : EventArgs
    {
        public readonly bool Enabled;

        public FeatureAvailabilityEventArgs(bool enabled)
        {
            Enabled = enabled;
        }
    }
}
