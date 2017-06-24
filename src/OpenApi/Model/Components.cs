﻿using System;
using System.Collections.Generic;
using SharpYaml.Serialization;

namespace Tavis.OpenApi.Model
{

    public class Components : IModel
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

        void IModel.Write(IParseNodeWriter writer)
        {
            writer.WriteStartMap();

            writer.WriteMap("schemas", Schemas, ModelHelper.WriteFull);
            writer.WriteMap("responses", Responses, ModelHelper.WriteFull);
            writer.WriteMap("parameters", Parameters, ModelHelper.WriteFull);
            writer.WriteMap("examples", Examples, ModelHelper.WriteFull);
            writer.WriteMap("requestBodies", RequestBodies, ModelHelper.WriteFull);
            writer.WriteMap("headers", Headers, ModelHelper.WriteFull);
            writer.WriteMap("securitySchemes", SecuritySchemes, ModelHelper.WriteFull);
            writer.WriteMap("links", Links, ModelHelper.WriteFull);
            writer.WriteMap("callbacks", Callbacks, ModelHelper.WriteFull);

            writer.WriteEndMap();
        }


    }
}