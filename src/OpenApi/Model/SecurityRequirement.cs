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

            writer.WriteStartMap();

            foreach (var scheme in Schemes)
            {
                
                writer.WritePropertyName(scheme.Key.Pointer.TypeName);
                writer.WriteStartList();
                foreach (var scope in scheme.Value)
                {
                    writer.WriteValue(scope);
                }
                writer.WriteEndList();
            }
            
            writer.WriteEndMap();
        }

        
    }
}