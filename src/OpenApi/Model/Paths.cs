using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpYaml.Serialization;

namespace Tavis.OpenApi.Model
{
    public class Paths
    {
        public IDictionary<string, PathItem> PathItems { get; set; }
        public Dictionary<string,string> Extensions { get; set; }


        private static FixedFieldMap<Paths> fixedFields = new FixedFieldMap<Paths> {
            //{ "name", (o,n) => { o.Name = n.GetScalarValue(); } },
            //{ "email", (o,n) => { o.Email = n.GetScalarValue(); } },
            //{ "url", (o,n) => { o.Url = new Uri(n.GetScalarValue()); } },
            // Parameters
            // host
            // scheme
            // basepath
            };

        private static PatternFieldMap<Paths> patternFields = new PatternFieldMap<Paths>
        {
            { (s)=> s.StartsWith("/"), (o,k,n)=> o.PathItems.Add(k, PathItem.Load(n)    ) },
            { (s)=> s.StartsWith("x-"), (o,k,n)=> o.Extensions.Add(k, n.GetScalarValue()) }
        };


        public Paths()
        {
            PathItems = new Dictionary<string, PathItem>();  
        }

        public PathItem GetPath(string key)
        {
            return PathItems[key];
        }
        internal static Paths Load(ParseNode node)
        {
            MapNode mapNode = node.CheckMapNode("Paths");

            //Schema domainObject = CreateOrReferenceDomainObject(mapNode, () => new Paths());

            Paths domainObject = new Paths();
            foreach (var propertyNode in mapNode)
            {
                propertyNode.ParseField(domainObject, fixedFields, patternFields);
            }

            return domainObject;
        }

        internal void Validate(List<string> errors)
        {
            //TODO:
        }
    }
}
