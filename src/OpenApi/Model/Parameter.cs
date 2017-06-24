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
        public bool Deprecated { get; set; } = false;
        public bool AllowEmptyValue { get; set; } = false;
        public string Style { get; set; }
        public bool Explode { get; set; }
        public bool AllowReserved { get; set; }
        public Schema Schema { get; set; }
        public List<AnyNode> Examples { get; set; } = new List<AnyNode>();
        public AnyNode Example { get; set; }
        public Dictionary<string, MediaType> Content { get; set; }
        public Dictionary<string, AnyNode> Extensions { get; set; } = new Dictionary<string, AnyNode>();

        public void Write(IParseNodeWriter writer)
        {
            writer.WriteStartMap();
            writer.WriteStringProperty("name", Name);
            writer.WriteStringProperty("in", In.ToString());
            writer.WriteStringProperty("description", Description);
            writer.WriteBoolProperty("required", Required, false);
            writer.WriteBoolProperty("deprecated", Deprecated, false);
            writer.WriteBoolProperty("allowEmptyValue", AllowEmptyValue, false);
            writer.WriteStringProperty("style", Style);
            writer.WriteBoolProperty("explode", Explode,false);
            writer.WriteBoolProperty("allowReserved", AllowReserved, false);
            writer.WriteObject("schema", Schema, Schema.Write);
            writer.WriteList("examples", Examples, AnyNode.Write);
            writer.WriteObject("example", Example, AnyNode.Write);
            writer.WriteMap("content", Content, MediaType.Write);
            writer.WriteEndMap();
        }

        public void WriteRef(IParseNodeWriter writer)
        {
            writer.WriteStartMap();
            writer.WriteStringProperty("$ref", this.Pointer);
            writer.WriteEndMap();
        }
        public static void WriteFull(IParseNodeWriter writer, Parameter parameter)
        {
            parameter.Write(writer);
        }
            public static void Write(IParseNodeWriter writer, Parameter parameter)
        {
            if (parameter.IsReference())
            {
                parameter.WriteRef(writer);
            }
            else
            {
                parameter.Write(writer);
            }
        }
    }
    }