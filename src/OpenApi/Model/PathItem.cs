using System;
using System.Collections.Generic;
using SharpYaml.Serialization;

namespace Tavis.OpenApi.Model

{

    public class PathItem
    {
        public Dictionary<string, string> Extensions { get; set; }

        public string Summary { get; set; }
        public string Description { get; set; }

        public Dictionary<string, Operation> Operations { get; set; } = new Dictionary<string, Operation>();

        public Server Server { get; set; }
        public List<Parameter> Parameters { get; set; } = new List<Parameter>();

    }
}