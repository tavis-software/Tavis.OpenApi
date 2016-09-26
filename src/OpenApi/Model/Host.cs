using SharpYaml.Serialization;
using System.Collections.Generic;

namespace Tavis.OpenApi.Model
{

    public class Host
    {
        public string HostName { get; set; }
        public string BasePath { get; set; }
        public string Scheme { get; set; }
        public Dictionary<string, string> Extensions { get; set; }


        public Host()
        {
            Extensions = new Dictionary<string, string>();
        }

        internal static Host Load(YamlMappingNode infoNode)
        {
            var host = new Host();

            foreach (var node in infoNode.Children)
            {
                var key = ((YamlScalarNode)node.Key).Value;
                switch (key)
                {
                    case "host":
                        host.HostName = node.Value.GetScalarValue();
                        break;

                    case "basePath":
                        host.BasePath = node.Value.GetScalarValue();
                        break;

                    case "scheme":
                        host.Scheme = node.Value.GetScalarValue();
                        break;

                    default:
                        if (key.StartsWith("x-"))
                        {
                            host.Extensions.Add(key, node.Value.GetScalarValue());
                        }
                        break;
                }
            }

            return host;
        }
    }

}