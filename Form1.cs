using NAudio.CoreAudioApi;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GarageLights
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (audioControl1.Playing)
            {
                audioControl1.Pause();
            }
            else
            {
                audioControl1.Play();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            audioControl1.LoadAudio("C:\\Users\\bjpca\\Music\\Streamtagger\\202303\\01 Cravin'.mp3");
        }

        private void audioControl1_AudioViewChanged(object sender, AudioControl.AudioViewChangedEventArgs e)
        {
            label1.Text = e.LeftTime.ToString();
            label2.Text = e.RightTime.ToString();
        }

        private void audioControl1_AudioPositionChanged(object sender, AudioControl.AudioPositionChangedEventArgs e)
        {
            label3.Text = e.AudioPosition.ToString();
        }
    }
}
