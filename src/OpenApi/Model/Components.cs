using System;
using System.Collections.Generic;
using SharpYaml.Serialization;

namespace Tavis.OpenApi.Model
{

    public class Components
    {
        public Dictionary<string, Schema> Definitions { get; set; }
        public Dictionary<string, Parameter> Parameters { get; set; }
        public Dictionary<string, Response> Responses { get; set; }
        public Dictionary<string, SecurityScheme> SecurityDefinitions { get; set; }
        public Dictionary<string, string> Extensions { get; set; }

        public static Components Load(YamlMappingNode value)
        {
            var components = new Components();

            foreach (var node in value.Children)
            {
                var key = (YamlScalarNode)node.Key;
                switch (key.Value)
                {
                    case "definitions":
                        components.Definitions = null;
                        break;

                    case "parameters":
                        components.Parameters = node.Value.CreateMap<Parameter>(Parameter.Load);
                        break;

                    case "responses":
                        components.Responses = null;
                        break;

                    case "securityDefinitions":
                        components.SecurityDefinitions = null;
                        break;


                }
            }
            return components;
        }

    }
}