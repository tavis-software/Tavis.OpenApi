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


        public Dictionary<string, string> Extensions { get; set; }


        private static FixedFieldMap<Response> fixedFields = new FixedFieldMap<Response>
        {
            { "description", (o,n) => { o.Description = n.GetScalarValue(); } },
            { "content", (o,n) => { o.Content = Content.Load(n); } },
            { "headers", (o,n) => { o.Headers = n.CreateMap(Header.Load); } },
            { "links", (o,n) => { o.Links = n.CreateMap(Link.Load); } }

        };

        private static PatternFieldMap<Response> patternFields = new PatternFieldMap<Response>
        {
            { (s)=> s.StartsWith("x-"), (o,k,n)=> o.Extensions.Add(k, n.GetScalarValue()) },
        };

        public static Response Load(ParseNode node)
        {
            var mapNode = node.CheckMapNode("response");

            var response = new Response();
            foreach (var property in mapNode)
            {
                property.ParseField(response, fixedFields, patternFields);
            }

            return response;
        }

    }
}