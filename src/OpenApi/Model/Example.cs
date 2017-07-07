using SharpYaml.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tavis.OpenApi.Model
{
    public class Example : IModel, IReference
    {
        public string Summary { get; set; }
        public string Description { get; set; }

        public AnyNode Value { get; set; }
        public Dictionary<string, AnyNode> Extensions { get; set; }

        public string Pointer
        {
            get; set;
        }

        void IModel.Write(IParseNodeWriter writer)
        {
            writer.WriteStartMap();
            writer.WriteStringProperty("summary", Summary);
            writer.WriteStringProperty("description", Description);
            if (Value != null)
            {
                writer.WritePropertyName("value");
                Value.Write(writer);
            }
            writer.WriteEndMap();
        }

     }

   

}
