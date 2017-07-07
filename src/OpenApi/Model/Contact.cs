using System;
using System.Collections.Generic;
using SharpYaml.Serialization;

namespace Tavis.OpenApi.Model
{
    public class Contact : IModel
    {
        public string Name { get; set; }
        public Uri Url { get; set; }
        public string Email { get; set; }
        public Dictionary<string, AnyNode> Extensions { get; set; } = new Dictionary<string, AnyNode>();

        
        void IModel.Write(IParseNodeWriter writer)
        {

            writer.WriteStartMap();

            writer.WriteStringProperty("name", Name);
            writer.WriteStringProperty("url", Url.OriginalString);
            writer.WriteStringProperty("email", Email);

            writer.WriteEndMap();
        }

    }
}