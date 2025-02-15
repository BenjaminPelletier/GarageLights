﻿using System;
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
        private KeyframeManager keyframeManager;
        private ShowNavigator showNavigator;
        private ShowManipulator showManipulator;
        private ChannelSelector channelSelector;

        public ControlPanel()
        {
            InitializeComponent();
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

        public ChannelSelector ChannelSelector
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

        private void SetButtonStates()
        {
            bool noChannelsSelected = channelSelector == null || !channelSelector.GetSelectedChannelElements().Any();
            bool activeKeyframe = showNavigator != null && showNavigator.ActiveKeyframe != null;

            tsbRemoveKeyframe.Enabled = activeKeyframe;
            tsbMoveKeyframe.Enabled = activeKeyframe;
            tsbEraseChecked.Enabled = activeKeyframe && !noChannelsSelected;
            tsbWrite.Enabled = activeKeyframe && !noChannelsSelected;
            tsbRecordStart.Enabled = activeKeyframe && !noChannelsSelected;
        }

        private void showNavigator_ActiveKeyframeChanged(object sender, EventArgs e)
        {
            if (InvokeRequired) { Invoke((Action<object, EventArgs>)showNavigator_ActiveKeyframeChanged, new object[] { sender, e }); return; }

            SetButtonStates();
        }

        private void channelSelector_SelectedChannelsChanged(object sender, EventArgs e)
        {
            SetButtonStates();
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

            SetButtonStates();
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
            if (keyframeManager != null && keyframeManager.Keyframes.Count > 0 && keyframeManager.Keyframes[0].Time == 0)
            {
                showNavigator.ActiveKeyframe = keyframeManager.Keyframes[0];
            }
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
