using System;
using SharpYaml.Serialization;
using System.Collections.Generic;

namespace Tavis.OpenApi.Model
{

    public class Response
    {

        public string Description { get; set; }
        public Content Content { get; set; }

        public Dictionary<string,Header> Headers { get; set; }
        public Dictionary<string, Link> Links { get; set; }
        public Dictionary<string, Callback> Callbacks { get; set; }

        public Dictionary<string, string> Extensions { get; set; }


        private static FixedFieldMap<Response> fixedFields = new FixedFieldMap<Response>
        {
            { "description", (o,n) => { o.Description = n.GetScalarValue(); } },
            
            { "content", (o,n) => { o.Content = Content.Load((YamlMappingNode)n);  } },
            { "headers", (o,n) => { o.Headers = n.CreateMap(Header.Load); } },
            { "links", (o,n) => { o.Links = n.CreateMap(Link.Load); } },
            { "callback", (o,n) => { o.Callbacks = n.CreateMap(Callback.Load); } }
        };

        private static PatternFieldMap<Response> patternFields = new PatternFieldMap<Response>
        {
            { (s)=> s.StartsWith("x-"), (o,k,n)=> o.Extensions.Add(k, n.GetScalarValue()) },
        };

        public static Response Load(YamlMappingNode mapNode)
        {
            var response = new Response();
            foreach (var node in mapNode.Children)
            {
                var key = (YamlScalarNode)node.Key;
                ParseHelper.ParseField(key.Value, node.Value, response, fixedFields, patternFields);
            }

            return response;
        }

    }
}