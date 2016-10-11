using System;
using System.Collections.Generic;
using SharpYaml.Serialization;

namespace Tavis.OpenApi.Model

{

    public class Path
    {
        public Dictionary<string, string> Extensions { get; set; }

        public string Summary { get; set; }
        public string Description { get; set; }

        public Dictionary<string, Operation> Operations { get; set; } = new Dictionary<string, Operation>();

        public Server Server { get; set; }
        public List<Parameter> Parameters { get; set; } = new List<Parameter>();

        private static FixedFieldMap<Path> fixedFields = new FixedFieldMap<Path>
        {
            { "summary", (o,n) => { o.Summary = n.GetScalarValue(); } },
            { "description", (o,n) => { o.Description = n.GetScalarValue(); } },
            { "server", (o,n) => { o.Server = Server.Load((YamlMappingNode)n); } },
            { "parameters", (o,n) => { o.Parameters = n.CreateList(Parameter.Load); } },

        };

        private static PatternFieldMap<Path> patternFields = new PatternFieldMap<Path>
        {
            { (s)=> s.StartsWith("x-"), (o,k,n)=> o.Extensions.Add(k, n.GetScalarValue()) },
            { (s)=> "get,put,post,delete,patch,options,head".Contains(s),
                (o,k,n)=> o.Operations.Add(k, Operation.Load((YamlMappingNode)n)) }
        };


        internal static Path Load(YamlMappingNode value)
        {
            var path = new Path();

            foreach(var node in value.Children)
            {
                var key = (YamlScalarNode)node.Key;
                ParseHelper.ParseField(key.Value, node.Value, path, fixedFields, patternFields);
            }

            return path;
        }
    }
}