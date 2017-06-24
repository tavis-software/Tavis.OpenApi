using SharpYaml.Serialization;
using System.Collections.Generic;

namespace Tavis.OpenApi.Model
{

    public class Server : IModel
    {
        public string Description { get; set; }
        public string Url { get; set; }
        public Dictionary<string, ServerVariable> Variables { get; set; } = new Dictionary<string, ServerVariable>();

        public Dictionary<string, string> Extensions { get; set; } = new Dictionary<string, string>();

        void IModel.Write(IParseNodeWriter writer)
        {
            writer.WriteStartMap();

            writer.WriteStringProperty("url", Url);
            writer.WriteStringProperty("description", Description);

            writer.WriteMap("variables", Variables, ModelHelper.Write);
            writer.WriteEndMap();
        }
        
    }

}