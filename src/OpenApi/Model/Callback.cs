using SharpYaml.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tavis.OpenApi.Model
{

    public class Callback : IModel, IReference
    {
        public Dictionary<string, PathItem> PathItems { get; set; } = new Dictionary<string, PathItem>();

        public OpenApiReference Pointer { get; set; }

        public Dictionary<string, string> Extensions { get; set; }


        void IModel.Write(IParseNodeWriter writer)
        {
            writer.WriteStartMap();
            writer.WriteEndMap();
        }
        


    }
}
