using SharpYaml.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tavis.OpenApi.Model
{
    public class Link
    {


        public Dictionary<string, string> Extensions { get; set; }


        private static FixedFieldMap<Link> fixedFields = new FixedFieldMap<Link>
        {
        };

        private static PatternFieldMap<Link> patternFields = new PatternFieldMap<Link>
        {
            { (s)=> s.StartsWith("x-"), (o,k,n)=> o.Extensions.Add(k, n.GetScalarValue()) },
        };



        public static Link Load(ParseNode node)
        {
            var mapNode = node.CheckMapNode("link");
            var link = new Link();
            foreach (var property in mapNode)
            {
               property.ParseField(link, fixedFields, patternFields);
            }

            return link;
        }
    }
}
