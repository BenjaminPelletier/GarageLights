using GarageLights.Audio;
using GarageLights.Keyframes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageLights.Show
{
    [DesignerCategory("Component")]
    [ToolboxItem(true)]
    class ShowNavigator : Component
    {
        private AudioPlayer audioPlayer;
        private KeyframeManager keyframeManager;

        private ShowKeyframe activeKeyframe;

        public event EventHandler ActiveKeyframeChanged;

        public ShowNavigator(AudioPlayer audioPlayer, KeyframeManager keyframeManager)
        {
            this.audioPlayer = audioPlayer;
            this.keyframeManager = keyframeManager;
            this.keyframeManager.KeyframesChanged += keyframeManager_KeyframesChanged;
        }

        public ShowKeyframe ActiveKeyframe
        {
            get { return activeKeyframe; }
            set
            {
                if (value != activeKeyframe)
                {
                    activeKeyframe = value;
                    // TODO: Seek to new keyframe unless audio is playing?
                    ActiveKeyframeChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private void keyframeManager_KeyframesChanged(object sender, EventArgs e)
        {
            if (activeKeyframe != null && !keyframeManager.Keyframes.Contains(activeKeyframe))
            {
                ActiveKeyframe = null;
            }
        }

        public void SeekKeyframe(bool forward)
        {
            if (audioPlayer == null || !audioPlayer.IsAudioLoaded) { return; }
            if (keyframeManager == null || keyframeManager.Keyframes.Count == 0) { return; }

            float currentTime = audioPlayer.AudioPosition;

            ShowKeyframe bestMatch = null;
            foreach (ShowKeyframe f in keyframeManager.Keyframes)
            {
                if (forward)
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
                ActiveKeyframe = bestMatch;
                if (!audioPlayer.Playing)
                {
                    audioPlayer.AudioPosition = bestMatch.Time;
                }
            }
        }

    }
}
