using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GarageLights.Audio
{
    public class AudioPlayer : IDisposable
    {
        private WaveOutEvent waveOut;
        private AudioFileReader audioFile;

        private float audioLength;

        private float audioPosition;
        private float timePlaybackStarted;

        private bool isLoadingAudio;
        private Task<bool> playingTask;

        public event EventHandler LoadingAudio;
        public event EventHandler<AudioLoadedEventArgs> AudioLoaded;
        public event EventHandler PlaybackStarted;
        public event EventHandler<AudioPositionChangedEventArgs> AudioPositionChanged;
        public event EventHandler<PlaybackErrorEventArgs> PlaybackError;
        public event EventHandler PlaybackStopped;

        public float AudioLength => audioLength;

        public bool Playing
        {
            get { return playingTask != null && !playingTask.IsCompleted; }
        }

        public float AudioPosition
        {
            get { return audioPosition; }
            set
            {
                if (!IsAudioLoaded) return;

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

        public bool IsLoadingAudio
        {
            get { return isLoadingAudio; }
        }

        public bool IsAudioLoaded
        {
            get { return audioFile != null && !isLoadingAudio; }
        }

        public void LoadAudio(string filePath)
        {
            if (isLoadingAudio)
            {
                throw new InvalidOperationException("Tried to LoadAudio while already loading audio");
            }
            isLoadingAudio = true;
            LoadingAudio?.Invoke(this, EventArgs.Empty);
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
        }

        private void LoadAudioWorker(string filePath)
        {
            var newAudioFile = new AudioFileReader(filePath);
            var newWaveOut = new WaveOutEvent();
            newWaveOut.Init(newAudioFile);

            waveOut?.Dispose();
            audioFile?.Dispose();

            audioFile = newAudioFile;
            waveOut = newWaveOut;
            audioLength = (float)audioFile.TotalTime.TotalSeconds;

            isLoadingAudio = false;
            AudioLoaded?.Invoke(this, new AudioLoadedEventArgs(audioFile));
            UpdateAudioPosition(0);
        }

        public void Play()
        {
            if (!IsAudioLoaded || Playing) { return; }

            var tcs = new TaskCompletionSource<bool>();
            playingTask = tcs.Task;

            timePlaybackStarted = (float)audioFile.CurrentTime.TotalSeconds;
            waveOut.Play();

            new Thread(() =>
            {
                try
                {
                    while (waveOut.PlaybackState == PlaybackState.Playing)
                    {
                        var newPosition = timePlaybackStarted + (float)waveOut.GetPosition() / audioFile.WaveFormat.AverageBytesPerSecond;
                        //Debug.Print("Playback thread: new position " + newPosition);
                        audioPosition = newPosition;
                        AudioPositionChanged?.Invoke(this, new AudioPositionChangedEventArgs(audioPosition));
                    }
                    tcs.TrySetResult(true);
                }
                catch (Exception ex)
                {
                    Debug.Print("Playback thread: exception " + ex);
                    if (waveOut.PlaybackState == PlaybackState.Playing)
                    {
                        waveOut.Pause();
                        audioPosition = timePlaybackStarted + (float)waveOut.GetPosition() / audioFile.WaveFormat.AverageBytesPerSecond;
                        waveOut.Stop();
                    }
                    PlaybackError?.Invoke(this, new PlaybackErrorEventArgs(new PlaybackException("An error occurred during playback", ex)));
                    tcs.SetException(ex);
                }
                finally
                {
                    Debug.Print("Playback thread: finally");
                    PlaybackStopped?.Invoke(this, EventArgs.Empty);
                }
            })
            { IsBackground = true }.Start();
            PlaybackStarted?.Invoke(this, EventArgs.Empty);
        }

        private void UpdateAudioPosition(float newAudioPosition)
        {
            audioPosition = Math.Max(0, Math.Min(newAudioPosition, audioLength));
            audioFile.Position = (long)(audioPosition * audioFile.WaveFormat.AverageBytesPerSecond);
            AudioPositionChanged?.Invoke(this, new AudioPositionChangedEventArgs(audioPosition));
        }

        public void Stop()
        {
            if (Playing)
            {
                waveOut.Pause();
                audioPosition = timePlaybackStarted + (float)waveOut.GetPosition() / audioFile.WaveFormat.AverageBytesPerSecond;
                waveOut.Stop();
                playingTask.Wait();
            }
        }

        public void UnloadAudio()
        {
            Stop();
            waveOut?.Dispose();
            audioFile?.Dispose();
            waveOut = null;
            audioFile = null;
            audioLength = 0;
        }

        public void Dispose()
        {
            UnloadAudio();
        }
    }

    public class AudioLoadedEventArgs : EventArgs
    {
        public readonly AudioFileReader AudioFile;

        public AudioLoadedEventArgs(AudioFileReader audioFile)
        {
            AudioFile = audioFile;
        }
    }

    public class PlaybackException : Exception
    {
        public PlaybackException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class PlaybackErrorEventArgs : EventArgs
    {
        public readonly PlaybackException Error;

        public PlaybackErrorEventArgs(PlaybackException error)
        {
            Error = error;
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
