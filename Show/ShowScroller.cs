using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using GarageLights.Audio;

namespace GarageLights.Show
{
    internal class ShowScroller : UserControl
    {
        float maxTime;
        float currentTime;
        float leftTime;
        float rightTime;

        float tDragOrigin;
        bool isDragging;

        public event EventHandler<AudioViewChangedEventArgs> AudioViewChange;

        public ShowScroller()
        {
            DoubleBuffered = true;
            Paint += ShowScroller_Paint;
            MouseDown += ShowScroller_MouseDown;
            MouseMove += ShowScroller_MouseMove;
            MouseUp += ShowScroller_MouseUp;
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

        private void ShowScroller_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(BackColor);
            if (maxTime == 0) { return; }

            float x0 = ClientSize.Width * leftTime / maxTime;
            float x1 = ClientSize.Width * rightTime / maxTime;
            e.Graphics.FillRectangle(Brushes.DarkGray, 0, 0, x0 - 1, ClientSize.Height);
            e.Graphics.FillRectangle(Brushes.DarkGray, x1 + 1, 0, ClientSize.Width - x1 - 1, ClientSize.Height);

            float x = ClientSize.Width * currentTime / maxTime;
            e.Graphics.DrawLine(Pens.Red, x, 0, x, ClientSize.Height);
        }

        private void ShowScroller_MouseDown(object sender, MouseEventArgs e)
        {
            float t = e.X * maxTime / ClientSize.Width;

            if (e.Button == MouseButtons.Left)
            {
                tDragOrigin = t;
                isDragging = leftTime <= tDragOrigin && tDragOrigin <= rightTime;
            }
        }

        private void ShowScroller_MouseMove(object sender, MouseEventArgs e)
        {
            float t = e.X * maxTime / ClientSize.Width;

            if (e.Button == MouseButtons.Left && isDragging)
            {
                tDragOrigin += ChangeAudioView(t - tDragOrigin);
            }
        }

        private void ShowScroller_MouseUp(object sender, MouseEventArgs e)
        {
            float t = e.X * maxTime / ClientSize.Width;
            if (e.Button == MouseButtons.Left && !isDragging)
            {
                ChangeAudioView(t - leftTime);
            }
        }
        
        private float ChangeAudioView(float dt)
        {
            if (leftTime + dt < 0)
            {
                dt = -leftTime;
            }
            if (rightTime + dt > maxTime)
            {
                dt = maxTime - rightTime;
            }
            AudioViewChange?.Invoke(this, new AudioViewChangedEventArgs(leftTime + dt, rightTime + dt));
            return dt;
        }
    }
}
