using System;
using System.Drawing;
using System.Windows.Forms;
using NAudio.Wave;
using System.Threading;
using System.Diagnostics;

namespace GarageLights
{
    public class AudioControl : UserControl
    {
        const float MIN_WINDOW_SECONDS = 0.5f;
        const float NAVIGATION_QUANTA_SECONDS = 0.001f;

        private IWavePlayer waveOut;
        private AudioFileReader audioFile;

        private float[] waveformData;
        private float audioLength;
        private float samplesPerPixel;
        private ViewableWaveform viewableWaveform;

        private float leftTime;
        private float rightTime;
        private float audioPosition;

        private bool isLoadingAudio;
        private bool isDragging;
        private Point dragStartPoint;
        private float dragStartLeftTime;
        private bool isPlaying;

        public event EventHandler<AudioViewChangedEventArgs> AudioViewChanged;
        public event EventHandler<AudioPositionChangedEventArgs> AudioPositionChanged;
        public event EventHandler PlaybackStarted;
        public event EventHandler PlaybackStopped;

        public AudioControl()
        {
            DoubleBuffered = true;
            MouseWheel += OnMouseWheel;
            MouseDown += OnMouseDown;
            MouseUp += OnMouseUp;
            MouseMove += OnMouseMove;
            Paint += OnPaint;
            MouseDoubleClick += OnMouseDoubleClick;
        }

        public float AudioLength => audioLength;

        public float LeftTime
        {
            get => leftTime;
            set
            {
                UpdateAudioView(value, rightTime);
            }
        }

        public float RightTime
        {
            get => rightTime;
            set
            {
                UpdateAudioView(leftTime, value);
            }
        }

        private float Quantize(float value)
        {
            return (float)(Math.Round(value / NAVIGATION_QUANTA_SECONDS, 0) * NAVIGATION_QUANTA_SECONDS);
        }

        private void UpdateAudioView(float newLeftTime, float newRightTime)
        {
            if (audioFile == null)
            {
                return;
            }
            if (newLeftTime == leftTime && newRightTime == rightTime)
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

            if (Math.Abs(newLeftTime - leftTime) >= 0.5f * NAVIGATION_QUANTA_SECONDS ||
                Math.Abs(newRightTime - rightTime) > 0.5f * NAVIGATION_QUANTA_SECONDS)
            {
                leftTime = newLeftTime;
                rightTime = newRightTime;
                UpdateSamplesPerPixel();
                BeginInvoke((Action)(() =>
                {
                    AudioViewChanged?.Invoke(this, new AudioViewChangedEventArgs(leftTime, rightTime));
                    Invalidate();
                }));
            }
        }

        public float AudioPosition
        {
            get => audioPosition;
            set
            {
                if (audioFile == null) return;
                UpdateAudioPosition(value);
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
            GenerateWaveform();

            isLoadingAudio = false;
            UpdateAudioView(0, audioLength);
            UpdateAudioPosition(0);
        }

        public void Play()
        {
            if (waveOut != null && !isPlaying)
            {
                isPlaying = true;
                waveOut.Play();
                Debug.Print("Play: waveOut.PlaybackState == " + waveOut.PlaybackState);
                new Thread(() =>
                {
                    Debug.Print("Playback thread: begin");
                    try
                    {
                        Debug.Print("Playback thread: try (waveOut.PlaybackState == " + waveOut.PlaybackState + ")");
                        while (waveOut.PlaybackState == PlaybackState.Playing)
                        {
                            Debug.Print("Playback thread: while Playing");
                            var newPosition = (float)audioFile.CurrentTime.TotalSeconds;
                            if (newPosition != audioPosition)
                            {
                                Debug.Print("Playback thread: new position");
                                audioPosition = newPosition;
                                BeginInvoke((Action)(() =>
                                {
                                    Debug.Print("Playback -> UI thread: AudioPositionChanged");
                                    AudioPositionChanged?.Invoke(this, new AudioPositionChangedEventArgs(audioPosition));
                                    Invalidate();
                                }));
                            }
                            Thread.Sleep(50);
                            Debug.Print("Playback thread: after sleep, " + waveOut.PlaybackState);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.Print("Playback thread: exception " + ex);
                    }
                    finally
                    {
                        Debug.Print("Playback thread: finally");
                        if (isPlaying)
                        {
                            isPlaying = false;
                            BeginInvoke((Action)(() => PlaybackStopped?.Invoke(this, EventArgs.Empty)));
                        }
                    }
                })
                { IsBackground = true }.Start();
                PlaybackStarted?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Pause()
        {
            if (waveOut != null && isPlaying)
            {
                waveOut.Pause();
                isPlaying = false;
                PlaybackStopped?.Invoke(this, EventArgs.Empty);
            }
        }

        private void GenerateWaveform()
        {
            var samples = new float[audioFile.Length / sizeof(float)];
            audioFile.ToSampleProvider().Read(samples, 0, samples.Length);

            waveformData = new float[samples.Length / 100]; // Downsample by a factor of 100 for performance
            for (int i = 0; i < waveformData.Length; i++)
            {
                waveformData[i] = Math.Abs(samples[i * 100]);
            }
        }

        private void UpdateSamplesPerPixel()
        {
            samplesPerPixel = waveformData.Length / (RightTime - LeftTime);
        }

        private float TimeAt(float x)
        {
            return leftTime + (rightTime - leftTime) * x / Width;
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(BackColor);

            if (isLoadingAudio)
            {
                g.Clear(Color.LightYellow);
            }

            if (waveformData == null) return;

            viewableWaveform.Draw(g, 0, ClientRectangle.Width, ClientRectangle.Y + ClientRectangle.Height / 2, ClientRectangle.Height / 2, leftTime, rightTime);

            /*var width = ClientSize.Width;
            var height = ClientSize.Height;

            for (int x = 0; x < width; x++)
            {
                int sampleIndex = (int)((LeftTime + (x / (float)width) * (RightTime - LeftTime)) * samplesPerPixel);
                if (sampleIndex >= 0 && sampleIndex < waveformData.Length)
                {
                    var amplitude = waveformData[sampleIndex] * height;
                    g.DrawLine(Pens.Green, x, height / 2 - amplitude / 2, x, height / 2 + amplitude / 2);
                }
            }*/

            // Draw AudioPosition line
            if (audioPosition >= LeftTime && audioPosition <= RightTime)
            {
                var positionX = (audioPosition - LeftTime) / (RightTime - LeftTime) * ClientSize.Width;
                g.DrawLine(Pens.Red, (int)positionX, 0, (int)positionX, ClientSize.Height);
            }
        }

        private void OnMouseWheel(object sender, MouseEventArgs e)
        {
            if (audioFile == null) return;

            var zoomAmount = e.Delta > 0 ? 0.9f : 1.1f;
            var mouseTime = TimeAt(e.X);

            UpdateAudioView(
                mouseTime - (mouseTime - leftTime) * zoomAmount,
                mouseTime + (rightTime - mouseTime) * zoomAmount
            );
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (audioFile == null) return;

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
    }
}
