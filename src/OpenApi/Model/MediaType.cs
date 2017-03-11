using SharpYaml.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tavis.OpenApi.Model
{
    public class MediaType
    {
        public Schema Schema { get; set; }
        public List<AnyNode> Examples { get; set; }
        public AnyNode Example { get; set; }

        public Dictionary<string, string> Extensions { get; set; }

        private static FixedFieldMap<MediaType> fixedFields = new FixedFieldMap<MediaType>
        {
            { "schema", (o,n) => { o.Schema = Schema.Load(n); } },
            { "examples", (o,n) => { o.Examples = ((ListNode)n).Select(s=> new AnyNode(s)).ToList(); } },
            { "example", (o,n) => { o.Example = new AnyNode(n); } },
        };

        private static PatternFieldMap<MediaType> patternFields = new PatternFieldMap<MediaType>
        {
            { (s)=> s.StartsWith("x-"), (o,k,n)=> o.Extensions.Add(k, n.GetScalarValue()) }
        };

        public static MediaType Load(ParseNode node)
        {
            var mapNode = node.CheckMapNode("contentType");
            var contentType = new MediaType();
            foreach (var property in mapNode)
            {
                property.ParseField(contentType, fixedFields, patternFields);
            }

            return contentType;
        }

    }
}
