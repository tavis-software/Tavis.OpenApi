using SharpYaml.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace Tavis.OpenApi.Model
{


    public class SecurityRequirement : IModel
    {
        public Dictionary<SecurityScheme, List<string>> Schemes { get; set; } = new Dictionary<SecurityScheme, List<string>>();


        void IModel.Write(IParseNodeWriter writer)
        {
                writer.WriteStartList();

                writer.WriteEndList();
        }

        
    }
}