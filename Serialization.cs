using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageLights
{
    internal static class Serialization
    {
        public static JsonSerializer GetSerializer()
        {
            var serializer = new JsonSerializer();
            return serializer;
        }
    }
}
