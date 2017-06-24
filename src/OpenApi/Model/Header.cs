using SharpYaml.Serialization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Tavis.OpenApi.Model
{

    public class Header : IModel
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

        void IModel.Write(IParseNodeWriter writer)
        {
            writer.WriteStartMap();
            writer.WriteStringProperty("description", Description);
            writer.WriteBoolProperty("required", Required,false);
            writer.WriteBoolProperty("deprecated", Deprecated,false);
            writer.WriteBoolProperty("allowEmptyValue", AllowEmptyValue,false);
            writer.WriteStringProperty("style", Style);
            writer.WriteBoolProperty("explode", Explode,false);
            writer.WriteBoolProperty("allowReserved", AllowReserved,false);
            writer.WriteObject("schema", Schema, ModelHelper.Write);
            writer.WriteList("examples", Examples, AnyNode.Write);
            writer.WriteObject("example", Example, AnyNode.Write);
            writer.WriteMap("content", Content, ModelHelper.Write);

            writer.WriteEndMap();
        }
        
    }
}
