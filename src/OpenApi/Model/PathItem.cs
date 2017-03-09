using System;
using System.Collections.Generic;
using SharpYaml.Serialization;

namespace Tavis.OpenApi.Model

{

    public class PathItem
    {
        public Dictionary<string, string> Extensions { get; set; }

        public string Summary { get; set; }
        public string Description { get; set; }

        public Dictionary<string, Operation> Operations { get; set; } = new Dictionary<string, Operation>();

        public Server Server { get; set; }
        public List<Parameter> Parameters { get; set; } = new List<Parameter>();

        private static FixedFieldMap<PathItem> fixedFields = new FixedFieldMap<PathItem>
        {
            { "summary", (o,n) => { o.Summary = n.GetScalarValue(); } },
            { "description", (o,n) => { o.Description = n.GetScalarValue(); } },
            { "server", (o,n) => { o.Server = Server.Load(n)    ; } },
            { "parameters", (o,n) => { o.Parameters = n.CreateList(Parameter.Load); } },

        };

        private static PatternFieldMap<PathItem> patternFields = new PatternFieldMap<PathItem>
        {
            { (s)=> s.StartsWith("x-"), (o,k,n)=> o.Extensions.Add(k, n.GetScalarValue()) },
            { (s)=> "get,put,post,delete,patch,options,head".Contains(s),
                (o,k,n)=> o.Operations.Add(k, Operation.Load(n)    ) }
        };


        internal static PathItem Load(ParseNode node)
        {
            var mapNode = node.CheckMapNode("Path");
            var path = new PathItem();

            foreach(var property in mapNode)
            {
                property.ParseField(path, fixedFields, patternFields);
            }

            return path;
        }
    }
}