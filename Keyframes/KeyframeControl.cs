﻿using GarageLights.Audio;
using GarageLights.Channels;
using GarageLights.Dialogs;
using GarageLights.Show;
using GarageLights.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GarageLights.Keyframes
{
    internal class KeyframeControl : UserControl
    {
        const float KeyframeSize = 8f;
        static ChannelKeyframe DefaultKeyframe = new ChannelKeyframe() { Value = 0, Style = KeyframeStyle.Linear };
        const float RowMargin = 2f;

        AudioPlayer audioPlayer;
        KeyframeManager keyframeManager;
        ShowNavigator showNavigator;
        IChannelSelector channelSelector;

        private ThrottledPainter bgPainter;

        float leftTime;
        float rightTime;

        public KeyframeControl()
        {
            DoubleBuffered = true;
            bgPainter = new ThrottledPainter(this, KeyframeControl_Paint);
            Paint += bgPainter.Paint;
            MouseDoubleClick += KeyframeControl_MouseDoubleClick;
        }

        public AudioPlayer AudioPlayer
        {
            set
            {
                audioPlayer = value;
                audioPlayer.AudioPositionChanged += audioPlayer_AudioPositionChanged;
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
                showNavigator.ActiveKeyframeChanged += showNavigator_ActiveKeyframeChanged;
            }
        }

        public void SetTimeRange(float leftTime, float rightTime)
        {
            this.leftTime = leftTime;
            this.rightTime = rightTime;
            Invalidate();
        }

        public IChannelSelector ChannelSelector
        {
            get { return channelSelector; }
            set
            {
                channelSelector = value;
                Invalidate();
            }
        }

        private void audioPlayer_AudioPositionChanged(object sender, AudioPositionChangedEventArgs e)
        {
            bgPainter.RequestPaint(!audioPlayer.Playing);
        }

        private void showNavigator_ActiveKeyframeChanged(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void keyframeManager_KeyframesChanged(object sender, EventArgs e)
        {
            Invalidate();
        }

        private ShowKeyframe ClosestKeyframe(float x)
        {
            float t = leftTime + (rightTime - leftTime) * x / ClientSize.Width;

            ShowKeyframe closestKeyframe = null;
            float dt = float.PositiveInfinity;
            foreach (ShowKeyframe keyframe in keyframeManager.Keyframes)
            {
                if (keyframe.Time < leftTime || keyframe.Time > rightTime) { continue; }
                float kdt = Math.Abs(t - keyframe.Time);
                if (kdt < dt)
                {
                    closestKeyframe = keyframe;
                    dt = kdt;
                }
            }
            return closestKeyframe;
        }

        private void KeyframeControl_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (audioPlayer == null || !audioPlayer.IsAudioLoaded ||
                keyframeManager == null || keyframeManager.Keyframes == null ||
                leftTime == rightTime) { return; }

            ShowKeyframe closestKeyframe = ClosestKeyframe(e.X);
            if (closestKeyframe == null) { return; }

            ChannelNodeTreeNode closestChannelNode = channelSelector.ClosestChannel(e.Y);
            if (closestChannelNode == null) { return; }

            bool newKeyframe = false;
            if (closestKeyframe.Channels == null || !closestKeyframe.Channels.ContainsKey(closestChannelNode.FullName))
            {
                var channelKeyframe = new ChannelKeyframe() { Value = 0 };
                if (closestKeyframe.Channels == null)
                {
                    closestKeyframe.Channels = new Dictionary<string, ChannelKeyframe>();
                }
                closestKeyframe.Channels[closestChannelNode.FullName] = channelKeyframe;
                newKeyframe = true;
            }
            var valueDialog = new ChannelValueDialog();
            valueDialog.ChannelName = closestChannelNode.FullName;
            valueDialog.ChannelKeyframe = closestKeyframe.Channels[closestChannelNode.FullName];
            if (valueDialog.ShowDialog(this) == DialogResult.OK)
            {
                closestKeyframe.Channels[closestChannelNode.FullName] = valueDialog.ChannelKeyframe;
            }
            else if (newKeyframe)
            {
                closestKeyframe.Channels.Remove(closestChannelNode.FullName);
            }

            keyframeManager.NotifyKeyframesChanged();
        }

        private void KeyframeControl_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(BackColor);
            if (audioPlayer == null || !audioPlayer.IsAudioLoaded) { return; }

            if (channelSelector != null && audioPlayer != null && audioPlayer.IsAudioLoaded && rightTime > leftTime)
            {
                DrawKeyframes(e.Graphics);
            }

            float currentTime = audioPlayer.AudioPosition;
            if (leftTime <= currentTime && currentTime <= rightTime && leftTime != rightTime)
            {
                float x = (currentTime - leftTime) / (rightTime - leftTime) * ClientSize.Width;
                e.Graphics.DrawLine(Pens.Red, x, 0, x, ClientSize.Height);
            }
        }

        private void DrawKeyframes(Graphics g)
        {
            float maxTime = audioPlayer.AudioLength;
            var rowFrames = new Dictionary<ChannelNodeTreeNode, List<TimedChannelKeyframe>>();

            // Calculate keyframes per row
            if (keyframeManager.Keyframes != null)
            {
                foreach (ChannelNodeTreeNode node in channelSelector.GetVisibleChannels())
                {
                    var bounds = node.Bounds;

                    // Collect a complete list of keyframes to draw, including implicit keyframes at the beginning
                    // and end of the song
                    string rowName = node.FullName;
                    List<TimedChannelKeyframe> frames = keyframeManager.Keyframes
                        .Where(f => f.Channels != null && f.Channels.ContainsKey(rowName))
                        .Select(f => new TimedChannelKeyframe(f.Time, f.Channels[rowName]))
                        .ToList();
                    if (frames.Count == 0) { continue; }

                    if (frames.Count == 0 || frames[0].Time != 0)
                    {
                        frames.Insert(0, new TimedChannelKeyframe(0, DefaultKeyframe));
                    }
                    if (frames.Count == 1 || frames[frames.Count - 1].Time != maxTime)
                    {
                        frames.Add(new TimedChannelKeyframe(maxTime, frames[frames.Count - 1].Keyframe));
                    }
                    rowFrames[node] = frames;
                }
            }

            // Draw signal representation
            foreach (var kv in rowFrames)
            {
                var bounds = kv.Key.Bounds;
                var frames = kv.Value;
                for (int i = 1; i < frames.Count; i++)
                {
                    TimedChannelKeyframe kf0 = frames[i - 1];
                    TimedChannelKeyframe kf1 = frames[i];
                    if (kf1.Time < leftTime || kf0.Time > rightTime) { continue; }
                    float x0 = ClientSize.Width * (kf0.Time - leftTime) / (rightTime - leftTime);
                    float x1 = ClientSize.Width * (kf1.Time - leftTime) / (rightTime - leftTime);
                    float yb = bounds.Bottom - RowMargin;
                    float y0 = yb - (bounds.Height - 2 * RowMargin) * (kf0.Keyframe.Value / 255.0f);
                    float y1 = yb - (bounds.Height - 2 * RowMargin) * (kf1.Keyframe.Value / 255.0f);
                    if (kf1.Keyframe.Style == KeyframeStyle.Linear)
                    {
                        g.FillPolygon(Brushes.LightSkyBlue, new PointF[]
                        {
                            new PointF(x0, yb),
                            new PointF(x0, y0),
                            new PointF(x1, y1),
                            new PointF(x1, yb),
                        });
                    }
                    else if (kf1.Keyframe.Style == KeyframeStyle.Step)
                    {
                        g.DrawLine(Pens.LightSkyBlue, x1, y0, x1, y1);
                        g.FillRectangle(Brushes.LightSkyBlue, x0, y0, x1 - x0, yb - y0);
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
            }

            // Draw row labels
            foreach (ChannelNodeTreeNode node in channelSelector.GetVisibleChannels())
            {
                var bounds = node.Bounds;

                SizeF labelSize = g.MeasureString(node.Text, Font);
                g.DrawString(node.Text, Font, Brushes.DarkGray, 0, bounds.Top + (bounds.Height - labelSize.Height) / 2);
            }

            if (keyframeManager.Keyframes != null)
            {
                // Draw keyframe times
                foreach (ShowKeyframe f in keyframeManager.Keyframes)
                {
                    if (f.Time < leftTime || f.Time > rightTime) { continue; }
                    float x = ClientSize.Width * (f.Time - leftTime) / (rightTime - leftTime);
                    g.DrawLine(showNavigator.ActiveKeyframe == f ? Pens.Orange : Pens.DarkGreen, x, 0, x, ClientSize.Height);
                }

                // Draw keyframe markers/icons
                foreach (var kv in rowFrames)
                {
                    var bounds = kv.Key.Bounds;
                    var frames = kv.Value;

                    for (int i = 0; i < frames.Count; i++)
                    {
                        TimedChannelKeyframe kf = frames[i];
                        float x = ClientSize.Width * (kf.Time - leftTime) / (rightTime - leftTime);
                        float y = (bounds.Top + bounds.Bottom) / 2;
                        var path = new GraphicsPath();
                        path.AddLine(x, y - KeyframeSize / 2, x + KeyframeSize / 2, y);
                        path.AddLine(x + KeyframeSize / 2, y, x, y + KeyframeSize / 2);
                        path.AddLine(x, y + KeyframeSize / 2, x - KeyframeSize / 2, y);
                        path.AddLine(x - KeyframeSize / 2, y, x, y - KeyframeSize / 2);
                        if (showNavigator.ActiveKeyframe != null && kf.Time == showNavigator.ActiveKeyframe.Time)
                        {
                            g.FillPath(Brushes.LightGoldenrodYellow, path);
                            g.DrawPath(Pens.DarkOrange, path);
                        }
                        else
                        {
                            g.FillPath(Brushes.LightGreen, path);
                            g.DrawPath(Pens.DarkGreen, path);
                        }
                    }
                }
            }
        }
    }
}
