using System;
using System.Collections.Generic;
using SharpYaml.Serialization;
using System.Linq;

namespace Tavis.OpenApi.Model
{

    public class Operation
    {
        public string OperationId { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public bool Deprecated { get; set; }
        public List<Parameter> Parameters {get;set;}
        public RequestBody RequestBody { get; set; }
        public Dictionary<string,Response> Responses { get; set; }
        public string Host { get; set; }
        public string BasePath { get; set; }
        public string[] Schemes { get; set; }


        internal static Operation Load(YamlMappingNode value)
        {
            var operation = new Operation();

            foreach (var node in value.Children)
            {
                var key = ((YamlScalarNode)node.Key).Value;
                switch (key)
                {
                    case "summary":
                        operation.Summary = node.Value.GetScalarValue();
                        break;
                    case "description":
                        operation.Description = node.Value.GetScalarValue();
                        break;
                    case "operationId":
                        operation.OperationId = node.Value.GetScalarValue();
                        break;
                    case "deprecated":
                        operation.Deprecated = bool.Parse(node.Value.GetScalarValue());
                        break;

                    case "parameters":
                        operation.Parameters = node.Value.CreateList(Parameter.Load);
                        break;

                    case "requestBody":
                        operation.RequestBody = RequestBody.Load((YamlMappingNode)node.Value);
                        break;

                    case "responses":
                        operation.Responses = node.Value.CreateMap(Response.Load);
                        break;

                    case "security":
                        //operation.s = node.Value.GetScalarValue();
                        break;

                    case "host":
                        operation.Host = node.Value.GetScalarValue();
                        break;

                    case "basePath":
                        operation.BasePath = node.Value.GetScalarValue();
                        break;
                    case "schemes":
                        operation.Schemes = ((YamlSequenceNode)node.Value).Select(n => n.GetScalarValue()).ToArray() ;
                        break;

                }
            }
            return operation;
        }
    }
}