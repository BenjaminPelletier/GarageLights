using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageLights
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class GarageLightsSettings
    {
        private string projectFile;

        public event EventHandler Changed;

        [JsonProperty]
        public string ProjectFile
        {
            get { return projectFile; }
            set
            {
                if (value != projectFile)
                {
                    projectFile = value;
                    Changed?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }
}
