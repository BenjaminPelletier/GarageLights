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

        private Multiquence multiquence1;
        private GarageLightsSettings settings;

        public frmMain()
        {
            InitializeComponent();

            multiquence1 = new Multiquence();
            this.multiquence1.Location = new Point(ClientRectangle.Left, toolStrip1.Bottom);
            this.multiquence1.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.multiquence1.Name = "multiquence1";
            this.multiquence1.Size = new Size(ClientSize.Width - 6, statusStrip1.Top - toolStrip1.Bottom - 12);
            this.multiquence1.TabIndex = 0;
            multiquence1.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
            this.Controls.Add(this.multiquence1);
            this.multiquence1.AudioPositionChanged += multiquence1_AudioPositionChanged;
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
            Text = "Garage Lights - " + Path.GetFileName(filename);
        }

        private void multiquence1_AudioPositionChanged(object sender, AudioControl.AudioPositionChangedEventArgs e)
        {
            tsslAudioPosition.Text = e.AudioPosition.ToString();
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
