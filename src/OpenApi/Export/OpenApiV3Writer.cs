using SharpYaml.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tavis.OpenApi.Export;
using Tavis.OpenApi.Model;

namespace Tavis.OpenApi
{
    public class OpenApiV3Writer
    {
        OpenApiDocument document;
        public OpenApiV3Writer(OpenApiDocument document)
        {
            this.document = document;
        }

        public void Writer(Stream stream)
        {
            //var serializer = new Serializer();

            //var writer = new StreamWriter(stream);

            //writer.Write(serializer.Serialize(this.document));

            var writer = new YamlParseNodeWriter(stream);
            writer.WriteStartDocument();
            WriteOpenApi(writer, this.document);
            writer.WriteEndDocument();
            writer.Flush();
        }

        private void WriteOpenApi(IParseNodeWriter writer, OpenApiDocument doc)
        {
            
            writer.WriteStartMap();
                writer.WritePropertyName("openapi");
                writer.WriteValue("3.0.0");
                WriteInfo(writer, doc.Info);
                WriteServers(writer, doc.Servers);
                WritePaths(writer, doc.Paths);
                WriteTags(writer, doc.Tags);
                WriteComponents(writer, doc.Components);
                WriteExternalDocs(writer, doc.ExternalDocs);
                WriteSecurity(writer, doc.SecurityRequirements);
            writer.WriteEndMap();

        }


        private void WriteInfo(IParseNodeWriter writer, Info info)
        {
            writer.WritePropertyName("info");
            writer.WriteStartMap();

            WriteStringProperty(writer,"title", info.Title);
            WriteStringProperty(writer,"description",info.Description);
            WriteStringProperty(writer,"termsOfService", info.TermsOfService);
            WriteStringProperty(writer, "version", info.Version);

            writer.WriteEndMap();

        }

        private void WriteStringProperty(IParseNodeWriter writer, string name, string value) {
            if (!String.IsNullOrEmpty(value))
            {
                writer.WritePropertyName(name);
                writer.WriteValue(value);
            }
        }

        private void WritePaths(IParseNodeWriter writer, Paths paths)
        {
            writer.WritePropertyName("paths");
            writer.WriteStartMap();

            writer.WriteEndMap();
        }

        private void WriteServers(IParseNodeWriter writer, List<Server> servers)
        {
            if (servers.Count > 0)
            {
                writer.WritePropertyName("servers");
                writer.WriteStartList();

                writer.WriteEndList();
            }

        }

        private void WriteTags(IParseNodeWriter writer, List<Tag> tags)
        {
            if (tags.Count > 0)
            {
                writer.WritePropertyName("tags");
                writer.WriteStartList();

                writer.WriteEndList();
            }

        }

        private void WriteComponents(IParseNodeWriter writer, Components components)
        {
            if (!components.IsEmpty())
            {
                writer.WritePropertyName("components");
                writer.WriteStartMap();

                writer.WriteEndMap();
            }

        }

        private void WriteExternalDocs(IParseNodeWriter writer, ExternalDocs externalDocs)
        {

        }

        private void WriteSecurity(IParseNodeWriter writer, List<SecurityRequirement> securityRequirements)
        {
            if (securityRequirements != null && securityRequirements.Count > 0)
            {
                writer.WritePropertyName("security");
                writer.WriteStartList();

                writer.WriteEndList();
            }

        }
    }
}
