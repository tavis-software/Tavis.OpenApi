using SharpYaml.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tavis.OpenApi.Model
{
    public class Content
    {
        public Dictionary<string, MediaType> ContentTypes { get; set; } = new Dictionary<string, MediaType>();
        public Dictionary<string, string> Extensions { get; set; } = new Dictionary<string, string>();


    }
}
