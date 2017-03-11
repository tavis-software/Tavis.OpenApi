using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections;

namespace Tavis.OpenApi.Model
{


    public class OpenApiDocument
    {
        string version;
        public string Version { get { return version; }
            set {
                if (versionRegex.IsMatch(value))
                {
                    version = value;
                } else
                {
                    throw new DomainParseException("`openapi` property does not match the required format major.minor.patch");
                }
            } } // Swagger
        public Info Info { get; set; } = new Info();
        public List<Server> Servers { get; set; } = new List<Server>();
        public List<SecurityRequirement> SecurityRequirements { get; set; }
        public Paths Paths { get; set; }
        public Components Components { get; set; } = new Components();
        public List<Tag> Tags { get; set; } = new List<Tag>();
        public ExternalDocs ExternalDocs { get; set; } = new ExternalDocs();
        public Dictionary<string, string> Extensions { get; set; } = new Dictionary<string, string>();


        private static Regex versionRegex = new Regex(@"\d+\.\d+\.\d+");


        public static OpenApiDocument Load(RootNode rootNode)
        {
            var openApidoc = new OpenApiDocument();

            var rootMap = rootNode.GetMap();
            bool haspaths = false;
            foreach (var node in rootMap)
            {
                node.ParseField<OpenApiDocument>(openApidoc, OpenApiParser.OpenApiFixedFields, OpenApiParser.OpenApiPatternFields);

                if (node.Name == "paths")
                {
                    haspaths = true;
                }
            }

            if (!haspaths)
            {
                rootMap.Context.ParseErrors.Add(new OpenApiError("", "`paths` is a required property"));
            }

            return openApidoc;
        }

    }


}
