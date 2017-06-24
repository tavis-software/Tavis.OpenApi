using System;
using SharpYaml.Serialization;
using System.Collections.Generic;

namespace Tavis.OpenApi.Model
{

    public class Response : IReference
    {

        public string Description { get; set; }
        public Dictionary<string, MediaType> Content { get; set; }
        public Dictionary<string, Header> Headers { get; set; }
        public Dictionary<string, Link> Links { get; set; }
        public Dictionary<string, string> Extensions { get; set; }

        public string Pointer
        {
            get; set;
        }

        void Write(IParseNodeWriter writer)
        {
            writer.WriteStartMap();

            writer.WriteStringProperty("description", Description);
            writer.WriteMap("content", Content, MediaType.Write);

            writer.WriteMap("headers", Headers, Header.Write);
            writer.WriteMap("links", Links, Link.Write);

            //Links
            writer.WriteEndMap();
        }


        public static void WriteFull(IParseNodeWriter writer, Response response)
        {
            response.Write(writer);
        }
        public static void Write(IParseNodeWriter writer, Response response)
        {
            if (response.IsReference())
            {
                response.WriteRef(writer);
            }
            else
            {
                response.Write(writer);
            }
        }
    }
}