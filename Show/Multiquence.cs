using GarageLights.Audio;
using GarageLights.Keyframes;
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
        private AudioPlayer audioPlayer;

        public Multiquence()
        {
            designMode = LicenseManager.UsageMode == LicenseUsageMode.Designtime;
            InitializeComponent();
            tvChannels.NodeLayoutChanged += tvChannels_NodeLayoutChanged;
            keyframeControl1.RowSource = tvChannels;
            controlPanel1.KeyframeManager = keyframeControl1;
        }

        public AudioPlayer AudioPlayer
        {
            set
            {
                audioPlayer = value;
                audioControl1.AudioPlayer = audioPlayer;
                keyframeControl1.AudioPlayer = audioPlayer;
                showScroller1.AudioPlayer = audioPlayer;
                controlPanel1.AudioPlayer = audioPlayer;
            }
        }

        public IKeyframeManger KeyframeManager { get { return keyframeControl1; } }

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
                        project.Keyframes = new List<ShowKeyframe>();
                    }
                    keyframeControl1.Keyframes = project.Keyframes;

                    if (project.AudioFile != null)
                    {
                        audioPlayer.LoadAudio(project.AudioFile);
                    }
                }
            }
        }

        private void toolPanel1_Play(object sender, EventArgs e)
        {
            audioPlayer.Play();
        }

        private void toolPanel1_Stop(object sender, EventArgs e)
        {
            audioPlayer.Stop();
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
                audioPlayer.LoadAudio(ofdAudioFile.FileName);
            }
        }

        private void audioControl1_AudioViewChanged(object sender, AudioViewChangedEventArgs e)
        {
            keyframeControl1.SetTimeRange(e.LeftTime, e.RightTime);
            showScroller1.SetTimeRange(e.LeftTime, e.RightTime);
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

        private void showScroller1_NewViewRequested(object sender, AudioViewChangedEventArgs e)
        {
            audioControl1.UpdateAudioView(e.LeftTime, e.RightTime);
        }
    }
}
