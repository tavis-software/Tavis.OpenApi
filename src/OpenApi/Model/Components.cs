using System;
using System.Collections.Generic;
using SharpYaml.Serialization;

namespace Tavis.OpenApi.Model
{

    public class Components
    {
        public Dictionary<string, Schema> Schemas { get; set; } = new Dictionary<string, Schema>();
        public Dictionary<string, Parameter> Parameters { get; set; } = new Dictionary<string, Parameter>();
        public Dictionary<string, Response> Responses { get; set; } = new Dictionary<string, Response>();
        public Dictionary<string, Headers> ResponseHeaders { get; set; } = new Dictionary<string, Headers>();
        public Dictionary<string, SecurityScheme> SecuritySchemes { get; set; } = new Dictionary<string, SecurityScheme>();
        public Dictionary<string, Callback> Callbacks { get; set; } = new Dictionary<string, Callback>();
        public Dictionary<string, Link> Links { get; set; } = new Dictionary<string, Link>();
        public Dictionary<string, string> Extensions { get; set; } = new Dictionary<string, string>();

        public bool IsEmpty()
        {
            return !(this.Schemas.Count > 0 
                || this.Parameters.Count > 0 
                || this.Responses.Count > 0
                || this.ResponseHeaders.Count > 0 
                || this.SecuritySchemes.Count > 0
                || this.Callbacks.Count > 0 
                || this.Links.Count > 0 
                || this.Extensions.Count > 0);

        }

    }
}