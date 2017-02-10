using SharpYaml.Serialization;
using System;

namespace Tavis.OpenApi.Model
{

    public class ExternalDocs
    {
        public string Description { get; set; }
        public Uri Url { get; set; }

        public static ExternalDocs Load(ParseNode node)
        {
            var mapNode = node.CheckMapNode("externalDocs");

            var obj = new ExternalDocs();

            foreach (var property in mapNode)
            {
                switch (property.Name)
                {
                    case "description":
                        obj.Description= property.Value.GetScalarValue();
                        break;
                    case "url":
                        obj.Url = new Uri(property.Value.GetScalarValue());
                        break;

                }
            }
            return obj;
        }
    }
}