using System;
using SharpYaml.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace Tavis.OpenApi.Model
{


    public enum InEnum
    {
        path,
        query,
        header
    }

    public class Parameter :IReference
    {
        public string Pointer { get; set; }
        public string Name { get; set; }
        public InEnum In { get; set; }  
        public string Description { get; set; }
        public bool Required { get; set; } = false;
        public bool Deprecated { get; set; }
        public bool AllowEmptyValue { get; set; } = false;

        public string Style { get; set; }
        public bool Explode { get; set; }
        public bool AllowReserved { get; set; }
        public Schema Schema { get; set; }
       
        public List<AnyNode> Examples { get; set; }
        public AnyNode Example { get; set; }
        public Dictionary<string, string> Extensions { get; set; }

        public Content Content { get; set; }
    }
}