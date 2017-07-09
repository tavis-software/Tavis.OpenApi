using System;
using System.Collections.Generic;
using System.Linq;

namespace Tavis.OpenApi.Model
{
 
    public class Schema : IModel, IReference

    {
        public string Title { get; set; }
        public string Type { get; set; }
        public string Format { get; set; }
        public string Description { get; set; }
        public string Minimum { get; set; }
        public string Maximum { get; set; }
        public string MinLength { get; set; }
        public string MaxLength { get; set; }
        public string Default { get; set; }
        public bool ReadOnly { get; set; }
        public List<Schema> AllOf { get; set; }
        public string[] Required { get; set; }
        public Schema Items { get; set; }
        public Dictionary<string,Schema> Properties { get; set; }
        public List<AnyNode> Examples { get; set; }
        public AnyNode Example { get; set; }
        public List<string> Enum { get; set; } = new List<string>();

        public Dictionary<string, AnyNode> Extensions { get; set; } = new Dictionary<string, AnyNode>();

        public OpenApiReference Pointer
        {
            get;
            set;
        }


        void IModel.Write(IParseNodeWriter writer)
        {
            writer.WriteStartMap();

            writer.WriteStringProperty("title", Title);
            writer.WriteStringProperty("type", Type);
            writer.WriteStringProperty("format", Format);
            writer.WriteStringProperty("description", Description);

            if (Required != null && Required.Length > 0)
            {
                writer.WritePropertyName("required");
                writer.WriteStartList();
                foreach (var name in Required)
                {
                    writer.WriteValue(name);
                }
                writer.WriteEndList();
            }

            if (Items != null)
            {
                writer.WritePropertyName("items");
                ModelHelper.Write(writer, Items);
            }
            if (Properties != null)
            {
                writer.WritePropertyName("properties");
                writer.WriteStartMap();
                foreach (var prop in Properties)
                {
                    writer.WritePropertyName(prop.Key);
                    if (prop.Value != null)
                    {
                        ModelHelper.Write(writer, prop.Value);
                    } else
                    {
                        writer.WriteValue("null");
                    }
                }
                writer.WriteEndMap();
            }

            if (Enum.Count > 0 )
            {
                writer.WritePropertyName("enum");
                writer.WriteStartList();
                foreach (var item in Enum)
                {
                    writer.WriteValue(item);
                }
                writer.WriteEndList();
            }
            writer.WriteEndMap();
        }
        
    }
}