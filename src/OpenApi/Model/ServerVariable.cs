using SharpYaml.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tavis.OpenApi.Model
{
    public class ServerVariable : IModel
    {
        public string Description { get; set; }
        public string Default { get; set; }
        public List<string> Enum { get; set; } = new List<string>();

        public Dictionary<string, string> Extensions { get; set; } = new Dictionary<string, string>();

        void IModel.Write(IParseNodeWriter writer)
        {
            writer.WriteStartMap();

            writer.WriteList("enum", Enum, (nodeWriter, s) => nodeWriter.WriteValue(s));
            writer.WriteStringProperty("default", Default);
            writer.WriteStringProperty("description", Description);

            writer.WriteEndMap();

        }
        

    }
}
