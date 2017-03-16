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
            WriteContact(writer, info.Contact);
            WriteLicense(writer, info.License);
            WriteStringProperty(writer, "version", info.Version);

            writer.WriteEndMap();

        }

        private void WriteContact(IParseNodeWriter writer, Contact contact)
        {
            writer.WritePropertyName("contact");
            writer.WriteStartMap();

            WriteStringProperty(writer, "name", contact.Name);
            WriteStringProperty(writer, "url", contact.Url.OriginalString);
            WriteStringProperty(writer, "email", contact.Email);

            writer.WriteEndMap();

        }

        private void WriteLicense(IParseNodeWriter writer, License license)
        {
            writer.WritePropertyName("license");
            writer.WriteStartMap();

            WriteStringProperty(writer, "name", license.Name);
            WriteStringProperty(writer, "url", license.Url.OriginalString);

            writer.WriteEndMap();

        }

        private void WritePaths(IParseNodeWriter writer, Paths paths)
        {
            writer.WritePropertyName("paths");
            writer.WriteStartMap();
            foreach (var pathItem in paths.PathItems)
            {
                writer.WritePropertyName(pathItem.Key);
                WritePathItem(writer, pathItem.Value);
            }
            writer.WriteEndMap();
        }

        private void WritePathItem(IParseNodeWriter writer, PathItem pathItem)
        {
            writer.WriteStartMap();
            WriteStringProperty(writer,"summary",pathItem.Summary);
            WriteStringProperty(writer, "description", pathItem.Description);
            foreach (var operation in pathItem.Operations) 
            {

            }
            writer.WriteEndMap();
        }

        private void WriteServers(IParseNodeWriter writer, List<Server> servers)
        {
            if (servers.Count > 0)
            {
                writer.WritePropertyName("servers");
                writer.WriteStartList();
                foreach (var server in servers)
                {
                    WriteServer(writer, server);
                }
                
                writer.WriteEndList();
            }

        }

        private void WriteServer(IParseNodeWriter writer, Server server)
        {
            writer.WriteStartMap();

            WriteStringProperty(writer, "url", server.Url);
            WriteStringProperty(writer, "description", server.Description);

            if (server.Variables.Count > 0)
            {
                writer.WritePropertyName("variables");
                WriteServerVariables(writer, server);
            }
            writer.WriteEndMap();

        }

        private void WriteServerVariables(IParseNodeWriter writer, Server server)
        {
                writer.WriteStartMap();

                foreach (var variable in server.Variables)
                {
                    writer.WritePropertyName(variable.Key);
                    WriteServerVariable(writer, variable.Value);
                }

                writer.WriteEndMap();
        }

        private void WriteServerVariable(IParseNodeWriter writer, ServerVariable servervariable)
        {
            writer.WriteStartMap();

            if ( servervariable.Enum.Count > 0)
            {
                writer.WriteStartList();
                foreach (var enumItem in servervariable.Enum)
                {
                    writer.WriteValue(enumItem);
                }
                writer.WriteEndList();
            }
            WriteStringProperty(writer, "default", servervariable.Default);
            WriteStringProperty(writer, "description", servervariable.Description);

            writer.WriteEndMap();

        }

        void WriteTags(IParseNodeWriter writer, List<Tag> tags)
        {
            if (tags.Count > 0)
            {
                writer.WritePropertyName("tags");
                writer.WriteStartList();
                foreach (var tag in tags)
                {
                    writer.WriteStartMap();
                    WriteStringProperty(writer, "name", tag.Name);
                    WriteStringProperty(writer, "description", tag.Description);
                    writer.WriteEndMap();

                }
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


        private void WriteStringProperty(IParseNodeWriter writer, string name, string value)
        {
            if (!String.IsNullOrEmpty(value))
            {
                writer.WritePropertyName(name);
                writer.WriteValue(value);
            }
        }

    }
}
