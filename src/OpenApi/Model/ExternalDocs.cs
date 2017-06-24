using SharpYaml.Serialization;
using System;

namespace Tavis.OpenApi.Model
{

    public class ExternalDocs : IModel
    {
        public string Description { get; set; }
        public Uri Url { get; set; }

        void IModel.Write(IParseNodeWriter writer)
        {
            writer.WriteStartMap();
            writer.WriteStringProperty("description", this.Description);
            writer.WriteStringProperty("url", this.Url?.OriginalString);
            writer.WriteEndMap();
        }
        

    }
}