using System;
using SharpYaml.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace Tavis.OpenApi.Model
{


    public enum InEnum
    {
        path,
        query,
        header
    }

    public class Parameter : IReference
    {
        public string Pointer { get; set; }
        public string Name { get; set; }
        public InEnum In { get; set; }
        public string Description { get; set; }
        public bool Required { get; set; } = false;
        public bool Deprecated { get; set; }
        public bool AllowEmptyValue { get; set; } = false;

        public string Style { get; set; }
        public bool Explode { get; set; }
        public bool AllowReserved { get; set; }
        public Schema Schema { get; set; }

        public List<AnyNode> Examples { get; set; }
        public AnyNode Example { get; set; }
        public Dictionary<string, string> Extensions { get; set; }

        public Content Content { get; set; }



        public void Write(IParseNodeWriter writer)
        {
            writer.WriteStartMap();
            writer.WriteStringProperty("name", Name);
            writer.WriteStringProperty("in", In.ToString());
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
            writer.WriteObject("content", Content, Content.Write);
            writer.WriteEndMap();
        }

        public static void Write(IParseNodeWriter writer, Parameter parameter)
        {
            parameter.Write(writer);
        }
    }
    }