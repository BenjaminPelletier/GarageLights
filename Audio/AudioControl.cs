using System;
using System.Drawing;
using System.Windows.Forms;
using NAudio.Wave;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;
using System.ComponentModel;

namespace GarageLights.Audio
{
    internal class AudioControl : UserControl
    {
        const float MIN_WINDOW_SECONDS = 0.5f;
        const float NAVIGATION_QUANTA_SECONDS = 0.001f;

        private AudioPlayer audioPlayer;
        private ViewableWaveform viewableWaveform;
        private Bitmap waveformBitmap;

        private float leftTime;
        private float rightTime;

        private ThrottledPainter bgPainter;
        private bool isDragging;
        private Point dragStartPoint;
        private float dragStartLeftTime;

        public event EventHandler<AudioViewChangedEventArgs> AudioViewChanged;
        public event EventHandler FileLoadRequested;

        public AudioControl()
        {
            bool designMode = LicenseManager.UsageMode == LicenseUsageMode.Designtime;
            DoubleBuffered = true;
            bgPainter = new ThrottledPainter(this, AudioControl_Paint);
            if (!designMode)
            {
                MouseWheel += AudioControl_MouseWheel;
                MouseDown += AudioControl_MouseDown;
                MouseUp += AudioControl_MouseUp;
                MouseMove += AudioControl_MouseMove;
                Paint += bgPainter.Paint;
                MouseDoubleClick += AudioControl_MouseDoubleClick;
                Resize += AudioControl_Resize;
            }
        }

        public AudioPlayer AudioPlayer
        {
            set
            {
                audioPlayer = value;
                audioPlayer.LoadingAudio += audioPlayer_LoadingAudio;
                audioPlayer.AudioLoaded += audioPlayer_AudioLoaded;
                audioPlayer.AudioPositionChanged += audioPlayer_AudioPositionChanged;
            }
        }

        public float LeftTime
        {
            get { return leftTime; }
            set
            {
                UpdateAudioView(value, rightTime);
            }
        }

        public float RightTime
        {
            get { return rightTime; }
            set
            {
                UpdateAudioView(leftTime, value);
            }
        }

        private bool AudioLoaded { get { return audioPlayer != null && audioPlayer.IsAudioLoaded; } }

        private float Quantize(float value)
        {
            return (float)(Math.Round(value / NAVIGATION_QUANTA_SECONDS, 0) * NAVIGATION_QUANTA_SECONDS);
        }

        public void UpdateAudioView(float newLeftTime, float newRightTime)
        {
            if (!AudioLoaded) { return; }

            newLeftTime = Quantize(newLeftTime);
            newRightTime = Quantize(newRightTime);

            float timeWidth = newRightTime - newLeftTime;
            if (timeWidth < MIN_WINDOW_SECONDS)
            {
                float timeCenter = 0.5f * (newLeftTime + newRightTime);
                newLeftTime = Quantize(timeCenter - 0.5f * MIN_WINDOW_SECONDS);
                newRightTime = Quantize(timeCenter + 0.5f * MIN_WINDOW_SECONDS);
            }

            float audioLength = audioPlayer.AudioLength;
            if (timeWidth > audioLength)
            {
                newLeftTime = 0;
                newRightTime = Quantize(audioLength);
            }
            else if (newLeftTime < 0)
            {
                float deltaTime = -newLeftTime;
                newLeftTime = 0;
                newRightTime = Quantize(newRightTime + deltaTime);
            }
            else if (newRightTime > audioLength)
            {
                float deltaTime = audioLength - newRightTime;
                newLeftTime = Quantize(newLeftTime + deltaTime);
                newRightTime = Quantize(audioLength);
            }

            using (Graphics g = Graphics.FromImage(waveformBitmap))
            {
                g.Clear(BackColor);
                viewableWaveform.Draw(g, 0, waveformBitmap.Width, waveformBitmap.Height / 2, waveformBitmap.Height / 2, newLeftTime, newRightTime);
            }

            if (Math.Abs(newLeftTime - leftTime) >= 0.5f * NAVIGATION_QUANTA_SECONDS ||
                Math.Abs(newRightTime - rightTime) > 0.5f * NAVIGATION_QUANTA_SECONDS)
            {
                leftTime = newLeftTime;
                rightTime = newRightTime;
                Invoke((Action)(() =>
                {
                    AudioViewChanged?.Invoke(this, new AudioViewChangedEventArgs(leftTime, rightTime));
                    Invalidate();
                }));
            }
        }

        private float TimeAt(float x)
        {
            return leftTime + (rightTime - leftTime) * x / Width;
        }

        private void audioPlayer_LoadingAudio(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void audioPlayer_AudioLoaded(object sender, AudioLoadedEventArgs e)
        {
            viewableWaveform = new ViewableWaveform(e.AudioFile);
            UpdateAudioView(0, audioPlayer.AudioLength);
            Invalidate();
        }

        private void audioPlayer_AudioPositionChanged(object sender, AudioPositionChangedEventArgs e)
        {
            bgPainter.RequestPaint(!audioPlayer.Playing);
        }

        private void AudioControl_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;

            if (audioPlayer == null)
            {
                g.Clear(BackColor);
                return;
            }

            if (audioPlayer.IsLoadingAudio)
            {
                g.Clear(Color.LightYellow);
                return;
            }

            // Draw waveform
            if (waveformBitmap != null)
            {
                g.DrawImage(waveformBitmap, 0, 0);
            }

            // Draw AudioPosition line
            float audioPosition = audioPlayer.AudioPosition;
            if (audioPosition >= leftTime && audioPosition <= rightTime)
            {
                float positionX = leftTime != rightTime ? (audioPosition - leftTime) / (rightTime - leftTime) * ClientSize.Width : 0;
                g.DrawLine(Pens.Red, (int)positionX, 0, (int)positionX, ClientSize.Height);
            }
        }

        private void AudioControl_MouseWheel(object sender, MouseEventArgs e)
        {
            if (!AudioLoaded) return;

            const float ZOOM_PER_WHEEL = 0.7f;
            float zoomAmount = e.Delta > 0 ? ZOOM_PER_WHEEL : (1.0f / ZOOM_PER_WHEEL);
            float mouseTime = TimeAt(e.X);

            UpdateAudioView(
                mouseTime - (mouseTime - leftTime) * zoomAmount,
                mouseTime + (rightTime - mouseTime) * zoomAmount
            );
        }

        private void AudioControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                isDragging = true;
                dragStartPoint = e.Location;
                dragStartLeftTime = leftTime;
            }
            else if (e.Button == MouseButtons.Left && audioPlayer != null && audioPlayer.IsAudioLoaded)
            {
                audioPlayer.Stop();
                audioPlayer.AudioPosition = TimeAt(e.X);
            }
        }

        private void AudioControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging && audioPlayer != null && audioPlayer.IsAudioLoaded)
            {
                float timeWidth = rightTime - leftTime;
                float dragStartRightTime = dragStartLeftTime + timeWidth;
                var deltaX = e.Location.X - dragStartPoint.X;
                var deltaTime = timeWidth * deltaX / Width;
                if (dragStartLeftTime - deltaTime < 0)
                {
                    deltaTime = dragStartLeftTime;
                }
                float audioLength = audioPlayer.AudioLength;
                if (dragStartRightTime - deltaTime > audioLength)
                {
                    deltaTime = dragStartRightTime - audioLength;
                }
                UpdateAudioView(dragStartLeftTime - deltaTime, dragStartRightTime - deltaTime);
            }
        }

        private void AudioControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                isDragging = false;
            }
            else if (audioPlayer == null || !audioPlayer.IsAudioLoaded)
            {
                FileLoadRequested?.Invoke(this, EventArgs.Empty);
            }
        }

        private void AudioControl_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && audioPlayer != null || audioPlayer.IsAudioLoaded)
            {
                audioPlayer.Play();
            }
        }

        private void AudioControl_Resize(object sender, EventArgs e)
        {
            if (waveformBitmap != null)
            {
                waveformBitmap.Dispose();
            }
            waveformBitmap = new Bitmap(ClientSize.Width, ClientSize.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            UpdateAudioView(leftTime, rightTime);
            Invalidate();
        }
    }

    public class AudioViewChangedEventArgs : EventArgs
    {
        public float LeftTime { get; }
        public float RightTime { get; }

        public AudioViewChangedEventArgs(float leftTime, float rightTime)
        {
            LeftTime = leftTime;
            RightTime = rightTime;
        }
    }
}
