using SharpYaml.Serialization;
using System.Collections.Generic;

namespace Tavis.OpenApi.Model
{

 
    public class Schema
    {

        public Dictionary<string, string> Extensions { get; set; }

        private static FixedFieldMap<Schema> fixedFields = new FixedFieldMap<Schema>
        {
        };

        private static PatternFieldMap<Schema> patternFields = new PatternFieldMap<Schema>
        {
            { (s)=> s.StartsWith("x-"), (o,k,n)=> o.Extensions.Add(k, n.GetScalarValue()) }
        };


        public static Schema Load(YamlMappingNode mapNode)
        {
            var contentType = new Schema();
            foreach (var node in mapNode.Children)
            {
                var key = (YamlScalarNode)node.Key;
                ParseHelper.ParseField(key.Value, node.Value, contentType, fixedFields, patternFields);
            }

            return contentType;
        }

    }
}