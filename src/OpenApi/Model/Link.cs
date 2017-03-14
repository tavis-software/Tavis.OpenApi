using SharpYaml.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tavis.OpenApi.Model
{
    public class Link
    {
        public string Href { get; set; }
        public string OperationId { get; set; }
        public Dictionary<string, VariableExpression> Parameters { get; set; }
        public Dictionary<string, Header> Headers { get; set; }
        public string Description { get; set; }
        public Dictionary<string, string> Extensions { get; set; }

    }
}
