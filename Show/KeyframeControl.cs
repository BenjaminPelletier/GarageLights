using GarageLights.Lights;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GarageLights.Show
{
    internal class KeyframeControl : UserControl
    {
        const float KeyframeSize = 8f;
        static ChannelKeyframe DefaultKeyframe = new ChannelKeyframe() { Value = 0, Style = KeyframeStyle.Linear };
        const float RowMargin = 2f;

        ChannelTreeView rowSource;
        List<Keyframe> keyframes;
        Dictionary<string, Dictionary<int, List<TimedChannelKeyframe>>> keyframesByControllerAndAddress;
        Keyframe activeKeyframe;
        float maxTime;
        float currentTime;
        float leftTime;
        float rightTime;

        public event EventHandler ActiveKeyframeChanged;

        public KeyframeControl() : base()
        {
            DoubleBuffered = true;
            Paint += KeyframeControl_Paint;
        }

        public float MaxTime
        {
            get { return maxTime; }
            set
            {
                maxTime = value;
            }
        }

        public void SetTimeRange(float leftTime, float rightTime)
        {
            this.leftTime = leftTime;
            this.rightTime = rightTime;
            Invalidate();
        }

        public float CurrentTime
        {
            get { return currentTime; }
            set
            {
                currentTime = value;
                Refresh();
            }
        }

        public ChannelTreeView RowSource
        {
            get { return rowSource; }
            set
            {
                rowSource = value;
                Invalidate();
            }
        }

        public List<Keyframe> Keyframes
        {
            get { return keyframes; }
            set
            {
                keyframesByControllerAndAddress = null;
                keyframes = value;
            }
        }

        public Keyframe ActiveKeyframe
        {
            get { return activeKeyframe; }
            set
            {
                if (value != activeKeyframe)
                {
                    activeKeyframe = value;
                    // TODO: Seek to new keyframe unless audio is playing
                    ActiveKeyframeChanged?.Invoke(this, EventArgs.Empty);
                    Invalidate();
                }
            }
        }

        #region Keyframe organization

        public Dictionary<string, Dictionary<int, List<TimedChannelKeyframe>>> KeyframesByControllerAndAddress
        {
            get
            {
                if (keyframesByControllerAndAddress == null && keyframes != null && rowSource != null)
                {
                    var topNodes = new List<ChannelNode>();
                    foreach (TreeNode node in rowSource.Nodes)
                    {
                        topNodes.Add((node as ChannelNodeTreeNode).ChannelNode);
                    }
                    keyframesByControllerAndAddress = OrganizeKeyframesByControllerAndAddress(keyframes, topNodes);
                }
                return keyframesByControllerAndAddress;
            }
        }

        private static Dictionary<string, Dictionary<int, List<TimedChannelKeyframe>>> OrganizeKeyframesByControllerAndAddress(List<Keyframe> keyframes, IEnumerable<ChannelNode> channelNodes)
        {
            Dictionary<string, Channel> channelsByFullName = MapChannelNodes(channelNodes);

            var addressKeyframesByController = new Dictionary<string, Dictionary<int, List<TimedChannelKeyframe>>>();
            foreach (Keyframe f in keyframes)
            {
                if (f.Channels == null) { continue; }
                foreach (var fullChannelNameAndKeyframe in f.Channels)
                {
                    string fullName = fullChannelNameAndKeyframe.Key;
                    var channelKeyframe = fullChannelNameAndKeyframe.Value;

                    Channel channel = channelsByFullName[fullName];

                    // Select output by controller
                    Dictionary<int, List<TimedChannelKeyframe>> keyframesByAddress;
                    if (!addressKeyframesByController.TryGetValue(channel.Controller, out keyframesByAddress))
                    {
                        keyframesByAddress = new Dictionary<int, List<TimedChannelKeyframe>>();
                        addressKeyframesByController[channel.Controller] = keyframesByAddress;
                    }

                    // Select output by address
                    List<TimedChannelKeyframe> channelKeyframes;
                    if (!keyframesByAddress.TryGetValue(channel.Address, out channelKeyframes))
                    {
                        channelKeyframes = new List<TimedChannelKeyframe>();
                        keyframesByAddress[channel.Address] = channelKeyframes;
                    }

                    // Add a new channel keyframe
                    channelKeyframes.Add(new TimedChannelKeyframe(f.Time, channelKeyframe));
                }
            }
            return addressKeyframesByController;
        }

        private static Dictionary<string, Channel> MapChannelNodes(IEnumerable<ChannelNode> channelNodes)
        {
            var channelsByFullName = new Dictionary<string, Channel>();
            foreach (ChannelNode node in channelNodes)
            {
                if (node.Group != null)
                {
                    foreach (var channelByName in MapChannelNodes(node.Group.Nodes))
                    {
                        Channel mappedChannel = channelByName.Value
                            .OffsetAddress(node.Group.Address)
                            .WithParentController(node.Group.Controller);
                        channelsByFullName[node.Name + "." + channelByName.Key] = mappedChannel;
                    }
                }
                else if (node.Channel != null)
                {
                    channelsByFullName[node.Name] = node.Channel;
                }
                else
                {
                    throw new NotImplementedException("ChannelNode '" + node.Name + "' was not a Group node nor Channel node");
                }
            }
            return channelsByFullName;
        }

        #endregion

        private void KeyframeControl_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(BackColor);
            if (maxTime == 0) { return; }

            if (rowSource != null)
            {
                DrawKeyframes(e.Graphics);
            }

            if (leftTime <= currentTime && currentTime <= rightTime && leftTime != rightTime)
            {
                float x = (currentTime - leftTime) / (rightTime - leftTime) * ClientSize.Width;
                e.Graphics.DrawLine(Pens.Red, x, 0, x, ClientSize.Height);
            }

            if (activeKeyframe != null)
            {

            }
        }

        private void DrawKeyframes(Graphics g)
        {
            var rowFrames = new Dictionary<ChannelNodeTreeNode, List<TimedChannelKeyframe>>();

            // Calculate keyframes per row
            if (keyframes != null)
            {
                foreach (NodeBounds nodeBounds in rowSource.GetVisibleNodes())
                {
                    var bounds = nodeBounds.Node.Bounds;

                    // Collect a complete list of keyframes to draw, including implicit keyframes at the beginning
                    // and end of the song
                    string rowName = nodeBounds.Node.FullName;
                    List<TimedChannelKeyframe> frames = keyframes
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
                    rowFrames[nodeBounds.Node] = frames;
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
            foreach (NodeBounds nodeBounds in rowSource.GetVisibleNodes())
            {
                var bounds = nodeBounds.Node.Bounds;

                SizeF labelSize = g.MeasureString(nodeBounds.Node.Text, Font);
                g.DrawString(nodeBounds.Node.Text, Font, Brushes.DarkGray, 0, bounds.Top + (bounds.Height - labelSize.Height) / 2);
            }

            if (keyframes != null)
            {
                // Draw keyframe times
                foreach (Keyframe f in keyframes)
                {
                    if (f.Time < leftTime || f.Time > rightTime) { continue; }
                    float x = ClientSize.Width * (f.Time - leftTime) / (rightTime - leftTime);
                    g.DrawLine(activeKeyframe == f ? Pens.Orange : Pens.DarkGreen, x, 0, x, ClientSize.Height);
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
                        if (activeKeyframe != null && kf.Time == activeKeyframe.Time)
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
