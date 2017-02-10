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

        public static Server Load(ParseNode node)
        {
            var mapNode = node.CheckMapNode("server");

            var server = new Server();
            foreach (var property in mapNode)
            {
                property.ParseField(server, fixedFields, patternFields);
            }

            return server;
        }

    }

}