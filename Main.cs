using GarageLights.Audio;
using GarageLights.Controllers;
using GarageLights.InputDevices.Definitions;
using GarageLights.InputDevices.UI;
using GarageLights.Lights;
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
using System.Diagnostics;
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
        private AudioPlayer audioPlayer;
        private IChannelInputDevice channelInputDevice;

        private ThrottledUiCall<float> updateAudioPosition;

        public frmMain()
        {
            updateAudioPosition = new ThrottledUiCall<float>(this, OnAudioPositionChange);
            InitializeComponent();
            audioPlayer = new AudioPlayer();
            audioPlayer.AudioPositionChanged += audioPlayer_AudioPositionChanged;
            audioPlayer.PlaybackError += audioPlayer_PlaybackError;
            multiquence1.AudioPlayer = audioPlayer;
            multiquence1.KeyframeManager.KeyframesChanged += keyframeManager_KeyframesChanged;
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
                saveToolStripMenuItem.Enabled = settings.ProjectFile != null && settings.ProjectFile != "";
                LoadChannelInputDevice(settings.ChannelInputDevice);
                if (settings.ProjectFile != null)
                {
                    LoadProject(settings.ProjectFile);
                }
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
            saveToolStripMenuItem.Enabled = settings.ProjectFile != null && settings.ProjectFile != "";
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (settings.ProjectFile != null)
            {
                ofdProject.FileName = Path.GetFileName(settings.ProjectFile);
                ofdProject.InitialDirectory = Path.GetFileName(settings.ProjectFile);
            }
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
            if (project.Controllers != null)
            {
                LoadControllerManager(project.Controllers);
            }
            Text = "Garage Lights - " + Path.GetFileName(filename);
        }

        private void LoadControllerManager(List<Controller> controllerDefinitions)
        {
            if (controllerManager != null)
            {
                controllerManager.Dispose();
            }
            controllerManager = new ControllerManager(controllerDefinitions);
        }

        private void audioPlayer_AudioPositionChanged(object sender, AudioPositionChangedEventArgs e)
        {
            UpdateControllers();
            updateAudioPosition.Trigger(e.AudioPosition);
        }

        private void keyframeManager_KeyframesChanged(object sender, EventArgs e)
        {
            UpdateControllers();
        }

        private void UpdateControllers()
        {
            if (controllerManager != null && multiquence1.KeyframeManager.Keyframes != null && audioPlayer != null)
            {
                var keyframes = multiquence1.KeyframeManager.GetKeyframesByControllerAndAddress(multiquence1.GetChannels());
                controllerManager.WriteValues(audioPlayer.AudioPosition, keyframes);
            }
        }

        private void OnAudioPositionChange(float audioPosition)
        {
            tsslAudioPosition.Text = audioPosition.ToString();
        }

        private void audioPlayer_PlaybackError(object sender, PlaybackErrorEventArgs e)
        {
            Invoke((Action)(() => {
                MessageBox.Show(this, e.ToString(), "Playback error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }));
        }

        private void tsbPlay_Click(object sender, EventArgs e)
        {
            audioPlayer.Play();
        }

        private void tsbStop_Click(object sender, EventArgs e)
        {
            audioPlayer.Stop();
        }

        private void selectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selector = new ChannelInputDeviceSelector();
            if (selector.ShowDialog(this) == DialogResult.OK)
            {
                ChannelInputDevice definition = selector.ChannelInputDevice;
                LoadChannelInputDevice(definition);
                settings.ChannelInputDevice = definition;
            }
        }

        private void LoadChannelInputDevice(ChannelInputDevice definition)
        {
            IChannelInputDevice newDevice = InputDevices.Implementations.ChannelInputDevice.Create(definition);
            if (channelInputDevice != null)
            {
                channelInputDevice.Error -= channelInputDevice_Error;
            }
            channelInputDevice = newDevice;
            channelInputDevice.Error += channelInputDevice_Error;
            multiquence1.ChannelInputDevice = channelInputDevice;
        }

        private void channelInputDevice_Error(object sender, ChannelInputDeviceErrorEventArgs e)
        {
            Invoke((Action)(() =>
            {
                MessageBox.Show(this, "Channel input device error: " + e.Exception.Message, "Channel input device error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Debug.Print(e.Exception.ToString());
            }));
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveProject();
        }

        private void saveasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (settings.ProjectFile != null)
            {
                sfdProject.FileName = Path.GetFileName(settings.ProjectFile);
                sfdProject.InitialDirectory = Path.GetDirectoryName(settings.ProjectFile);
            }
            if (sfdProject.ShowDialog(this) == DialogResult.OK)
            {
                SaveProject(sfdProject.FileName);
                Text = "Garage Lights - " + Path.GetFileName(sfdProject.FileName);
            }
        }

        private void SaveProject(string filename = null)
        {
            if (filename == null)
            {
                filename = settings.ProjectFile;
            }
            multiquence1.Project.Save(filename);
            settings.ProjectFile = filename;
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            multiquence1.Project = new Project();
            settings.ProjectFile = null;
            Text = "Garage Lights";
        }
    }
}
