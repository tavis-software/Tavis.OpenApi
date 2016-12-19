using SharpYaml.Serialization;
using System.Collections.Generic;

namespace Tavis.OpenApi.Model
{

    public class Server
    {
        public string Description { get; set; }
        public string Url { get; set; }
        public Dictionary<string, ServerTemplate> Templates { get; set; } = new Dictionary<string, ServerTemplate>();

        public Dictionary<string, string> Extensions { get; set; } = new Dictionary<string, string>();

        private static FixedFieldMap<Server> fixedFields = new FixedFieldMap<Server>
        {
            { "description", (o,n) => { o.Description = n.GetScalarValue();  } },
            { "url", (o,n) => { o.Url=  n.GetScalarValue(); } },
            { "templates", (o,n) => {  o.Templates = n.CreateMap(ServerTemplate.Load); } }
        };

        private static PatternFieldMap<Server> patternFields = new PatternFieldMap<Server>
        {
            { (s)=> s.StartsWith("x-"), (o,k,n)=> o.Extensions.Add(k, n.GetScalarValue()) }
        };

        public static Server Load(YamlMappingNode mapNode)
        {
            var server = new Server();
            foreach (var node in mapNode.Children)
            {
                var key = (YamlScalarNode)node.Key;
                ParseHelper.ParseField(key.Value, node.Value, server, fixedFields, patternFields);
            }

            return server;
        }

    }

}