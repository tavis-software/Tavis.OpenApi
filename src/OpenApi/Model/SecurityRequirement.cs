using SharpYaml.Serialization;
using System.Linq;

namespace Tavis.OpenApi.Model
{


    public class SecurityRequirement
    {
        public string Name { get; set; }
        public string[] Scopes { get; set; }
        internal static SecurityRequirement Load(YamlMappingNode n)
        {

            var obj = new SecurityRequirement();

            foreach (var node in n.Children)
            {
                var key = (YamlScalarNode)node.Key;
                switch (key.Value)
                {
                    default:
                        obj.Name = key.Value;
                        var scopeSequence = (YamlSequenceNode)node.Value;
                        obj.Scopes =  scopeSequence.Select(s => s.GetScalarValue()).ToArray<string>();
                        break;

                }
            }
            return obj;
        }
    }
}