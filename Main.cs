using GarageLights.Audio;
using GarageLights.Controllers;
using GarageLights.Properties;
using GarageLights.Show;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GarageLights
{
    public partial class frmMain : Form
    {
        const string SETTINGS_PATH = "settings.json";

        private GarageLightsSettings settings;
        private ControllerManager controllerManager;

        public frmMain()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            string settingsJson = File.Exists(SETTINGS_PATH) ? File.ReadAllText(SETTINGS_PATH) : null;
            LoadSettings(settingsJson);
        }

        void LoadSettings(string settingsJson)
        {
            if (settingsJson != null)
            {
                settings = JsonConvert.DeserializeObject<GarageLightsSettings>(settingsJson);
                settings.Changed += Settings_Changed;
                LoadProject(settings.ProjectFile);
            }
            else
            {
                settings = new GarageLightsSettings();
                settings.Changed += Settings_Changed;
            }
        }

        string GetSettingsString()
        {
            return JsonConvert.SerializeObject(settings, Formatting.Indented);
        }

        void SaveSettings()
        {
            string settingsJson = GetSettingsString();
            File.WriteAllText(SETTINGS_PATH, settingsJson);
        }

        void Settings_Changed(object sender, EventArgs e)
        {
            SaveSettings();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ofdProject.ShowDialog() == DialogResult.OK)
            {
                LoadProject(ofdProject.FileName);
            }
        }

        private void LoadProject(string filename)
        {
            Project project = Project.FromFile(filename);
            multiquence1.Project = project;
            settings.ProjectFile = filename;
            controllerManager = new ControllerManager(project.Controllers);
            Text = "Garage Lights - " + Path.GetFileName(filename);
        }

        private void multiquence1_AudioPositionChanged(object sender, AudioPositionChangedEventArgs e)
        {
            tsslAudioPosition.Text = e.AudioPosition.ToString();
        }

        private void multiquence1_PlaybackContinued(object sender, PlaybackContinuedEventArgs e)
        {
            if (controllerManager != null && e.Keyframes != null)
            {
                controllerManager.WriteValues(e.AudioPosition, e.Keyframes);
            }
        }

        private void multiquence1_PlaybackError(object sender, AudioControl.PlaybackErrorEventArgs e)
        {
            MessageBox.Show(this, e.ToString(), "Playback error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void tsbPlay_Click(object sender, EventArgs e)
        {
            multiquence1.Play();
        }

        private void tsbStop_Click(object sender, EventArgs e)
        {
            multiquence1.Stop();
        }
    }
}
