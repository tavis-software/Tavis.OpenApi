using SharpYaml.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tavis.OpenApi.Model
{
    public class ContentType
    {
        public Schema Schema { get; set; }
        public List<Example> Examples { get; set; }
        public Example Example { get; set; }

        public Dictionary<string, string> Extensions { get; set; }

        private static FixedFieldMap<ContentType> fixedFields = new FixedFieldMap<ContentType>
        {
            { "schema", (o,n) => { o.Schema = Schema.Load((YamlMappingNode)n); } },
            { "examples", (o,n) => { o.Examples=  n.CreateList(Example.Load); } },
            { "example", (o,n) => { o.Example=  Example.Load((YamlMappingNode)n); } },
        };

        private static PatternFieldMap<ContentType> patternFields = new PatternFieldMap<ContentType>
        {
            { (s)=> s.StartsWith("x-"), (o,k,n)=> o.Extensions.Add(k, n.GetScalarValue()) }
        };

        public static ContentType Load(YamlMappingNode mapNode)
        {
            var contentType = new ContentType();
            foreach (var node in mapNode.Children)
            {
                var key = (YamlScalarNode)node.Key;
                ParseHelper.ParseField(key.Value, node.Value, contentType, fixedFields, patternFields);
            }

            return contentType;
        }

    }
}
