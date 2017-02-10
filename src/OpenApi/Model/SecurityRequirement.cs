using SharpYaml.Serialization;
using System.Linq;

namespace Tavis.OpenApi.Model
{


    public class SecurityRequirement
    {
        public string Name { get; set; }
        public string[] Scopes { get; set; }
        internal static SecurityRequirement Load(ParseNode node)
        {

            var mapNode = node.CheckMapNode("security");

            var obj = new SecurityRequirement();

            foreach (var property in mapNode)
            {
                switch (property.Name)  // What's this for?
                {
                    default:
                        obj.Name = property.Name ;
                        var scopeSequence = (ListNode)property.Value;
                        obj.Scopes =  scopeSequence.Select(s => s.GetScalarValue()).ToArray<string>();
                        break;

                }
            }
            return obj;
        }
    }
}