using SharpYaml.Serialization;
using System;
using System.Collections.Generic;

namespace Tavis.OpenApi.Model
{
    public class SecurityScheme
    {
        public string Type { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string In { get; set; }
        public string Scheme { get; set; }
        public string BearerFormat { get; set; }
        public Uri OpenIdConnectUrl { get; set; }
        public string Flow { get; set; }
        public Uri AuthorizationUrl { get; set; }
        public Uri TokenUrl { get; set; }
        public Dictionary<string,string> Scopes { get; set; }

        public Dictionary<string, string> Extensions { get; set; } = new Dictionary<string, string>();

        private static FixedFieldMap<SecurityScheme> fixedFields = new FixedFieldMap<SecurityScheme>
        {
            { "type", (o,n) => { o.Type = n.GetScalarValue();  } },
            { "description", (o,n) => { o.Description = n.GetScalarValue();  } },
            { "name", (o,n) => { o.Name = n.GetScalarValue();  } },
            { "in", (o,n) => { o.In = n.GetScalarValue();  } },
            { "scheme", (o,n) => { o.Scheme = n.GetScalarValue();  } },
            { "bearerFormat", (o,n) => { o.BearerFormat = n.GetScalarValue();  } },
            { "openIdConnectUrl", (o,n) => { o.OpenIdConnectUrl = new Uri(n.GetScalarValue(), UriKind.RelativeOrAbsolute);  } },
            { "flow", (o,n) => { o.Flow = n.GetScalarValue();  } },
            { "authorizationUrl", (o,n) => { o.AuthorizationUrl = new Uri(n.GetScalarValue(), UriKind.RelativeOrAbsolute);  } },
            { "tokenUrl", (o,n) => { o.TokenUrl = new Uri(n.GetScalarValue(), UriKind.RelativeOrAbsolute);  } },
            { "scopes", (o,n) => { o.Scopes= n.CreateMap<string>(v => v.GetScalarValue()  ); } },
        };

        private static PatternFieldMap<SecurityScheme> patternFields = new PatternFieldMap<SecurityScheme>
        {
            { (s)=> s.StartsWith("x-"), (o,k,n)=> o.Extensions.Add(k, n.GetScalarValue()) }
        };

        public static SecurityScheme Load(ParseNode node)
        {
            var mapNode = node.CheckMapNode("securityScheme");

            var securityScheme = new SecurityScheme();
            foreach (var property in mapNode)
            {
                property.ParseField(securityScheme, fixedFields, patternFields);
            }

            return securityScheme;
        }

    }
}