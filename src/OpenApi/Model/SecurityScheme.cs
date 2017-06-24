using SharpYaml.Serialization;
using System;
using System.Collections.Generic;

namespace Tavis.OpenApi.Model
{
    public class SecurityScheme : IModel, IReference
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

        public Dictionary<string, AnyNode> Extensions { get; set; } = new Dictionary<string, AnyNode>();

        public string Pointer
        {
            get; set;
        }

        internal static SecurityScheme LoadByReference(ParseNode node)
        {
            var schemeName = node.GetScalarValue();
            var context = node.Context;
            var schemeObject = (SecurityScheme)context.GetReferencedObject($"#/components/securitySchemes/{schemeName}");

            return schemeObject;
        }

        void IModel.Write(IParseNodeWriter writer)
        {
            writer.WriteStartMap();
            writer.WriteStringProperty("type",this.Type);
            switch(this.Type) {
                case "http" :
                    writer.WriteStringProperty("scheme", this.Scheme);
                    writer.WriteStringProperty("bearerFormat", this.BearerFormat);
                    break;
                case "oauth2":
                //writer.WriteStringProperty("scheme", this.Scheme);
                case "apikey":
                    writer.WriteStringProperty("in", this.In);
                    writer.WriteStringProperty("name", this.Name);

                    break;
            }
            writer.WriteEndMap();

        }
        

    }
}