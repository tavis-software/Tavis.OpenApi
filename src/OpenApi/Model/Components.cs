using System;
using System.Collections.Generic;
using SharpYaml.Serialization;

namespace Tavis.OpenApi.Model
{

    public class Components
    {
        public Dictionary<string, Schema> Schemas { get; set; } = new Dictionary<string, Schema>();
        public Dictionary<string, Response> Responses { get; set; } = new Dictionary<string, Response>();
        public Dictionary<string, Parameter> Parameters { get; set; } = new Dictionary<string, Parameter>();
        public Dictionary<string, Example> Examples { get; set; } = new Dictionary<string, Example>();
        public Dictionary<string, RequestBody> RequestBodies { get; set; } = new Dictionary<string, RequestBody>();
        public Dictionary<string, Header> Headers { get; set; } = new Dictionary<string, Header>();
        public Dictionary<string, SecurityScheme> SecuritySchemes { get; set; } = new Dictionary<string, SecurityScheme>();
        public Dictionary<string, Link> Links { get; set; } = new Dictionary<string, Link>();
        public Dictionary<string, Callback> Callbacks { get; set; } = new Dictionary<string, Callback>();
        public Dictionary<string, string> Extensions { get; set; } = new Dictionary<string, string>();

        public bool IsEmpty()
        {
            return !(this.Schemas.Count > 0
                || this.Responses.Count > 0
                || this.Parameters.Count > 0
                || this.Examples.Count > 0
                || this.RequestBodies.Count > 0
                || this.Headers.Count > 0 
                || this.SecuritySchemes.Count > 0
                || this.Links.Count > 0
                || this.Callbacks.Count > 0 
                || this.Extensions.Count > 0);

        }

        public void Write(IParseNodeWriter writer)
        {
            writer.WriteStartMap();

            writer.WriteMap("schemas", Schemas, Schema.Write);
            writer.WriteMap("responses", Responses, Response.Write);
            writer.WriteMap("parameters", Parameters, Parameter.Write);
            writer.WriteMap("examples", Examples, Example.Write);
            writer.WriteMap("requestBodies", RequestBodies, RequestBody.Write);
            writer.WriteMap("headers", Headers, Header.Write);
            writer.WriteMap("securitySchemes", SecuritySchemes, SecurityScheme.Write);
            writer.WriteMap("links", Links, Link.Write);
            writer.WriteMap("callbacks", Callbacks, Callback.Write);

            writer.WriteEndMap();
        }

        public static void Write(IParseNodeWriter writer, Components components)
        {
            components.Write(writer);
        }
    }
}