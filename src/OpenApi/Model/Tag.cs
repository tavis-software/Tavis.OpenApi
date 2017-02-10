using SharpYaml.Serialization;

namespace Tavis.OpenApi.Model
{
    public class Tag
    {
        public string Name { get; set; }
        public string Description { get; set; }

        internal static Tag Load(ParseNode n)
        {
            var mapNode = n.CheckMapNode("tag");

            var obj = new Tag();

            foreach (var node in mapNode)
            {
                var key = node.Name;
                switch (key)
                {
                    case "description":
                        obj.Description = node.Value.GetScalarValue();
                        break;
                    case "name":
                        obj.Name = node.Value.GetScalarValue();
                        break;

                }
            }
            return obj;
        }
    }
}