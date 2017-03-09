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

    public class Callback : IReference
    {
        public Dictionary<string, PathItem> PathItems { get; set; } = new Dictionary<string, PathItem>();

        public string Pointer { get; set; }

        public Dictionary<string, string> Extensions { get; set; }

        private static FixedFieldMap<Callback> fixedFields = new FixedFieldMap<Callback>
        {
        };

        private static PatternFieldMap<Callback> patternFields = new PatternFieldMap<Callback>
        {
             { (s)=> s.StartsWith("$"),
                (o,k,n)=> o.PathItems.Add(k, PathItem.Load(n)    ) }
        };

        public static Callback Load(ParseNode node)
        {
            var mapNode = node.CheckMapNode("callback");

            var refpointer = mapNode.GetReferencePointer();
            if (refpointer != null)
            {
                return mapNode.GetReferencedObject<Callback>(refpointer);
            }

            var domainObject = new Callback();

            foreach (var property in mapNode)
            {
                property.ParseField(domainObject, fixedFields, patternFields);
            }
            
            return domainObject;
        }
    }
}
