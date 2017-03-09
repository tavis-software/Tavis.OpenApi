using SharpYaml.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tavis.OpenApi.Model
{

    public class Headers : Dictionary<string, Header>
    {
        public static Headers Load(ParseNode node)
        {
            var mapNode = node.CheckMapNode("header");

            var headers = new Headers();
            var nodes = mapNode.Select(n => new KeyValuePair<string,Header>(n.GetScalarValue(), Header.Load(n) ));

            foreach (var item in nodes)
            {
                headers.Add(item.Key,item.Value );
            }

            return headers;
        }
    } 

    public class Header
    {

        public string Name { get; set; }
        public string Description { get; set; }
        public bool Required { get; set; }
        public bool Deprecated { get; set; }
        public Schema Schema { get; set; }
        public bool AllowReserved { get; set; }
        public string Style { get; set; }
        public Dictionary<string, string> Extensions { get; set; }

        private static FixedFieldMap<Header> fixedFields = new FixedFieldMap<Header>
        {
            { "name", (o,n) => { o.Name = n.GetScalarValue(); } },
            { "description", (o,n) => { o.Description = n.GetScalarValue(); } },
            { "required", (o,n) => { o.Required = bool.Parse(n.GetScalarValue()); } },
            { "deprecated", (o,n) => { o.Deprecated = bool.Parse(n.GetScalarValue()); } },
            { "allowReserved", (o,n) => { o.AllowReserved = bool.Parse(n.GetScalarValue()); } },
            { "style", (o,n) => { o.Style = n.GetScalarValue(); } },
            { "schema", (o,n) => { o.Schema = Schema.Load(n); } }
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
