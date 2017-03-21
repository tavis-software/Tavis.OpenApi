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
        public List<Parameter> Parameters {get;set;}
        public RequestBody RequestBody { get; set; }
        public Dictionary<string,Response> Responses { get; set; }
        public Dictionary<string, Callback> Callbacks { get; set; }
        public bool Deprecated { get; set; }
        public List<SecurityRequirement> Security { get; set; }
        public Server Server { get; set; }
        public Dictionary<string, string> Extensions { get; set; }


        public void Write(IParseNodeWriter writer)
        {
            writer.WriteStartMap();
            writer.WriteList("tags", Tags, Tag.Write);
            writer.WriteStringProperty("summary", Summary);
            writer.WriteStringProperty("description", Description);
            writer.WriteObject("externalDocs", ExternalDocs, ExternalDocs.Write);

            writer.WriteStringProperty("operationId", OperationId);

            writer.WriteList<Parameter>("parameters", Parameters, Parameter.Write);

            writer.WriteObject("requestBody",RequestBody, Model.RequestBody.Write);

            writer.WriteMap<Response>("responses", Responses, Response.Write);

            writer.WriteMap<Callback>("callbacks", Callbacks, Callback.Write);

            writer.WriteBoolProperty("deprecated", Deprecated);

            //operation.Security

            writer.WriteObject("server",Server, Server.Write);

            writer.WriteEndMap();
        }

    }
}