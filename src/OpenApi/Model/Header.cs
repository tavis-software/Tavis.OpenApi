using SharpYaml.Serialization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Tavis.OpenApi.Model
{

    public class Header
    {

        public string Name { get; set; }
        public string Description { get; set; }
        public bool Required { get; set; }
        public bool Deprecated { get; set; }
        public Schema Schema { get; set; }
        public bool AllowReserved { get; set; }
        public string Style { get; set; }
        public Dictionary<string, string> Extensions { get; set; }


        public void Write(IParseNodeWriter writer)
        {
            writer.WriteStartMap();

            writer.WriteEndMap();
        }

        public static void Write(IParseNodeWriter writer, Header header)
        {
            header.Write(writer);
        }

    }
}
