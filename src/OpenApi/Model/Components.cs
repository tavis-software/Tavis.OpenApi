using System;
using System.Collections.Generic;
using SharpYaml.Serialization;

namespace Tavis.OpenApi.Model
{

    public class Components
    {
        public Dictionary<string, Schema> Definitions { get; set; } = new Dictionary<string, Schema>();
        public Dictionary<string, Parameter> Parameters { get; set; } = new Dictionary<string, Parameter>();
        public Dictionary<string, Response> Responses { get; set; } = new Dictionary<string, Response>();
        public Dictionary<string, SecurityScheme> SecurityDefinitions { get; set; } = new Dictionary<string, SecurityScheme>();
        public Dictionary<string, Callback> Callbacks { get; set; } = new Dictionary<string, Callback>();
        public Dictionary<string, Link> Links { get; set; } = new Dictionary<string, Link>();

        public Dictionary<string, string> Extensions { get; set; } = new Dictionary<string, string>();

        private static FixedFieldMap<Components> fixedFields = new FixedFieldMap<Components> {
            { "definitions", (o,n) => { o.Definitions = n.CreateMap(Schema.Load); } },
            { "parameters", (o,n) => o.Parameters = n.CreateMap(Parameter.Load) },
            { "responses", (o,n) => o.Responses = n.CreateMap(Response.Load) },
            //{ "responseHeaders", (o,n) => o.ResponseHeaders = n.CreateMap(ResponseHeader.Load) },
            { "securityDefinitions", (o,n) => o.SecurityDefinitions = n.CreateMap(SecurityScheme.Load) },
            { "callbacks", (o,n) => o.Callbacks = n.CreateMap(Callback.Load) },
            { "links", (o,n) => o.Links = n.CreateMap(Link.Load) },

            };

        private static PatternFieldMap<Components> patternFields = new PatternFieldMap<Components>
        {
            { (s)=> s.StartsWith("x-"), (o,k,n)=> o.Extensions.Add(k, n.GetScalarValue()) }
        };


        public static Components Load(ParseNode node)
        {
            var mapNode = node.CheckMapNode("components");

            var components = new Components();

            foreach (var itemNode in mapNode)
            {
                itemNode.ParseField(components, fixedFields, patternFields);
            }
            return components;
        }

    }
}