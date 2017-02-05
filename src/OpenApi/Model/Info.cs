using System.Collections.Generic;
using SharpYaml.Serialization;
using System;
using System.Linq;
using System.Text.RegularExpressions;

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

        private static FixedFieldMap<Info> fixedFields
             = new FixedFieldMap<Info> {
            { "title", (o,n) => { o.Title = n.GetScalarValue(); } },
            { "version", (o,n) => { o.Version = n.GetScalarValue(); } },
            { "description", (o,n) => { o.Description = n.GetScalarValue(); } },
            { "termsOfService", (o,n) => { o.TermsOfService = n.GetScalarValue(); } },
            { "contact", (o,n) => { o.Contact = Contact.Load(n); } },
            { "license", (o,n) => { o.License = License.Load(n); } }
        };

        private static PatternFieldMap<Info> patternFields
           = new PatternFieldMap<Info>
           {
               { (s)=> s.StartsWith("x-"), (o,k,n)=> o.Extensions.Add(k, n.GetScalarValue()) }
           };


        public Info()
        {
            Extensions = new Dictionary<string, string>();
        }

        internal static Info Load(ParseNode node)
        {
            var infoNode = node as MapNode;
            if (infoNode != null)
            {
                throw new Exception("Info node must be a Map"); // Add to errors and return
            } 
            var info = new Info();

            foreach (var propertyNode in infoNode)
            {
                propertyNode.ParseField(info, fixedFields, patternFields);
            }

            return info;
        }

        private static Regex versionRegex = new Regex(@"\d+\.\d+\.\d+");

        public static Dictionary<Func<Info, bool>, string> ValidationRules = new Dictionary<Func<Info, bool>, string> { 
            { i => String.IsNullOrEmpty(i.Title), "`info.title` is a required property"   },
            { i => String.IsNullOrEmpty(i.Version), "`info.version` is a required property"   },
            { i => !versionRegex.IsMatch(i.Version), "`info.version` property does not match the required format major.minor.patch"},
            { i => !String.IsNullOrEmpty(i.TermsOfService) && !CheckUrl(i.TermsOfService), "`info.termsOfService` MUST be a URL"   }
        };

        private static bool CheckUrl(string href)
        {
            Uri output = null;
            return Uri.TryCreate(href, UriKind.RelativeOrAbsolute, out output);
        }

        public void Validate(List<string> errors)
        {
            errors.AddRange(ValidationRules.Where(kvp => kvp.Key(this)).Select(kvp => kvp.Value));
        }

    }
        
    }
