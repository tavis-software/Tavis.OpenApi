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

        internal static License Load(ParseNode node)
        {
            var mapNode = node.CheckMapNode("License");
            var license = new License();

            foreach (var property in mapNode)
            {
                switch (property.Name)
                {
                    case "name":
                        license.Name = property.Value.GetScalarValue();
                        break;

                    case "url":
                        license.Url = new Uri(property.Value.GetScalarValue());
                        break;

                    default:
                        if (property.Name.StartsWith("x-"))
                        {
                            license.Extensions.Add(property.Name, property.Value.GetScalarValue());
                        }
                        break;
                }
            }

            return license;
        }
    }
}