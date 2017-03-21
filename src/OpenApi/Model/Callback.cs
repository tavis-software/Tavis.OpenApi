using SharpYaml.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tavis.OpenApi.Model
{

    public class Callback : IReference
    {
        public Dictionary<string, PathItem> PathItems { get; set; } = new Dictionary<string, PathItem>();

        public string Pointer { get; set; }

        public Dictionary<string, string> Extensions { get; set; }


        public void Write(IParseNodeWriter writer)
        {
            writer.WriteStartMap();
            writer.WriteEndMap();
        }

        public static void Write(IParseNodeWriter writer, Callback callback)
        {
            callback.Write(writer);
        }


    }
}
