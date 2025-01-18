using NAudio.SoundFont;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GarageLights.Lights;
using Newtonsoft.Json;
using System.IO;

namespace GarageLights
{
    internal class Project
    {
        public string AudioFile;
        public List<Controller> Controllers;
        public List<ChannelNode> ChannelNodes;

        public static Project FromFile(string filename)
        {
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
