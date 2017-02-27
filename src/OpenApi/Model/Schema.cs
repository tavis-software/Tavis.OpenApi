using System;
using System.Collections.Generic;

namespace Tavis.OpenApi.Model
{
 
    public class Schema : IReference

    {
        public string Type { get; set; }
        public string Format { get; set; }
        public string[] Required { get; set; }
        public Schema Items { get; set; }
        public Dictionary<string,Schema> Properties { get; set; }

        public Dictionary<string, string> Extensions { get; set; }

        public string Pointer
        {
            get;
            set;
        }

        private static FixedFieldMap<Schema> fixedFields = new FixedFieldMap<Schema>
        {
                { "type", (o,n) => { o.Type = n.GetScalarValue(); } },
                { "format", (o,n) => { o.Format = n.GetScalarValue(); } },
                { "required", (o,n) => { o.Required = n.CreateSimpleList<string>(n2 => n2.GetScalarValue()).ToArray(); } },
                { "items", (o,n) => { o.Items = Schema.Load(n); } },
                { "properties", (o,n) => { o.Properties = n.CreateMap(Schema.Load); } },
        };

        private static PatternFieldMap<Schema> patternFields = new PatternFieldMap<Schema>
        {
            { (s)=> s.StartsWith("x-"), (o,k,n)=> o.Extensions.Add(k, n.GetScalarValue()) }
        };


        public static Schema Load(ParseNode node)
        {
            MapNode mapNode = node.CheckMapNode("schema");

            Schema domainObject = mapNode.CreateOrReferenceDomainObject(()=> new Schema());

            foreach (var propertyNode in mapNode)
            {
                propertyNode.ParseField(domainObject, fixedFields, patternFields);
            }

            return domainObject;
        }



    }
}