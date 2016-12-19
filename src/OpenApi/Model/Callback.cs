using SharpYaml.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tavis.OpenApi.Model
{
    public class Callback
    {
        public Dictionary<string, Operation> Operations { get; set; } = new Dictionary<string, Operation>();

        public Dictionary<string, string> Extensions { get; set; }

        private static FixedFieldMap<Callback> fixedFields = new FixedFieldMap<Callback>
        {
        };

        private static PatternFieldMap<Callback> patternFields = new PatternFieldMap<Callback>
        {
            { (s)=> s.StartsWith("x-"), (o,k,n)=> o.Extensions.Add(k, n.GetScalarValue()) },
            { (s)=> "get,put,post,delete,patch,options,head".Contains(s),
                (o,k,n)=> o.Operations.Add(k, Operation.Load((YamlMappingNode)n)) }

        };


        public static Callback Load(YamlMappingNode mapNode)
        {
            var callback = new Callback();
            foreach (var node in mapNode.Children)
            {
                var key = (YamlScalarNode)node.Key;
                ParseHelper.ParseField(key.Value, node.Value, callback, fixedFields, patternFields);
            }

            return callback;
        }
    }
}
