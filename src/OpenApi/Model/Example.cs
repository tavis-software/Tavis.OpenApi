using SharpYaml.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tavis.OpenApi.Model
{
    public class Example
    {
        public Dictionary<string, string> Extensions { get; set; }

        private static FixedFieldMap<Example> fixedFields = new FixedFieldMap<Example>
        {
        };

        private static PatternFieldMap<Example> patternFields = new PatternFieldMap<Example>
        {
            { (s)=> s.StartsWith("x-"), (o,k,n)=> o.Extensions.Add(k, n.GetScalarValue()) }
        };

        public static Example Load(ParseNode node)
        {
            var mapNode = node.CheckMapNode("Example");
            var example = new Example();
            foreach (var property in mapNode)
            {
                property.ParseField(example, fixedFields, patternFields);
            }

            return example;
        }

    }
}
