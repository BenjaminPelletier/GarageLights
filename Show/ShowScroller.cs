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
        private AudioPlayer audioPlayer;

        float leftTime;
        float rightTime;

        private ThrottledUiCall refresh;
        float tDragOrigin;
        bool isDragging;

        public event EventHandler<AudioViewChangedEventArgs> NewViewRequested;

        public ShowScroller()
        {
            refresh = new ThrottledUiCall(this, Refresh);
            DoubleBuffered = true;
            Paint += ShowScroller_Paint;
            MouseDown += ShowScroller_MouseDown;
            MouseMove += ShowScroller_MouseMove;
            MouseUp += ShowScroller_MouseUp;
        }

        public AudioPlayer AudioPlayer
        {
            set
            {
                audioPlayer = value;
                audioPlayer.AudioPositionChanged += audioPlayer_AudioPositionChanged;
            }
        }

        public void SetTimeRange(float leftTime, float rightTime)
        {
            this.leftTime = leftTime;
            this.rightTime = rightTime;
            Invalidate();
        }

        private void audioPlayer_AudioPositionChanged(object sender, AudioPositionChangedEventArgs e)
        {
            refresh.Trigger();
        }

        private void ShowScroller_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(BackColor);
            if (audioPlayer == null || !audioPlayer.IsAudioLoaded) { return; }

            float maxTime = audioPlayer.AudioLength;
            float x0 = ClientSize.Width * leftTime / maxTime;
            float x1 = ClientSize.Width * rightTime / maxTime;
            e.Graphics.FillRectangle(Brushes.DarkGray, 0, 0, x0 - 1, ClientSize.Height);
            e.Graphics.FillRectangle(Brushes.DarkGray, x1 + 1, 0, ClientSize.Width - x1 - 1, ClientSize.Height);

            float x = ClientSize.Width * audioPlayer.AudioPosition / maxTime;
            e.Graphics.DrawLine(Pens.Red, x, 0, x, ClientSize.Height);
        }

        private void ShowScroller_MouseDown(object sender, MouseEventArgs e)
        {
            if (audioPlayer == null || !audioPlayer.IsAudioLoaded) { return; }

            float t = e.X * audioPlayer.AudioLength / ClientSize.Width;

            if (e.Button == MouseButtons.Left)
            {
                tDragOrigin = t;
                isDragging = leftTime <= tDragOrigin && tDragOrigin <= rightTime;
            }
        }

        private void ShowScroller_MouseMove(object sender, MouseEventArgs e)
        {
            if (audioPlayer == null || !audioPlayer.IsAudioLoaded) { return; }

            float t = e.X * audioPlayer.AudioLength / ClientSize.Width;

            if (e.Button == MouseButtons.Left && isDragging)
            {
                tDragOrigin += ChangeAudioView(t - tDragOrigin);
            }
        }

        private void ShowScroller_MouseUp(object sender, MouseEventArgs e)
        {
            if (audioPlayer == null || !audioPlayer.IsAudioLoaded) { return; }

            float t = e.X * audioPlayer.AudioLength / ClientSize.Width;
            if (e.Button == MouseButtons.Left && !isDragging)
            {
                ChangeAudioView(t - leftTime);
            }
        }
        
        private float ChangeAudioView(float dt)
        {
            float maxTime = audioPlayer.AudioLength;

            if (leftTime + dt < 0)
            {
                dt = -leftTime;
            }
            if (rightTime + dt > maxTime)
            {
                dt = maxTime - rightTime;
            }
            NewViewRequested?.Invoke(this, new AudioViewChangedEventArgs(leftTime + dt, rightTime + dt));
            return dt;
        }
    }
}
