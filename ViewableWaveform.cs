using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageLights
{
    internal class ViewableWaveform
    {
        private float sampleRate;
        private float[] waveform;

        public ViewableWaveform(AudioFileReader audioFile)
        {
            if (audioFile.WaveFormat.Channels != 2)
            {
                throw new NotImplementedException("Only stereo audio files are currently supported");
            }
            sampleRate = audioFile.WaveFormat.SampleRate;
            int nSamples = (int)(audioFile.Length / sizeof(float) / 2);

            // Read source data
            var samples = new float[nSamples * 2];
            audioFile.ToSampleProvider().Read(samples, 0, samples.Length);

            // Collapse to one-dimensional waveform
            // TODO: figure out what to do with right channel
            waveform = new float[nSamples];
            for (int s = 0; s< nSamples; s++)
            {
                waveform[s] = samples[s << 1];
            }
        }

        public void Draw(Graphics g, int x0, int x1, float y0, float height, float t0, float t1)
        {
            Func<int, int> sampleOfX = x => (int)(sampleRate * (t0 + (t1 - t0) * (x - x0) / (x1 - x0)));
            int s0 = sampleOfX(x0);
            for (int x = x0; x <= x1; x++)
            {
                int s1 = sampleOfX(x + 1);
                if (s1 > waveform.Length)
                {
                    s1 = waveform.Length;
                }
                float minValue = 0;
                float maxValue = 0;
                for (int s = s0; s < s1; s++)
                {
                    if (waveform[s] < minValue)
                    {
                        minValue = waveform[s];
                    }
                    if (waveform[s] > maxValue)
                    {
                        maxValue = waveform[s];
                    }
                }
                g.DrawLine(Pens.Blue, x, y0 - height * minValue, x, y0 - height * maxValue);
                s0 = s1;
            }
        }
    }
}
