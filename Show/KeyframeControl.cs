using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GarageLights.Show
{
    internal class KeyframeControl : UserControl
    {
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
            get => MaxTime;
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
            get => currentTime;
            set
            {
                currentTime = value;
                Invalidate();
            }
        }

        private void KeyframeControl_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(BackColor);
            if (maxTime == 0) { return; }

            if (leftTime <= currentTime && currentTime <= rightTime)
            {
                float x = (currentTime - leftTime) / (rightTime - leftTime) * ClientSize.Width;
                e.Graphics.DrawLine(Pens.Red, x, 0, x, ClientSize.Height);
            }
        }
    }
}
