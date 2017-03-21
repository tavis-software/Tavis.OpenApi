using System;
using System.Collections.Generic;
using SharpYaml.Serialization;

namespace Tavis.OpenApi.Model
{

    public class Contact
    {
        public string Name { get; set; }
        public Uri Url { get; set; }
        public string Email { get; set; }
        public Dictionary<string,string> Extensions { get; set; }


        public Contact()
        {
            Extensions = new Dictionary<string, string>();
        }

        public void Write(IParseNodeWriter writer)
        {

            writer.WriteStartMap();

            writer.WriteStringProperty("name", Name);
            writer.WriteStringProperty("url", Url.OriginalString);
            writer.WriteStringProperty("email", Email);

            writer.WriteEndMap();

        }

    }
}