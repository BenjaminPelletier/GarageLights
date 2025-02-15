﻿using GarageLights.Audio;
using GarageLights.Channels;
using GarageLights.InputDevices.Definitions;
using GarageLights.Keyframes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
        private KeyframeManager keyframeManager;
        private ShowNavigator showNavigator;
        private ChannelSelector channelSelector;

        public Multiquence()
        {
            designMode = LicenseManager.UsageMode == LicenseUsageMode.Designtime;
            InitializeComponent();
            tvChannels.NodeLayoutChanged += tvChannels_NodeLayoutChanged;
        }

        public AudioPlayer AudioPlayer
        {
            set
            {
                audioPlayer = value;
                audioControl1.AudioPlayer = audioPlayer;
                keyframeControl1.AudioPlayer = audioPlayer;
                showScroller1.AudioPlayer = audioPlayer;
            }
        }

        public KeyframeManager KeyframeManager
        {
            set
            {
                if (keyframeManager != null)
                {
                    keyframeManager.KeyframesChanged -= keyframeManager_KeyframesChanged;
                }
                keyframeManager = value;
                keyframeControl1.KeyframeManager = value;
                controlPanel1.KeyframeManager = value;
                keyframeManager.KeyframesChanged += keyframeManager_KeyframesChanged;
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
                keyframeControl1.ShowNavigator = value;
                controlPanel1.ShowNavigator = value;
                showNavigator.ActiveKeyframeChanged += showNavigator_ActiveKeyframeChanged;
            }
        }

        public ShowManipulator ShowManipulator
        {
            set
            {
                controlPanel1.ShowManipulator = value;
            }
        }

        public ChannelSelector ChannelSelector
        {
            set
            {
                channelSelector = value;
                keyframeControl1.ChannelSelector = value;
                controlPanel1.ChannelSelector = value;
            }
        }

        public Project Project
        {
            get { return project; }
            set
            {
                project = value;


                if (project == null)
                {
                    project = new Project();
                }

                if (project.ChannelNodes == null)
                {
                    project.ChannelNodes = new List<ChannelNode>();
                }
                List<ChannelNodeTreeNode> topNodes = project.ChannelNodes.Select(n => ChannelNodeTreeNode.FromChannelNode(n)).ToList();
                tvChannels.Nodes.Clear();
                tvChannels.Nodes.AddRange(topNodes.Select(c => (TreeNode)c).ToArray());
                if (topNodes.Count > 0 && channelSelector == null)
                {
                    throw new InvalidOperationException("Attempted to set Multiquence.Project to a non-empty project before ChannelSelector had been populated");
                }
                if (channelSelector != null)
                {
                    channelSelector.SetTopElements(topNodes);
                }

                if (keyframeManager != null)
                {
                    if (project.Show == null)
                    {
                        project.Show = new Show();
                    }
                    if (project.Show.Keyframes == null)
                    {
                        project.Show.Keyframes = new List<ShowKeyframe>();
                    }
                    keyframeManager.Keyframes = project.Show.Keyframes;
                }

                if (audioPlayer != null)
                {
                    if (project.Show.AudioFile != null)
                    {
                        audioPlayer.LoadAudio(project.Show.AudioFile);
                    }
                    else
                    {
                        audioPlayer.UnloadAudio();
                    }
                }
            }
        }

        public IEnumerable<ChannelNode> GetChannels()
        {
            foreach (TreeNode node in tvChannels.Nodes)
            {
                yield return (node as ChannelNodeTreeNode).ChannelNode;
            }
        }

        private void keyframeManager_KeyframesChanged(object sender, EventArgs e)
        {
            keyframeControl1.Invalidate();
        }

        private void showNavigator_ActiveKeyframeChanged(object sender, EventArgs e)
        {
            keyframeControl1.Invalidate();
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

        private void showScroller1_NewViewRequested(object sender, AudioViewChangedEventArgs e)
        {
            audioControl1.UpdateAudioView(e.LeftTime, e.RightTime);
        }
    }
}
