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

namespace GarageLights.Show
{
    internal partial class ControlPanel : UserControl
    {
        private AudioPlayer audioPlayer;
        private KeyframeManager keyframeManager;
        private ShowNavigator showNavigator;

        private IChannelInputDevice channelInputDevice;
        private Dictionary<int, int> lastChannelValues;

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
            tsbRemoveKeyframe.Enabled = showNavigator.ActiveKeyframe != null;
            tsbMoveKeyframe.Enabled = showNavigator.ActiveKeyframe != null;
            tsbWrite.Enabled = showNavigator.ActiveKeyframe != null;
        }

        private void channelInputDevice_ChannelValuesChanged(object sender, ChannelValuesChangedEventArgs e)
        {
            bool inform = lastChannelValues == null;

            lastChannelValues = e.ChannelValues;

            if (inform)
            {
                BeginInvoke((Action)(() =>
                {
                    tsbWrite.Enabled = true;
                }));
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
    }
}
