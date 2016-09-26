using SharpYaml.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Tavis.OpenApi.Model
{

    
    public class OpenApiDocument
    {
        public string Version { get; set; } // Swagger

        public Info Info { get; set; }

        public List<Host> Hosts { get; set; }
        public List<SecurityRequirement> SecurityRequirements { get; set; }

        public Paths Paths { get; set; }
        public Components Components { get; set; }
        public List<Tag> Tags { get; set; }
        public ExternalDocs ExternalDocs { get; set; }
        public Dictionary<string, string> Extensions { get; set; }

        public List<string> ParseErrors { get; set; } = new List<string>();

        static OpenApiDocument()
        {

        }

        private static FixedFieldMap<OpenApiDocument> fixedFields = new FixedFieldMap<OpenApiDocument> {
            { "openapi", (o,n) => { o.Version = n.GetScalarValue(); } },
            { "info", (o,n) => o.Info = Info.Load((YamlMappingNode)n) },
            { "paths", (o,n) => o.Paths = Paths.Load((YamlMappingNode)n) },
            { "components", (o,n) => o.Components = Components.Load((YamlMappingNode)n) },
            { "tags", (o,n) => o.Tags = n.CreateList(Tag.Load)},
            { "externalDocs", (o,n) => o.ExternalDocs = ExternalDocs.Load((YamlMappingNode)n) },
            { "security", (o,n) => o.SecurityRequirements = n.CreateList(SecurityRequirement.Load)}
                // Callbacks
                // Links
            };

        private static PatternFieldMap<OpenApiDocument> patternFields = new PatternFieldMap<OpenApiDocument>
        {
                   { (s)=> s.StartsWith("x-"), (o,k,n)=> o.Extensions.Add(k, n.GetScalarValue()) }
        };

        public OpenApiDocument()
        {
            Info = new Info();
            Hosts = new List<Host>();
            Tags = new List<Tag>();
        }

        public static OpenApiDocument Parse(Stream stream)
        {
            var yamlStream = new YamlStream();
            yamlStream.Load(new StreamReader(stream));
            var doc = yamlStream.Documents.First();
            return Load(doc);
        }

        public static OpenApiDocument Load(YamlDocument doc)
        {
            var openApidoc = new OpenApiDocument();

            var rootMap = (YamlMappingNode)doc.RootNode;
            
            foreach (var node in rootMap.Children)
            {
                var key = (YamlScalarNode)node.Key;
                ParseHelper.ParseField<OpenApiDocument>(key.Value, node.Value, openApidoc, OpenApiDocument.fixedFields, OpenApiDocument.patternFields);
            }

            openApidoc.ParseErrors = openApidoc.Validate();
            return openApidoc;
        }

        public List<string> Validate()
        {
            var errors = new List<string>();

            Validate(errors);
            return errors;
        }

        private Regex versionRegex = new Regex(@"\d+\.\d+\.\d+");

        private void Validate(List<string> errors)
        {
            if (!versionRegex.IsMatch(Version))
            {
                errors.Add("`openapi` property does not match the required format major.minor.patch");
            }

            Info.Validate(errors);

            if (Paths == null)
            {
                errors.Add("`paths` is a required property");
            }
        }
    }

    
    public class FixedFieldMap<T> : Dictionary<string, Action<T, YamlNode>>
    {
        //public new Dictionary<Func<string, bool>, Action<T, string, YamlNode>> Add(Func<string, bool> key, Action<T, string, YamlNode> value)
        //{
        //    base.Add(key, value);
        //    return this;
        //}

    }

    public class PatternFieldMap<T> : Dictionary<Func<string, bool>, Action<T, string, YamlNode>>
    {
        //public new Dictionary<Func<string, bool>, Action<T, string, YamlNode>> Add(Func<string, bool> key, Action<T, string, YamlNode> value)
        //{
        //    base.Add(key, value);
        //    return this;
        //}
    }

    public static class ParseHelper
    {
        

        public static void ParseField<T>(string key,
                            YamlNode currentNode,
                            T parentInstance,
                            IDictionary<string, Action<T, YamlNode>> fixedFields,
                            IDictionary<Func<string, bool>, Action<T, string, YamlNode>> patternFields
            )
        {
            
            Action<T, YamlNode> fixedFieldMap;
            var found = fixedFields.TryGetValue(key, out fixedFieldMap);

            if (fixedFieldMap != null)
            {
                fixedFieldMap(parentInstance, currentNode);
            }
            else
            {
                var map = patternFields.Where(p => p.Key(key)).Select(p => p.Value).FirstOrDefault();
                if (map != null)
                {
                    map(parentInstance, key, currentNode);
                }
            }
        }
    }
}
