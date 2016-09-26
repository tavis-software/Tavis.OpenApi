using SharpYaml.Serialization;
using System;

namespace Tavis.OpenApi.Model
{

    public class ExternalDocs
    {
        public string Description { get; set; }
        public Uri Url { get; set; }

        public static ExternalDocs Load(YamlMappingNode n)
        {
            var obj = new ExternalDocs();

            foreach (var node in n.Children)
            {
                var key = (YamlScalarNode)node.Key;
                switch (key.Value)
                {
                    case "description":
                        obj.Description= node.Value.GetScalarValue();
                        break;
                    case "url":
                        obj.Url = new Uri(node.Value.GetScalarValue());
                        break;

                }
            }
            return obj;
        }
    }
}