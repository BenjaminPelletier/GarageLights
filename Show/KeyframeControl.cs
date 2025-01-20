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

        ChannelTreeView rowSource;
        Dictionary<string, List<Keyframe>> keyframes;
        float maxTime;
        float currentTime;
        float leftTime;
        float rightTime;

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
                Invalidate();
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

        public Dictionary<string, List<Keyframe>> Keyframes
        {
            set
            {
                keyframes = value;
            }
        }

        private void KeyframeControl_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(BackColor);
            if (maxTime == 0) { return; }

            if (rowSource != null)
            {
                foreach (NodeBounds nodeBounds in rowSource.GetVisibleNodes())
                {
                    var bounds = nodeBounds.Node.Bounds;

                    SizeF labelSize = e.Graphics.MeasureString(nodeBounds.Node.Text, Font);
                    e.Graphics.DrawString(nodeBounds.Node.Text, Font, Brushes.DarkGray, 0, bounds.Top + (bounds.Height - labelSize.Height) / 2);

                    string rowName = nodeBounds.Node.FullName;
                    if (keyframes != null && keyframes.ContainsKey(rowName))
                    {
                        List<Keyframe> definedFrames = keyframes[rowName];
                        var frames = new List<Keyframe>();
                        if (definedFrames.Count < 1 || definedFrames[0].Time != 0)
                        {
                            frames.Add(new Keyframe() { Time = 0, Value = 0 });
                        }
                        frames.AddRange(definedFrames);
                        if (definedFrames.Count < 1 || definedFrames[definedFrames.Count - 1].Time != maxTime)
                        {
                            int value = definedFrames.Count > 0 ? definedFrames[definedFrames.Count - 1].Value : 0;
                            frames.Add(new Keyframe() { Time = maxTime, Value = value });
                        }
                        for (int i = 1; i < frames.Count; i++)
                        {
                            Keyframe kf0 = frames[i - 1];
                            Keyframe kf1 = frames[i];
                            float x0 = ClientSize.Width * (kf0.Time - leftTime) / (rightTime - leftTime);
                            float y0 = bounds.Bottom + (bounds.Top - bounds.Bottom) * (kf0.Value / 255.0f);
                            float x1 = ClientSize.Width * (kf1.Time - leftTime) / (rightTime - leftTime);
                            float y1 = bounds.Bottom + (bounds.Top - bounds.Bottom) * (kf1.Value / 255.0f);
                            if (kf1.Style == KeyframeStyle.Linear)
                            {
                                e.Graphics.DrawLine(Pens.Green, x0, y0, x1, y1);
                            }
                            else if (kf1.Style == KeyframeStyle.Step)
                            {
                                e.Graphics.DrawLine(Pens.Green, x0, y0, x1, y0);
                                e.Graphics.DrawLine(Pens.Green, x1, y0, x1, y1);
                            }
                        }
                        for (int i = 0; i < frames.Count; i++)
                        {
                            Keyframe kf = frames[i];
                            float x = ClientSize.Width * (kf.Time - leftTime) / (rightTime - leftTime);
                            float y = bounds.Bottom + (bounds.Top - bounds.Bottom) * (kf.Value / 255.0f);
                            var path = new GraphicsPath();
                            path.AddLine(x, y - KeyframeSize / 2, x + KeyframeSize / 2, y);
                            path.AddLine(x + KeyframeSize / 2, y, x, y + KeyframeSize / 2);
                            path.AddLine(x, y + KeyframeSize / 2, x - KeyframeSize / 2, y);
                            path.AddLine(x - KeyframeSize / 2, y, x, y - KeyframeSize / 2);
                            e.Graphics.FillPath(Brushes.LightGreen, path);
                            e.Graphics.DrawPath(Pens.DarkGreen, path);
                        }
                    }
                }
            }

            if (leftTime <= currentTime && currentTime <= rightTime)
            {
                float x = (currentTime - leftTime) / (rightTime - leftTime) * ClientSize.Width;
                e.Graphics.DrawLine(Pens.Red, x, 0, x, ClientSize.Height);
            }
        }
    }
}
