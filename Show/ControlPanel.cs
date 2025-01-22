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

namespace GarageLights.Show
{
    internal partial class ControlPanel : UserControl
    {
        private AudioPlayer audioPlayer;
        private IKeyframeManger keyframeManager;

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

        public IKeyframeManger KeyframeManager
        {
            set
            {
                keyframeManager = value;
                keyframeManager.ActiveKeyframeChanged += KeyframeControl_ActiveKeyframeChanged;
                KeyframeControl_ActiveKeyframeChanged(this, EventArgs.Empty);
            }
        }

        private void SeekKeyframe(int di)
        {
            if (audioPlayer == null || !audioPlayer.IsAudioLoaded) { return; }
            if (keyframeManager == null || keyframeManager.Keyframes.Count == 0) { return; }

            float currentTime = audioPlayer.AudioPosition;

            ShowKeyframe bestMatch = null;
            foreach (ShowKeyframe f in keyframeManager.Keyframes)
            {
                if (di > 0)
                {
                    if (f.Time <= currentTime) { continue; }
                    if (bestMatch == null || f.Time < bestMatch.Time)
                    {
                        bestMatch = f;
                    }
                }
                else
                {
                    if (f.Time >= currentTime) { continue; }
                    if (bestMatch == null || f.Time > bestMatch.Time)
                    {
                        bestMatch = f;
                    }
                }
            }
            if (bestMatch != null)
            {
                keyframeManager.ActiveKeyframe = bestMatch;
                if (!audioPlayer.Playing)
                {
                    audioPlayer.AudioPosition = bestMatch.Time;
                }
            }
        }

        private void KeyframeControl_ActiveKeyframeChanged(object sender, EventArgs e)
        {
            tsbRemoveKeyframe.Enabled = keyframeManager.ActiveKeyframe != null;
        }

        private void tsbRemoveKeyframe_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void tsbAddKeyframe_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void tsbPreviousKeyframe_Click(object sender, EventArgs e)
        {
            SeekKeyframe(-1);
        }

        private void tsbNextKeyframe_Click(object sender, EventArgs e)
        {
            SeekKeyframe(1);
        }

        private void tsbGoToBeginning_Click(object sender, EventArgs e)
        {
            if (audioPlayer != null && audioPlayer.IsAudioLoaded)
            {
                audioPlayer.AudioPosition = 0;
            }
        }

        private void tsbGoToEnd_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
