using SharpYaml.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tavis.OpenApi.Model
{
    public class Example
    {
        public Dictionary<string, string> Extensions { get; set; }
        public AnyNode ExampleNode { get; set; }


        public void Write(IParseNodeWriter writer)
        {
                writer.WriteStartMap();

                writer.WriteEndMap();
        }

        public static void Write(IParseNodeWriter writer, Example example)
        {
            example.Write(writer);
        }
    }

   

}
