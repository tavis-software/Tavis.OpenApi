using SharpYaml.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace Tavis.OpenApi.Model
{


    public class SecurityRequirement
    {

        public Dictionary<SecurityScheme, List<string>> Schemes { get; set; } = new Dictionary<SecurityScheme, List<string>>();
        internal static SecurityRequirement Load(ParseNode node)
        {

            var mapNode = node.CheckMapNode("security");

            var obj = new SecurityRequirement();

            foreach (var property in mapNode)
            {
                var scheme = SecurityScheme.LoadByReference(new ValueNode(mapNode.Context, property.Name));

                obj.Schemes.Add(scheme, property.Value.CreateSimpleList<string>(n2 => n2.GetScalarValue()));
            }
            return obj;
        }
    }
}