using SharpYaml.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tavis.OpenApi.Model
{
    public class Content
    {
        public Dictionary<string, MediaType> MediaTypes { get; set; } = new Dictionary<string, MediaType>();
        public Dictionary<string, string> Extensions { get; set; } = new Dictionary<string, string>();

        public void Write(IParseNodeWriter writer)
        {
            writer.WriteStartMap();
            foreach (var mediaType in MediaTypes)
            {
                writer.WriteObject(mediaType.Key, mediaType.Value, MediaType.Write);
            }
            writer.WriteEndMap();
        }

        public static void Write(IParseNodeWriter writer, Content content)
        {
            content.Write(writer);
        }
    }
}
