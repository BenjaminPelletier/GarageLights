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
        private float[][] rms;

        public ViewableWaveform(AudioFileReader audioFile)
        {
            if (audioFile.WaveFormat.Channels != 2)
            {
                throw new NotImplementedException("Only stereo audio files are currently supported");
            }
            sampleRate = audioFile.WaveFormat.SampleRate;
            int nSamples = (int)(audioFile.Length / sizeof(float) / 2);
            int layers = (int)Math.Ceiling(Math.Log(nSamples, 2));

            // Read source data
            var waveform = new float[nSamples * 2];
            audioFile.ToSampleProvider().Read(waveform, 0, waveform.Length);

            // Allocate subsampled layers
            rms = new float[layers][];
            for (int layer = 0; layer < layers; layer++)
            {
                rms[layer] = new float[(int)Math.Ceiling((float)nSamples / (1 << layer))];
            }
            var sumSquares = new float[layers];

            // Compute values for subsampled layers
            for (int s = 0; s < nSamples; s++)
            {
                float value = waveform[s * 2];  // TODO: evaluate right channel as well
                rms[0][s] = Math.Abs(value);
                float value2 = value * value;
                for (int layer = 1; layer < layers; layer++)
                {
                    sumSquares[layer] += value2;
                    if ((s + 1) % (1 << layer) == 0)
                    {
                        rms[layer][((s + 1) >> layer) - 1] = (float)Math.Sqrt(sumSquares[layer] / (1 << layer));
                        sumSquares[layer] = 0;
                    }
                }
            }
        }

        public void Draw(Graphics g, int x0, int x1, float y0, float height, float t0, float t1)
        {
            Func<int, int> sampleOfX = x => (int)(sampleRate * (t0 + (t1 - t0) * (x - x0) / (x1 - x0)));
            int s0 = sampleOfX(x0);
            int s1 = sampleOfX(x1);
            int layer = (int)Math.Floor(Math.Log((s1 - s0) / (x1 - x0), 2));
            for (int x = x0; x <= x1; x++)
            {
                int i = sampleOfX(x) >> layer;
                float v = rms[layer][i];
                g.DrawLine(Pens.Blue, x, y0 - height * v, x, y0 + height * v);
            }
        }
    }
}
