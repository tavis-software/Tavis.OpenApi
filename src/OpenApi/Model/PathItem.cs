﻿using System;
using System.Collections.Generic;
using SharpYaml.Serialization;

namespace Tavis.OpenApi.Model

{

    public class PathItem : IModel
    {

        public string Summary { get; set; }
        public string Description { get; set; }

        public Dictionary<string, Operation> Operations { get; set; } = new Dictionary<string, Operation>();

        public List<Server> Servers { get; set; } = new List<Server>();
        public List<Parameter> Parameters { get; set; } = new List<Parameter>();
        public Dictionary<string, AnyNode> Extensions { get; set; } = new Dictionary<string, AnyNode>();

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
                    ModelHelper.Write(writer,parameter);
                }
                writer.WriteEndList();

            }
            writer.WriteList("servers", Servers, ModelHelper.Write);

            foreach (var operationPair in Operations)
            {
                writer.WritePropertyName(operationPair.Key);
                ModelHelper.Write(writer, operationPair.Value);
            }
            writer.WriteEndMap();
        }

        public static void Write(IParseNodeWriter writer, PathItem pathItem)
        {
            pathItem.Write(writer);
        }

    }
}