using SharpYaml.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tavis.OpenApi.Model
{
    public class Content
    {
        public Dictionary<string, ContentType> ContentTypes { get; set; } = new Dictionary<string, ContentType>();
        public Dictionary<string, string> Extensions { get; set; } = new Dictionary<string, string>();

        private static FixedFieldMap<Content> fixedFields = new FixedFieldMap<Content>
        {
        };

        private static PatternFieldMap<Content> patternFields = new PatternFieldMap<Content>
        {
            { (s)=> s.StartsWith("x-"), (o,k,n)=> o.Extensions.Add(k, n.GetScalarValue()) },
            { (s) => true, (o,k,n)=> o.ContentTypes.Add(k, ContentType.Load((YamlMappingNode)n)) }
        };        
        
        public static Content Load(YamlMappingNode mapNode)
        {
            var content = new Content();
            foreach (var node in mapNode.Children)
            {
                var key = (YamlScalarNode)node.Key;
                ParseHelper.ParseField(key.Value, node.Value, content, fixedFields, patternFields);
            }

            return content;
        }

    }
}
