using NAudio.SoundFont;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using GarageLights.Show;
using GarageLights.Keyframes;
using GarageLights.Controllers;
using GarageLights.Channels;

namespace GarageLights
{
    internal class Project
    {
        public List<Controller> Controllers;
        public List<ChannelNode> ChannelNodes;
        public Show.Show Show;

        public static Project FromFile(string filename)
        {
            if (filename == null) { return new Project(); }
            JsonSerializer serializer = Serialization.GetSerializer();
            using (var r = new StreamReader(filename))
            using (var jr = new JsonTextReader(r))
            {
                Project project = serializer.Deserialize<Project>(jr);
                return project;
            }
        }

        public void Save(string filename)
        {
            JsonSerializer serializer = Serialization.GetSerializer();
            using (var w = new StreamWriter(filename))
            {
                serializer.Serialize(w, this);
            }
        }
    }
}
