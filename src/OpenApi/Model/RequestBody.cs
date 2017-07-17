using SharpYaml.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tavis.OpenApi.Model
{
    public class RequestBody : IModel
    {
        public string Description { get; set; }
        public Boolean Required { get; set; }
        public Dictionary<string, MediaType> Content { get; set; }
        public Dictionary<string,string> Extensions { get; set; }

        void IModel.Write(IParseNodeWriter writer)
        {
            writer.WriteStartMap();

            writer.WriteStringProperty("description", Description);
            writer.WriteBoolProperty("required", Required, false);
            writer.WriteMap("content", Content, ModelHelper.Write);

            writer.WriteEndMap();
        }
        

    }
}
