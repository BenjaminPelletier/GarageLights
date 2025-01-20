using System;
using System.Drawing;
using System.Windows.Forms;
using NAudio.Wave;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;
using System.ComponentModel;

namespace GarageLights
{
    public class AudioControl : UserControl
    {
        const float MIN_WINDOW_SECONDS = 0.5f;
        const float NAVIGATION_QUANTA_SECONDS = 0.001f;

        bool designMode;

        private WaveOutEvent waveOut;
        private AudioFileReader audioFile;

        private float audioLength;
        private ViewableWaveform viewableWaveform;
        private Bitmap waveformBitmap;

        private float leftTime;
        private float rightTime;
        private float audioPosition;
        private float timePlaybackStarted;

        private bool isLoadingAudio;
        private bool isDragging;
        private Point dragStartPoint;
        private float dragStartLeftTime;
        private Task<bool> playingTask;

        public event EventHandler AudioLoaded;
        public event EventHandler<AudioViewChangedEventArgs> AudioViewChanged;
        public event EventHandler<AudioPositionChangedEventArgs> AudioPositionChanged;
        public event EventHandler PlaybackStarted;
        public event EventHandler PlaybackStopped;
        public event EventHandler FileLoadRequested;

        public AudioControl()
        {
            designMode = LicenseManager.UsageMode == LicenseUsageMode.Designtime;
            DoubleBuffered = true;
            if (!designMode)
            {
                MouseWheel += OnMouseWheel;
                MouseDown += OnMouseDown;
                MouseUp += OnMouseUp;
                MouseMove += OnMouseMove;
                Paint += OnPaint;
                MouseDoubleClick += OnMouseDoubleClick;
                Resize += OnResize;
            }
        }

        public float AudioLength => audioLength;

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

        public bool Playing
        {
            get { return playingTask != null && !playingTask.IsCompleted; }
        }

        private float Quantize(float value)
        {
            return (float)(Math.Round(value / NAVIGATION_QUANTA_SECONDS, 0) * NAVIGATION_QUANTA_SECONDS);
        }

        private void UpdateAudioView(float newLeftTime, float newRightTime)
        {
            if (audioFile == null || designMode)
            {
                return;
            }

            newLeftTime = Quantize(newLeftTime);
            newRightTime = Quantize(newRightTime);

            float timeWidth = newRightTime - newLeftTime;
            if (timeWidth < MIN_WINDOW_SECONDS)
            {
                float timeCenter = 0.5f * (newLeftTime + newRightTime);
                newLeftTime = Quantize(timeCenter - 0.5f * MIN_WINDOW_SECONDS);
                newRightTime = Quantize(timeCenter + 0.5f * MIN_WINDOW_SECONDS);
            }

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
                BeginInvoke((Action)(() =>
                {
                    AudioViewChanged?.Invoke(this, new AudioViewChangedEventArgs(leftTime, rightTime));
                    Invalidate();
                }));
            }
        }

        public float AudioPosition
        {
            get { return audioPosition; }
            set
            {
                if (audioFile == null || designMode) return;

                bool play = false;
                if (Playing)
                {
                    play = true;
                    Stop();
                }
                UpdateAudioPosition(value);
                if (play)
                {
                    Play();
                }
            }
        }

        private void UpdateAudioPosition(float newAudioPosition)
        {
            audioPosition = Math.Max(0, Math.Min(newAudioPosition, audioLength));
            audioFile.Position = (long)(audioPosition * audioFile.WaveFormat.AverageBytesPerSecond);
            BeginInvoke((Action)(() =>
            {
                AudioPositionChanged?.Invoke(this, new AudioPositionChangedEventArgs(audioPosition));
                Invalidate();
            }));
        }

        public void LoadAudio(string filePath)
        {
            if (isLoadingAudio)
            {
                throw new InvalidOperationException("Tried to LoadAudio while already loading audio");
            }
            isLoadingAudio = true;
            new Thread(() =>
            {
                try
                {
                    LoadAudioWorker(filePath);
                }
                catch (Exception ex)
                {
                    Debug.Print("LoadAudio error: " + ex);
                }
            })
            { IsBackground = true }.Start();
            Invalidate();
        }

        private void LoadAudioWorker(string filePath)
        {
            var newAudioFile = new AudioFileReader(filePath);
            var newWaveOut = new WaveOutEvent();
            newWaveOut.Init(newAudioFile);
            var newViewableWaveform = new ViewableWaveform(newAudioFile);

            waveOut?.Dispose();
            audioFile?.Dispose();

            audioFile = newAudioFile;
            waveOut = newWaveOut;
            audioLength = (float)audioFile.TotalTime.TotalSeconds;
            viewableWaveform = newViewableWaveform;

            isLoadingAudio = false;
            AudioLoaded?.Invoke(this, EventArgs.Empty);
            UpdateAudioView(0, audioLength);
            UpdateAudioPosition(0);
        }

        public void Play()
        {
            if (waveOut != null && Playing)
            {
                return;
            }

            var tcs = new TaskCompletionSource<bool>();
            playingTask = tcs.Task;

            timePlaybackStarted = (float)audioFile.CurrentTime.TotalSeconds;
            waveOut.Play();

            new Thread(() =>
            {
                try
                {
                    IAsyncResult uiResult = null;
                    while (waveOut.PlaybackState == PlaybackState.Playing)
                    {
                        var newPosition = timePlaybackStarted + (float)waveOut.GetPosition() / audioFile.WaveFormat.AverageBytesPerSecond;
                        Debug.Print("Playback thread: new position " + newPosition);
                        audioPosition = newPosition;
                        if (uiResult == null || uiResult.IsCompleted)
                        {
                            uiResult = BeginInvoke((Action)(() =>
                            {
                                Debug.Print("Playback -> UI thread: AudioPositionChanged");
                                AudioPositionChanged?.Invoke(this, new AudioPositionChangedEventArgs(audioPosition));
                                Invalidate();
                            }));
                        }
                    }
                    tcs.SetResult(true);
                }
                catch (Exception ex)
                {
                    Debug.Print("Playback thread: exception " + ex);
                    tcs.SetException(ex);
                }
                finally
                {
                    Debug.Print("Playback thread: finally");
                    BeginInvoke((Action)(() => PlaybackStopped?.Invoke(this, EventArgs.Empty)));
                }
            })
            { IsBackground = true }.Start();
            PlaybackStarted?.Invoke(this, EventArgs.Empty);
        }

        public void Stop()
        {
            if (Playing)
            {
                waveOut.Pause();
                audioPosition = timePlaybackStarted + (float)waveOut.GetPosition() / audioFile.WaveFormat.AverageBytesPerSecond;
                waveOut.Stop();
                playingTask.Wait();
                if (playingTask.IsFaulted)
                {
                    throw new PlaybackException("An exception occurred during audio playback", playingTask.Exception);
                }
                PlaybackStopped?.Invoke(this, EventArgs.Empty);
            }
        }

        private float TimeAt(float x)
        {
            return leftTime + (rightTime - leftTime) * x / Width;
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;

            if (isLoadingAudio)
            {
                g.Clear(Color.LightYellow);
            }

            // Draw waveform
            if (waveformBitmap != null)
            {
                g.DrawImage(waveformBitmap, 0, 0);
            }

            // Draw AudioPosition line
            if (audioPosition >= leftTime && audioPosition <= rightTime)
            {
                float positionX = leftTime != rightTime ? (audioPosition - leftTime) / (rightTime - leftTime) * ClientSize.Width : 0;
                g.DrawLine(Pens.Red, (int)positionX, 0, (int)positionX, ClientSize.Height);
            }
        }

        private void OnMouseWheel(object sender, MouseEventArgs e)
        {
            if (audioFile == null) return;

            const float ZOOM_PER_WHEEL = 0.7f;
            float zoomAmount = e.Delta > 0 ? ZOOM_PER_WHEEL : (1.0f / ZOOM_PER_WHEEL);
            float mouseTime = TimeAt(e.X);

            UpdateAudioView(
                mouseTime - (mouseTime - leftTime) * zoomAmount,
                mouseTime + (rightTime - mouseTime) * zoomAmount
            );
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                isDragging = true;
                dragStartPoint = e.Location;
                dragStartLeftTime = leftTime;
            }
            else if (e.Button == MouseButtons.Left)
            {
                AudioPosition = TimeAt(e.X);
            }
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                isDragging = false;
            }
            else if (audioFile == null)
            {
                FileLoadRequested?.Invoke(this, EventArgs.Empty);
            }
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging && audioFile != null)
            {
                float timeWidth = rightTime - leftTime;
                float dragStartRightTime = dragStartLeftTime + timeWidth;
                var deltaX = e.Location.X - dragStartPoint.X;
                var deltaTime = timeWidth * deltaX / Width;
                if (dragStartLeftTime - deltaTime < 0)
                {
                    deltaTime = -dragStartLeftTime;
                }
                if (dragStartRightTime + deltaTime > audioLength)
                {
                    deltaTime = audioLength - dragStartRightTime;
                }
                UpdateAudioView(dragStartLeftTime - deltaTime, dragStartRightTime - deltaTime);
            }
        }

        private void OnMouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Play();
            }
        }

        private void OnResize(object sender, EventArgs e)
        {
            if (waveformBitmap != null)
            {
                waveformBitmap.Dispose();
            }
            waveformBitmap = new Bitmap(ClientSize.Width, ClientSize.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            UpdateAudioView(leftTime, rightTime);
            Invalidate();
        }

        protected override void Dispose(bool disposing)
        {
            waveOut?.Dispose();
            audioFile?.Dispose();
            base.Dispose(disposing);
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

        public class AudioPositionChangedEventArgs : EventArgs
        {
            public float AudioPosition { get; }

            public AudioPositionChangedEventArgs(float audioPosition)
            {
                AudioPosition = audioPosition;
            }
        }

        public class PlaybackException : Exception
        {
            public PlaybackException(string message, Exception innerException) : base(message, innerException) { }
        }
    }
}
