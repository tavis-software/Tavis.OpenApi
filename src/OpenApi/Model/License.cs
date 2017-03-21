using System;
using System.Collections.Generic;
using SharpYaml.Serialization;

namespace Tavis.OpenApi.Model
{

    public class License
    {
        public string Name { get; set; }
        public Uri Url { get; set; }
        public Dictionary<string, string> Extensions { get; set; }


        public License()
        {
            Extensions = new Dictionary<string, string>();
        }


        public void Write(IParseNodeWriter writer)
        {

            writer.WriteStartMap();

            writer.WriteStringProperty("name", Name);
            writer.WriteStringProperty("url", Url.OriginalString);

            writer.WriteEndMap();

        }

    }
}