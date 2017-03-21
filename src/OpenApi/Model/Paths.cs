using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpYaml.Serialization;

namespace Tavis.OpenApi.Model
{
    public class Paths
    {
        public IDictionary<string, PathItem> PathItems { get; set; }
        public Dictionary<string, string> Extensions { get; set; }


        public Paths()
        {
            PathItems = new Dictionary<string, PathItem>();  
        }

        public PathItem GetPath(string key)
        {
            return PathItems[key];
        }
        internal void Validate(List<string> errors)
        {
            //TODO:
        }


        public void Write(IParseNodeWriter writer)
        {
            writer.WriteStartMap();
            foreach (var pathItem in PathItems)
            {
                writer.WritePropertyName(pathItem.Key);
                pathItem.Value.Write(writer);
            }
            writer.WriteEndMap();
        }

    }
}
