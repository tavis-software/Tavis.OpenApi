using System;
using System.Collections.Generic;
using System.Linq;

namespace Tavis.OpenApi.Model
{
 
    public class Schema : IReference

    {
        public string Title { get; set; }
        public string Type { get; set; }
        public string Format { get; set; }
        public string Description { get; set; }
        public List<Schema> AllOf { get; set; }
        public string[] Required { get; set; }
        public Schema Items { get; set; }
        public Dictionary<string,Schema> Properties { get; set; }
        public List<AnyNode> Examples { get; set; }
        public AnyNode Example { get; set; }
        public List<string> Enum { get; set; } = new List<string>();

        public Dictionary<string, string> Extensions { get; set; }

        public string Pointer
        {
            get;
            set;
        }


        public void Write(IParseNodeWriter writer)
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
                Items.Write(writer);
            }
            if (Properties != null)
            {
                writer.WritePropertyName("properties");
                writer.WriteStartMap();
                foreach (var prop in Properties)
                {
                    writer.WritePropertyName(prop.Key);
                    prop.Value.Write(writer);
                }
                writer.WriteEndMap();
            }

            writer.WriteEndMap();
        }

        public static void Write(IParseNodeWriter writer, Schema schema)
        {
            schema.Write(writer);
        }

    }
}