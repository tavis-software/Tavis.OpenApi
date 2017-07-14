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
        public decimal? Maximum { get; set; }
        public bool ExclusiveMaximum { get; set; } = false;
        public decimal? Minimum { get; set; }
        public bool ExclusiveMinimum { get; set; } = false;
        public int? MaxLength { get; set; }
        public int? MinLength { get; set; }
        public string Pattern { get; set; }
        public decimal MultipleOf { get; set; }
        public string Default { get; set; }
        public bool ReadOnly { get; set; }
        public bool WriteOnly { get; set; }
        public List<Schema> AllOf { get; set; }
        public List<Schema> OneOf { get; set; }
        public List<Schema> AnyOf { get; set; }
        public Schema Not { get; set; }
        public string[] Required { get; set; }
        public Schema Items { get; set; }
        public int? MaxItems { get; set; }
        public int? MinItems { get; set; }
        public bool UniqueItems { get; set; }
        public Dictionary<string,Schema> Properties { get; set; }
        public int? MaxProperties { get; set; }
        public int? MinProperties { get; set; }
        public bool AdditionalPropertiesAllowed { get; set; }
        public Schema AdditionalProperties { get; set; }

        public AnyNode Example { get; set; }
        public List<string> Enum { get; set; } = new List<string>();
        public bool Nullable { get; set; }
        public ExternalDocs ExternalDocs { get; set; }
        public bool Deprecated { get; set; }

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

            writer.WriteNumberProperty("maxLength", MaxLength);
            writer.WriteNumberProperty("minLength", MinLength);
            writer.WriteStringProperty("pattern", Pattern);
            writer.WriteStringProperty("default", Default);

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

            writer.WriteNumberProperty("maximum", Maximum);
            writer.WriteBoolProperty("exclusiveMaximum", ExclusiveMaximum, false);
            writer.WriteNumberProperty("minimum", Minimum);
            writer.WriteBoolProperty("exclusiveMinimum", ExclusiveMinimum, false);

            if (Items != null)
            {
                writer.WritePropertyName("items");
                ModelHelper.Write(writer, Items);
            }
            writer.WriteNumberProperty("maxItems", MaxItems);
            writer.WriteNumberProperty("minItems", MinItems);

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
            writer.WriteNumberProperty("maxProperties", MaxProperties);
            writer.WriteNumberProperty("minProperties", MinProperties);



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