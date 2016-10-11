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

        public static Example Load(YamlMappingNode mapNode)
        {
            var example = new Example();
            foreach (var node in mapNode.Children)
            {
                var key = (YamlScalarNode)node.Key;
                ParseHelper.ParseField(key.Value, node.Value, example, fixedFields, patternFields);
            }

            return example;
        }

    }
}
