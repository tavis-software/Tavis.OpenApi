using System;
using SharpYaml.Serialization;
using System.Collections.Generic;

namespace Tavis.OpenApi.Model
{

    public class Response
    {

        public string Description { get; set; }
        public Content Content { get; set; }
        public Dictionary<string, Header> Headers { get; set; }
        public Dictionary<string, Link> Links { get; set; }
        public Dictionary<string, string> Extensions { get; set; }

        void Write(IParseNodeWriter writer)
        {
            writer.WriteStartMap();

            writer.WriteStringProperty("description", Description);
            writer.WriteObject("content", Content, Content.Write);

            writer.WriteMap("headers", Headers, Header.Write);
            writer.WriteMap("links", Links, Link.Write);

            //Links
            writer.WriteEndMap();
        }

      
        public static void Write(IParseNodeWriter writer, Response response)
        {
            response.Write(writer);
        }
    }
}