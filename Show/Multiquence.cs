using GarageLights.Audio;
using GarageLights.Lights;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GarageLights.Show
{
    internal partial class Multiquence : UserControl
    {
        private bool designMode;
        private Project project;

        public event EventHandler<AudioFileEventArgs> AudioFileChanged;
        public event EventHandler<AudioPositionChangedEventArgs> AudioPositionChanged;
        public event EventHandler<AudioControl.PlaybackErrorEventArgs> PlaybackError;

        /// <summary>
        /// Playback continued, with a new audio position.
        /// This event is invoked on the playback thread rather than the UI thread.
        /// </summary>
        public event EventHandler<PlaybackContinuedEventArgs> PlaybackContinued;

        public Multiquence()
        {
            designMode = LicenseManager.UsageMode == LicenseUsageMode.Designtime;
            InitializeComponent();
            tvChannels.NodeLayoutChanged += tvChannels_NodeLayoutChanged;
            keyframeControl1.RowSource = tvChannels;
            controlPanel1.KeyframeControl = keyframeControl1;
        }

        public Project Project
        {
            get { return project; }
            set
            {
                project = value;

                tvChannels.Nodes.Clear();
                if (project != null)
                {
                    tvChannels.Nodes.AddRange(
                        project.ChannelNodes
                            .Select(n => ChannelNodeTreeNode.FromChannelNode(n))
                            .ToArray()
                    );

                    if (project.Keyframes == null)
                    {
                        project.Keyframes = new List<Keyframe>();
                    }
                    keyframeControl1.Keyframes = project.Keyframes;

                    if (project.AudioFile != null)
                    {
                        audioControl1.LoadAudio(project.AudioFile);
                    }
                }
            }
        }

        public void Play()
        {
            audioControl1.Play();
        }

        public void Stop()
        {
            audioControl1.Stop();
        }

        private void toolPanel1_Play(object sender, EventArgs e)
        {
            audioControl1.Play();
        }

        private void toolPanel1_Stop(object sender, EventArgs e)
        {
            audioControl1.Stop();
        }

        private void splitContainer1_Panel1_Resize(object sender, EventArgs e)
        {
            tvChannels.Width = splitContainer1.Panel1.Width - 2 * tvChannels.Left;
            tvChannels.Height = splitContainer1.Panel1.Height - tvChannels.Top - tvChannels.Left;
            controlPanel1.Width = tvChannels.Width;
        }

        private void splitContainer1_Panel2_Resize(object sender, EventArgs e)
        {
            showScroller1.Width = splitContainer1.Panel2.Width - 2 * showScroller1.Left;
            audioControl1.Width = showScroller1.Width;
            keyframeControl1.Width = showScroller1.Width;
            keyframeControl1.Height = ClientSize.Height - keyframeControl1.Top - showScroller1.Left;
        }

        private void audioControl1_FileLoadRequested(object sender, EventArgs e)
        {
            if (ofdAudioFile.ShowDialog() == DialogResult.OK)
            {
                audioControl1.LoadAudio(ofdAudioFile.FileName);
                AudioFileChanged?.Invoke(this, new AudioFileEventArgs(ofdAudioFile.FileName));
            }
        }

        private void audioControl1_AudioLoaded(object sender, EventArgs e)
        {
            keyframeControl1.MaxTime = audioControl1.AudioLength;
            showScroller1.MaxTime = audioControl1.AudioLength;
        }

        private void audioControl1_AudioViewChanged(object sender, AudioViewChangedEventArgs e)
        {
            keyframeControl1.SetTimeRange(e.LeftTime, e.RightTime);
            showScroller1.SetTimeRange(e.LeftTime, e.RightTime);
        }

        private void audioControl1_AudioPositionChanged(object sender, AudioPositionChangedEventArgs e)
        {
            controlPanel1.CurrentTime = e.AudioPosition;
            keyframeControl1.CurrentTime = e.AudioPosition;
            showScroller1.CurrentTime = e.AudioPosition;
            AudioPositionChanged?.Invoke(this, e);
        }

        private void tvChannels_NodeLayoutChanged(object sender, EventArgs e)
        {
            keyframeControl1.Invalidate();
        }

        public class AudioFileEventArgs : EventArgs
        {
            public readonly string FileName;

            public AudioFileEventArgs(string fileName)
            {
                FileName = fileName;
            }
        }

        private void showScroller1_AudioViewChange(object sender, AudioViewChangedEventArgs e)
        {
            audioControl1.UpdateAudioView(e.LeftTime, e.RightTime);
        }

        private void audioControl1_PlaybackContinued(object sender, AudioPositionChangedEventArgs e)
        {
            if (PlaybackContinued != null)
            {
                var keyframes = keyframeControl1.KeyframesByControllerAndAddress;
                PlaybackContinued.Invoke(this, new PlaybackContinuedEventArgs(e.AudioPosition, keyframes));
            }
        }

        private void audioControl1_PlaybackError(object sender, AudioControl.PlaybackErrorEventArgs e)
        {
            PlaybackError?.Invoke(this, e);
        }

        private void controlPanel1_Seek(object sender, AudioPositionChangedEventArgs e)
        {
            audioControl1.AudioPosition = e.AudioPosition;
        }
    }

    internal class PlaybackContinuedEventArgs : EventArgs
    {
        public float AudioPosition { get; }

        /// <summary>
        /// Per-controller, per-address keyframe information.
        /// Key: Controller name
        /// Value:
        ///     Key: Address
        ///     Value: Value keyframes for that address
        /// </summary>
        public Dictionary<string, Dictionary<int, List<TimedChannelKeyframe>>> Keyframes { get; }

        public PlaybackContinuedEventArgs(float audioPosition, Dictionary<string, Dictionary<int, List<TimedChannelKeyframe>>> keyframes)
        {
            AudioPosition = audioPosition;
            Keyframes = keyframes;
        }
    }
}
