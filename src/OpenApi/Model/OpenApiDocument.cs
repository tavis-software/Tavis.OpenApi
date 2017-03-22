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
        public Paths Paths { get; set; } = new Paths();
        public Components Components { get; set; } = new Components();
        public List<Tag> Tags { get; set; } = new List<Tag>();
        public ExternalDocs ExternalDocs { get; set; } = new ExternalDocs();
        public Dictionary<string, string> Extensions { get; set; } = new Dictionary<string, string>();

        private static Regex versionRegex = new Regex(@"\d+\.\d+\.\d+");



        public void Write(IParseNodeWriter writer)
        {
            writer.WriteStartMap();

            writer.WritePropertyName("openapi");
            writer.WriteValue("3.0.0-RC0");

            writer.WriteObject("info", Info, Info.Write);
            writer.WriteList("servers", Servers, Server.Write);
            writer.WritePropertyName("paths");
            Paths.Write(writer);
            writer.WriteList("tags", Tags, Tag.Write);
            writer.WriteObject("components", Components, Components.Write);
            writer.WriteObject("externalDocs", ExternalDocs, ExternalDocs.Write);
            writer.WriteList("security", SecurityRequirements, SecurityRequirement.Write);

            writer.WriteEndMap();
        }
    }


}
