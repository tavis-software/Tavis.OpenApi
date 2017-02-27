using SharpYaml.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tavis.OpenApi.Model
{
    public class VariableExpression
    {
        public static VariableExpression Load(string expression)
        {
            return new VariableExpression(expression);
        }

        string expression;
        public VariableExpression(string expression)
        {
            this.expression = expression;
        }

    }

    public class Callback
    {
        public Dictionary<string, Operation> Operations { get; set; } = new Dictionary<string, Operation>();

        public Dictionary<string, string> Extensions { get; set; }

        private static FixedFieldMap<Callback> fixedFields = new FixedFieldMap<Callback>
        {
        };

        private static PatternFieldMap<Callback> patternFields = new PatternFieldMap<Callback>
        {
             { (s)=> "get,put,post,delete,patch,options,head".Contains(s),
                (o,k,n)=> o.Operations.Add(k, Operation.Load(n)    ) }
        };

        public static Callback Load(ParseNode node)
        {
            var mapNode = node.CheckMapNode("callback");

            Callback domainObject = new Callback();

            foreach (var property in mapNode)
            {
                property.ParseField(domainObject, fixedFields, patternFields);
            }

            return domainObject;
        }
    }

    public class Callbacks : IReference
    {
        public Dictionary<string, Callback> Items { get; set; } = new Dictionary<string, Callback>();

        public Dictionary<string, string> Extensions { get; set; }


        public string Pointer { get; set; }

        private static FixedFieldMap<Callbacks> fixedFields = new FixedFieldMap<Callbacks>
        {
        };

        private static PatternFieldMap<Callbacks> patternFields = new PatternFieldMap<Callbacks>
        {
            { (s)=> s.StartsWith("x-"), (o,k,n)=> o.Extensions.Add(k, n.GetScalarValue()) },
            { (s)=> s.StartsWith("$"), (o,k,n)=> o.Items.Add(k, Callback.Load(n)    ) }
        };


        public static Callbacks Load(ParseNode node)
        {
            var mapNode = node.CheckMapNode("callbacks");
            
            Callbacks domainObject = mapNode.CreateOrReferenceDomainObject(() => new Callbacks());

            if (!domainObject.IsReference())
            {
                foreach (var property in mapNode)
                {
                    property.ParseField(domainObject, fixedFields, patternFields);
                }
            }

            return domainObject;
        }
    }
}
