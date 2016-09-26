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
        public IDictionary<string, Path> PathMap { get; set; }
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
            { (s)=> s.StartsWith("/"), (o,k,n)=> o.PathMap.Add(k, Path.Load((YamlMappingNode)n)) },
            { (s)=> s.StartsWith("x-"), (o,k,n)=> o.Extensions.Add(k, n.GetScalarValue()) }
        };


        public Paths()
        {
            PathMap = new Dictionary<string, Path>();  
        }
        internal static Paths Load(YamlMappingNode value)
        {
            var paths = new Paths();

            foreach(var node in value.Children)
            {
                var key = (YamlScalarNode)node.Key;
                ParseHelper.ParseField(key.Value, node.Value, paths, fixedFields, patternFields);
            }

            return paths;
        }
    }
}
