using SharpYaml.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tavis.OpenApi.Model
{
    public class ServerTemplate
    {
        public string Description { get; set; }
        public string Default { get; set; }
        public List<string> Enum { get; set; }

        public Dictionary<string, string> Extensions { get; set; } = new Dictionary<string, string>();

        private static FixedFieldMap<ServerTemplate> fixedFields = new FixedFieldMap<ServerTemplate>
        {
            { "description", (o,n) => { o.Description = n.GetScalarValue();  } },
            { "default", (o,n) => { o.Default =  n.GetScalarValue(); } },
            { "enum", (o,n) => { o.Enum =  n.CreateList<string>((s)=> s.GetScalarValue()); } }
        };

        private static PatternFieldMap<ServerTemplate> patternFields = new PatternFieldMap<ServerTemplate>
        {
            { (s)=> s.StartsWith("x-"), (o,k,n)=> o.Extensions.Add(k, n.GetScalarValue()) }
        };

        public static ServerTemplate Load(YamlMappingNode mapNode)
        {
            var serverTemplate = new ServerTemplate();
            foreach (var node in mapNode.Children)
            {
                var key = (YamlScalarNode)node.Key;
                ParseHelper.ParseField(key.Value, node.Value, serverTemplate, fixedFields, patternFields);
            }

            return serverTemplate;
        }
    }
}
