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
        public Server Server { get; set; }
        public Dictionary<string, string> Extensions { get; set; }


        private static FixedFieldMap<Operation> fixedFields = new FixedFieldMap<Operation>
        {
            { "operationId", (o,n) => { o.OperationId = n.GetScalarValue(); } },
            { "summary", (o,n) => { o.Summary = n.GetScalarValue(); } },
            { "description", (o,n) => { o.Description = n.GetScalarValue(); } },
            { "deprecated", (o,n) => { o.Deprecated = bool.Parse(n.GetScalarValue()); } },
            { "requestBody", (o,n) => { o.RequestBody = RequestBody.Load(n)    ; } },
            { "responses", (o,n) => { o.Responses = n.CreateMap(Response.Load); } },
            { "server", (o,n) => { o.Server = Server.Load(n)    ; } },
//            { "security", (o,n) => { o.Se = n.GetScalarValue(); } },
            { "parameters", (o,n) => { o.Parameters = n.CreateList(Parameter.Load); } },
        };

        private static PatternFieldMap<Operation> patternFields = new PatternFieldMap<Operation>
        {
            { (s)=> s.StartsWith("x-"), (o,k,n)=> o.Extensions.Add(k, n.GetScalarValue()) },
        };


        internal static Operation Load(ParseNode node)
        {
            var mapNode = node.CheckMapNode("Operation");
            var operation = new Operation();

            foreach (var property in mapNode)
            {
                property.ParseField(operation, fixedFields, patternFields);
            }

            return operation;
        }
    }
}