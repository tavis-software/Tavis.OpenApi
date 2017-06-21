using System;
using System.Collections.Generic;
using SharpYaml.Serialization;
using System.Linq;

namespace Tavis.OpenApi.Model
{

    public class Operation
    {
        public List<Tag> Tags { get; set; } = new List<Tag>();
        public string Summary { get; set; }
        public string Description { get; set; }
        public ExternalDocs ExternalDocs { get; set; } 
        public string OperationId { get; set; }
        public List<Parameter> Parameters { get; set; } = new List<Parameter>();
        public RequestBody RequestBody { get; set; }
        public Dictionary<string, Response> Responses { get; set; } = new Dictionary<string, Response>();
        public Dictionary<string, Callback> Callbacks { get; set; } = new Dictionary<string, Callback>();

        const bool DeprecatedDefault = false;
        public bool Deprecated { get; set; } = DeprecatedDefault;
        public List<SecurityRequirement> Security { get; set; } = new List<SecurityRequirement>();
        public List<Server> Servers { get; set; } = new List<Server>();
        public Dictionary<string, AnyNode> Extensions { get; set; } = new Dictionary<string, AnyNode>();


        public void Write(IParseNodeWriter writer)
        {
            writer.WriteStartMap();
            writer.WriteList("tags", Tags, Tag.WriteRef);
            writer.WriteStringProperty("summary", Summary);
            writer.WriteStringProperty("description", Description);
            writer.WriteObject("externalDocs", ExternalDocs, ExternalDocs.Write);

            writer.WriteStringProperty("operationId", OperationId);
            writer.WriteList<Parameter>("parameters", Parameters, Parameter.Write);
            writer.WriteObject("requestBody",RequestBody, Model.RequestBody.Write);
            writer.WriteMap<Response>("responses", Responses, Response.Write);
            writer.WriteMap<Callback>("callbacks", Callbacks, Callback.Write);
            writer.WriteBoolProperty("deprecated", Deprecated, DeprecatedDefault);
            writer.WriteList("security", Security, SecurityRequirement.Write);
            writer.WriteList("servers", Servers, Server.Write);

            writer.WriteEndMap();
        }

    }
}