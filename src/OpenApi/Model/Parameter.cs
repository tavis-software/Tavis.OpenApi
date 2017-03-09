using System;
using SharpYaml.Serialization;
using System.Collections.Generic;

namespace Tavis.OpenApi.Model
{


    public enum InEnum
    {
        path,
        query,
        header
    }

    public class Parameter :IReference
    {
        public string Pointer { get; set; }
        public string Name { get; set; }
        public InEnum In { get; set; }  
        public string Description { get; set; }
        public bool Required { get; set; }
        public bool Deprecated { get; set; }
        public Schema Schema { get; set; }
        public bool AllowReserved { get; set; }
        public string Style { get; set; }
        public Dictionary<string, string> Extensions { get; set; }


        private static FixedFieldMap<Parameter> fixedFields = new FixedFieldMap<Parameter>
        {
            { "name", (o,n) => { o.Name = n.GetScalarValue(); } },
            { "in", (o,n) => { o.In = (InEnum)Enum.Parse(typeof(InEnum), n.GetScalarValue()); } },
            { "description", (o,n) => { o.Description = n.GetScalarValue(); } },
            { "required", (o,n) => { o.Required = bool.Parse(n.GetScalarValue()); } },
            { "deprecated", (o,n) => { o.Deprecated = bool.Parse(n.GetScalarValue()); } },
            { "allowReserved", (o,n) => { o.AllowReserved = bool.Parse(n.GetScalarValue()); } },
            { "style", (o,n) => { o.Style = n.GetScalarValue(); } },
            { "schema", (o,n) => { o.Schema = Schema.Load(n); } },

        };

        private static PatternFieldMap<Parameter> patternFields = new PatternFieldMap<Parameter>
        {
            { (s)=> s.StartsWith("x-"), (o,k,n)=> o.Extensions.Add(k, n.GetScalarValue()) },
        };


        public static Parameter Load(ParseNode node)
        {
            var mapNode = node.CheckMapNode("parameter");

            var refpointer = mapNode.GetReferencePointer();
            if (refpointer != null)
            {
                return mapNode.GetReferencedObject<Parameter>(refpointer);
            }

            var parameter = new Parameter();

            foreach (var item in mapNode)
            {
                item.ParseField(parameter, fixedFields, patternFields);
            }

            return parameter;
        }
    }
}