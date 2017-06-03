using System;
using System.Collections.Generic;
using SharpYaml.Serialization;

namespace Tavis.OpenApi.Model

{

    public class PathItem
    {

        public string Summary { get; set; }
        public string Description { get; set; }

        public Dictionary<string, Operation> Operations { get; set; } = new Dictionary<string, Operation>();

        public List<Server> Servers { get; set; } = new List<Server>();
        public List<Parameter> Parameters { get; set; } = new List<Parameter>();
        public Dictionary<string, string> Extensions { get; set; } = new Dictionary<string, string>();

        public void Write(IParseNodeWriter writer)
        {
            writer.WriteStartMap();
            writer.WriteStringProperty("summary", Summary);
            writer.WriteStringProperty("description", Description);
            if (Parameters != null && Parameters.Count >0)
            {
                writer.WritePropertyName("parameters");
                writer.WriteStartList();
                foreach (var parameter in this.Parameters)
                {
                    parameter.Write(writer);
                }
                writer.WriteEndList();

            }
            writer.WriteList("servers", Servers, Server.Write);

            foreach (var operationPair in Operations)
            {
                writer.WritePropertyName(operationPair.Key);
                operationPair.Value.Write(writer);
            }
            writer.WriteEndMap();
        }

        public static void Write(IParseNodeWriter writer, PathItem pathItem)
        {
            pathItem.Write(writer);
        }

    }
}