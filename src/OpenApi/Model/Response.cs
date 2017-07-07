using System;
using SharpYaml.Serialization;
using System.Collections.Generic;

namespace Tavis.OpenApi.Model
{

    public class Response : IModel, IReference
    {

        public string Description { get; set; }
        public Dictionary<string, MediaType> Content { get; set; }
        public Dictionary<string, Header> Headers { get; set; }
        public Dictionary<string, Link> Links { get; set; }
        public Dictionary<string, AnyNode> Extensions { get; set; }

        public string Pointer
        {
            get; set;
        }

        void IModel.Write(IParseNodeWriter writer)
        {
            writer.WriteStartMap();

            writer.WriteStringProperty("description", Description);
            writer.WriteMap("content", Content, ModelHelper.Write);

            writer.WriteMap("headers", Headers, ModelHelper.Write);
            writer.WriteMap("links", Links, ModelHelper.Write);

            //Links
            writer.WriteEndMap();
        }

    }
}