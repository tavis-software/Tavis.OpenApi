using SharpYaml.Serialization;
using System.Collections.Generic;

namespace Tavis.OpenApi.Model
{
 
    public class Schema
    {
        public string Type { get; set; }
        public string Format { get; set; }
        public string[] Required { get; set; }
        public Schema Items { get; set; }
        public Dictionary<string,Schema> Properties { get; set; }

        public Dictionary<string, string> Extensions { get; set; }

        private static FixedFieldMap<Schema> fixedFields = new FixedFieldMap<Schema>
        {
                { "type", (o,n) => { o.Type = n.GetScalarValue(); } },
                { "format", (o,n) => { o.Format = n.GetScalarValue(); } },
                { "required", (o,n) => { o.Required = YamlHelper.CreateSimpleList<string>(n, n2 => n2.GetScalarValue()).ToArray(); } },
                { "items", (o,n) => { o.Items = Schema.Load((YamlMappingNode)n); } },
                { "properties", (o,n) => { o.Properties = n.CreateMap(Schema.Load); } },
        };

        private static PatternFieldMap<Schema> patternFields = new PatternFieldMap<Schema>
        {
            { (s)=> s.StartsWith("x-"), (o,k,n)=> o.Extensions.Add(k, n.GetScalarValue()) }
        };


        public static Schema Load(YamlMappingNode mapNode)
        {
            var contentType = new Schema();
            foreach (var node in mapNode.Children)
            {
                var key = (YamlScalarNode)node.Key;
                ParseHelper.ParseField(key.Value, node.Value, contentType, fixedFields, patternFields);
            }

            return contentType;
        }

    }
}