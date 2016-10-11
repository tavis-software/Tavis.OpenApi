using SharpYaml.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tavis.OpenApi.Model
{
    public class RequestBody
    {
        public string Description { get; set; }
        public Content Content { get; set; }
        public Boolean Required { get; set; }
        public Dictionary<string,string> Extensions { get; set; }


        private static FixedFieldMap<RequestBody> fixedFields = new FixedFieldMap<RequestBody>
        {
            { "description", (o,n) => { o.Description = n.GetScalarValue(); } },
            { "required", (o,n) => { o.Required = bool.Parse(n.GetScalarValue()); } },
            { "content", (o,n) => { o.Content = Content.Load((YamlMappingNode)n);  } },
        };

        private static PatternFieldMap<RequestBody> patternFields = new PatternFieldMap<RequestBody>
        {
            { (s)=> s.StartsWith("x-"), (o,k,n)=> o.Extensions.Add(k, n.GetScalarValue()) },
        };

        public static RequestBody Load(YamlMappingNode mapNode)
        {
            var requestBody = new RequestBody();
            foreach (var node in mapNode.Children)
            {
                var key = (YamlScalarNode)node.Key;
                ParseHelper.ParseField(key.Value, node.Value, requestBody, fixedFields, patternFields);
            }

            return requestBody;
        }
    }
}
