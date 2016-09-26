using System;
using System.Collections.Generic;
using SharpYaml.Serialization;

namespace Tavis.OpenApi.Model

{

    public class Path
    {
        public string Summary { get; set; }
        public string Description { get; set; }

        public Dictionary<string, Operation> Operations {get;set;}

        public string Host { get; set; }

        public string BasePath { get; set; }

        public string[] Schemes { get; set; }

        public Dictionary<string, Parameter> Parameters { get; set; }

        public Path()
        {
            Operations = new Dictionary<string, Operation>();
        }

        internal static Path Load(YamlMappingNode value)
        {
            var path = new Path();

            foreach(var node in value.Children)
            {
                var key = ((YamlScalarNode)node.Key).Value;
                switch(key)
                {
                    case "summary":
                        path.Summary = node.Value.GetScalarValue();                        
                        break;

                    case "description":
                        path.Description = node.Value.GetScalarValue();
                        break;

                    case "host":
                        path.Host = node.Value.GetScalarValue();
                        break;

                    case "basePath":
                        path.BasePath = node.Value.GetScalarValue();
                        break;
                    case "schemes":
                        //path.Schemes = node.Value.GetScalarValue();
                        break;
                    case "get":
                    case "put":
                    case "post":
                    case "delete":
                    case "patch":
                    case "options":
                    case "head":
                        path.Operations.Add(key, Operation.Load((YamlMappingNode)node.Value));
                        break;

                    case "parameters":
                        //path.Parameters = Parameters.
                        break;
                }
            }

            return path;
        }
    }
}