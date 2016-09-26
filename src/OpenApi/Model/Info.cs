using System.Collections.Generic;
using SharpYaml.Serialization;
using System;
using System.Linq;

namespace Tavis.OpenApi.Model
{




    public class Info
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string TermsOfService { get; set; }
        public Contact Contact { get; set; }
        public License License { get; set; }
        public string Version { get; set; }
        public Dictionary<string, string> Extensions { get; set; }

        private static Dictionary<string, Action<Info, YamlNode>> fixedFields
             = new Dictionary<string, Action<Info, YamlNode>> {
            { "title", (o,n) => { o.Title = n.GetScalarValue(); } },
            { "version", (o,n) => { o.Version = n.GetScalarValue(); } },
            { "description", (o,n) => { o.Description = n.GetScalarValue(); } }, 
            { "termsOfService", (o,n) => { o.TermsOfService = n.GetScalarValue(); } },
            { "contact", (o,n) => { o.Contact = Contact.Load((YamlMappingNode)n); } },
            { "license", (o,n) => { o.License = License.Load((YamlMappingNode)n); } }
        };

        private static Dictionary<Func<string,bool>, Action<Info, string, YamlNode>> patternFields
           = new Dictionary<Func<string, bool>, Action<Info, string, YamlNode>>
           {
               { (s)=> s.StartsWith("x-"), (o,k,n)=> o.Extensions.Add(k, n.GetScalarValue()) }
           };


        public Info()
        {
            Extensions = new Dictionary<string, string>();
        }

        internal static Info Load(YamlMappingNode infoNode)
        {
            var info = new Info();

            foreach(var node in infoNode.Children)
            {
                var key = (YamlScalarNode)node.Key;
                ParseHelper.ParseField(key.Value, node.Value, info, fixedFields, patternFields);
            }

            return info;
        }

        public void Validate(List<string> errors)
        {
            // Title
            if (String.IsNullOrEmpty(Title))
            {
                errors.Add("`info.title` is a required property");
            }
        }

    }


}
