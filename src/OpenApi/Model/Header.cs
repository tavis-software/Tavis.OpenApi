using SharpYaml.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tavis.OpenApi.Model
{
    public class Header
    {


        public Dictionary<string, string> Extensions { get; set; }


        private static FixedFieldMap<Header> fixedFields = new FixedFieldMap<Header>
        {
        };

        private static PatternFieldMap<Header> patternFields = new PatternFieldMap<Header>
        {
            { (s)=> s.StartsWith("x-"), (o,k,n)=> o.Extensions.Add(k, n.GetScalarValue()) },
        };


        public static Header Load(ParseNode node)
        {
            var mapNode = node.CheckMapNode("header");
            var header = new Header();
            foreach (var property in mapNode)
            {
                property.ParseField(header, fixedFields, patternFields);
            }

            return header;
        }
    }
}
