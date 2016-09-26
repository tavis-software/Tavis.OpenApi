using System;
using System.Collections.Generic;
using SharpYaml.Serialization;

namespace Tavis.OpenApi.Model
{


    public class License
    {
        public string Name { get; set; }
        public Uri Url { get; set; }
        public Dictionary<string, string> Extensions { get; set; }


        public License()
        {
            Extensions = new Dictionary<string, string>();
        }

        internal static License Load(YamlMappingNode licenseNode)
        {
            var license = new License();

            foreach (var node in licenseNode.Children)
            {
                var key = ((YamlScalarNode)node.Key).Value;
                switch (key)
                {
                    case "name":
                        license.Name = node.Value.GetScalarValue();
                        break;

                    case "url":
                        license.Url = new Uri(node.Value.GetScalarValue());
                        break;

                    default:
                        if (key.StartsWith("x-"))
                        {
                            license.Extensions.Add(key, node.Value.GetScalarValue());
                        }
                        break;
                }
            }

            return license;
        }
    }
}