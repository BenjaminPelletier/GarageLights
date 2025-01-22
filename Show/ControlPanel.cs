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

namespace GarageLights.Show
{
    internal partial class ControlPanel : UserControl
    {
        private float currentTime;
        private KeyframeControl keyframeControl;

        public event EventHandler<AudioPositionChangedEventArgs> Seek;

        public ControlPanel()
        {
            InitializeComponent();
        }

        public KeyframeControl KeyframeControl
        {
            set
            {
                keyframeControl = value;
                keyframeControl.ActiveKeyframeChanged += KeyframeControl_ActiveKeyframeChanged;
                KeyframeControl_ActiveKeyframeChanged(this, EventArgs.Empty);
            }
        }

        public float CurrentTime
        {
            set
            {
                currentTime = value;
            }
        }

        public void ChangeKeyframe(int di)
        {
            if (keyframeControl.Keyframes.Count == 0) { return; }

            Keyframe bestMatch = null;
            foreach (Keyframe f in keyframeControl.Keyframes)
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
                keyframeControl.ActiveKeyframe = bestMatch;
                Seek?.Invoke(this, new AudioPositionChangedEventArgs(bestMatch.Time));
            }
        }

        private void KeyframeControl_ActiveKeyframeChanged(object sender, EventArgs e)
        {

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
            ChangeKeyframe(-1);
        }

        private void tsbNextKeyframe_Click(object sender, EventArgs e)
        {
            ChangeKeyframe(1);
        }

        private void tsbGoToBeginning_Click(object sender, EventArgs e)
        {
            Seek?.Invoke(this, new AudioPositionChangedEventArgs(0));
        }

        private void tsbGoToEnd_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
