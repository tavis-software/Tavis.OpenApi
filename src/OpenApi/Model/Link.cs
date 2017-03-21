using SharpYaml.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tavis.OpenApi.Model
{
    public class Link : IReference
    {
        public string Href { get; set; }
        public string OperationId { get; set; }
        public Dictionary<string, VariableExpression> Parameters { get; set; }
        public Dictionary<string, Header> Headers { get; set; }
        public string Description { get; set; }
        public Dictionary<string, string> Extensions { get; set; }

        public string Pointer { get; set; }


        public void Write(IParseNodeWriter writer)
        {
            writer.WriteStartMap();
            writer.WriteStringProperty("href", Href);
            writer.WriteStringProperty("operationId", OperationId);
            writer.WriteMap("parameters", Parameters, (w, x) => { w.WriteValue(x.ToString()); });

            writer.WriteEndMap();
        }

        public static void Write(IParseNodeWriter writer, Link link)
        {
            link.Write(writer);
        }
    }
}
