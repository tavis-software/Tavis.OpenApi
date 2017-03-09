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
        public Dictionary<string, MediaType> ContentTypes { get; set; } = new Dictionary<string, MediaType>();
        public Dictionary<string, string> Extensions { get; set; } = new Dictionary<string, string>();

        private static FixedFieldMap<Content> fixedFields = new FixedFieldMap<Content>
        {
        };

        private static PatternFieldMap<Content> patternFields = new PatternFieldMap<Content>
        {
            { (s)=> s.StartsWith("x-"), (o,k,n)=> o.Extensions.Add(k, n.GetScalarValue()) },
            { (s) => true, (o,k,n)=> o.ContentTypes.Add(k, MediaType.Load(n)    ) }
        };        
        
        public static Content Load(ParseNode node)
        {
            var mapNode = node.CheckMapNode("content");

            var content = new Content();
            foreach (var property in mapNode)
            {
                property.ParseField(content, fixedFields, patternFields);
            }

            return content;
        }

    }
}
