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

        public static FixedFieldMap<OpenApiDocument> fixedFields = new FixedFieldMap<OpenApiDocument> {
            { "openapi", (o,n) => { o.Version = n.GetScalarValue(); } },
            { "info", (o,n) => o.Info = Info.Load(n) },
            { "servers", (o,n) => o.Servers = n.CreateList<Server>(Server.Load) },
            { "paths", (o,n) => o.Paths = Paths.Load(n) },
            { "components", (o,n) => o.Components = Components.Load(n) },
            { "tags", (o,n) => o.Tags = n.CreateList(Tag.Load)},
            { "externalDocs", (o,n) => o.ExternalDocs = ExternalDocs.Load(n) },
            { "security", (o,n) => o.SecurityRequirements = n.CreateList(SecurityRequirement.Load)}

            };

        public static PatternFieldMap<OpenApiDocument> patternFields = new PatternFieldMap<OpenApiDocument>
        {
                    // We have no semantics to verify X- nodes, therefore treat them as just values.
                   { (s)=> s.StartsWith("x-"), (o,k,n)=> o.Extensions.Add(k, ((ValueNode)n).GetScalarValue()) }
        };


        private static Regex versionRegex = new Regex(@"\d+\.\d+\.\d+");

        public static OpenApiDocument Load(RootNode rootNode)
        {
            var openApidoc = new OpenApiDocument();

            var rootMap = rootNode.GetMap();

            // Parse Version first so that future parsing can be dependent on it
            var versionNode = rootMap["version"];
            versionNode.ParseField(openApidoc, fixedFields, patternFields);

            rootNode.Context.Version = openApidoc.Version;

            bool haspaths = false;
            foreach (var node in rootMap)
            {
                node.ParseField(openApidoc, fixedFields, patternFields);
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
