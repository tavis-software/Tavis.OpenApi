using SharpYaml.Serialization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Tavis.OpenApi.Model
{

    public class Header
    {

        public string Description { get; set; }
        public bool Required { get; set; }
        public bool Deprecated { get; set; }
        public bool AllowEmptyValue { get; set; }
        public string Style { get; set; }
        public bool Explode { get; set; }
        public bool AllowReserved { get; set; }
        public Schema Schema { get; set; }
        public AnyNode Example { get; set; }
        public List<AnyNode> Examples { get; set; }
        public Dictionary<string, MediaType> Content { get; set; }

        public Dictionary<string, string> Extensions { get; set; }

        public void Write(IParseNodeWriter writer)
        {
            writer.WriteStartMap();
            writer.WriteStringProperty("description", Description);
            writer.WriteBoolProperty("required", Required);
            writer.WriteBoolProperty("deprecated", Deprecated);
            writer.WriteBoolProperty("allowEmptyValue", AllowEmptyValue);
            writer.WriteStringProperty("style", Style);
            writer.WriteBoolProperty("explode", Explode);
            writer.WriteBoolProperty("allowReserved", AllowReserved);
            writer.WriteObject("schema", Schema, Schema.Write);
            writer.WriteList("examples", Examples, AnyNode.Write);
            writer.WriteObject("example", Example, AnyNode.Write);
            writer.WriteMap("content", Content, MediaType.Write);

            writer.WriteEndMap();
        }

        public static void Write(IParseNodeWriter writer, Header header)
        {
            header.Write(writer);
        }

    }
}
