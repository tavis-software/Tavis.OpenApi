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

        void WriteOpenApi(IParseNodeWriter writer, OpenApiDocument doc)
        {

            if (doc == null) return;

            writer.WriteStartMap();
                writer.WritePropertyName("openapi");
                writer.WriteValue("3.0.0-RC0");
                WriteInfo(writer, doc.Info);
                WriteList(writer, "servers",doc.Servers, WriteServer);
                WritePaths(writer, doc.Paths);
                WriteList(writer, "tags",doc.Tags,WriteTag);
                WriteComponents(writer, doc.Components);
                WriteExternalDocs(writer, doc.ExternalDocs);
                WriteSecurity(writer, doc.SecurityRequirements);
            writer.WriteEndMap();
        }

        void WriteInfo(IParseNodeWriter writer, Info info)
        {
            if (info == null) return;

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

        void WriteContact(IParseNodeWriter writer, Contact contact)
        {
            if (contact == null) return;

            writer.WritePropertyName("contact");
            writer.WriteStartMap();

            WriteStringProperty(writer, "name", contact.Name);
            WriteStringProperty(writer, "url", contact.Url.OriginalString);
            WriteStringProperty(writer, "email", contact.Email);

            writer.WriteEndMap();

        }

        void WriteLicense(IParseNodeWriter writer, License license)
        {
            if (license == null) return;
            writer.WritePropertyName("license");
            writer.WriteStartMap();

            WriteStringProperty(writer, "name", license.Name);
            WriteStringProperty(writer, "url", license.Url.OriginalString);

            writer.WriteEndMap();

        }

        void WriteServer(IParseNodeWriter writer, Server server)
        {
            if (server == null) return;

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

        void WriteServerVariables(IParseNodeWriter writer, Server server)
        {
            writer.WriteStartMap();

            foreach (var variable in server.Variables)
            {
                writer.WritePropertyName(variable.Key);
                WriteServerVariable(writer, variable.Value);
            }

            writer.WriteEndMap();
        }

        void WriteServerVariable(IParseNodeWriter writer, ServerVariable servervariable)
        {
            writer.WriteStartMap();

            if (servervariable.Enum.Count > 0)
            {
                writer.WritePropertyName("enum");
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

        void WritePaths(IParseNodeWriter writer, Paths paths)
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

        void WritePathItem(IParseNodeWriter writer, PathItem pathItem)
        {
            writer.WriteStartMap();
            WriteStringProperty(writer,"summary",pathItem.Summary);
            WriteStringProperty(writer, "description", pathItem.Description);
            foreach (var operationPair in pathItem.Operations) 
            {
                writer.WritePropertyName(operationPair.Key);
                WriteOperation(writer, operationPair.Value);
            }
            writer.WriteEndMap();
        }

        void WriteOperation(IParseNodeWriter writer, Operation operation)
        {
            writer.WriteStartMap();
            WriteList(writer, "tags",operation.Tags, WriteTag);
            WriteStringProperty(writer, "summary", operation.Summary);
            WriteStringProperty(writer, "description", operation.Description);
            WriteExternalDocs(writer, operation.ExternalDocs);
            WriteStringProperty(writer, "operationId", operation.OperationId);

            WriteList<Parameter>(writer, "parameters",operation.Parameters, WriteParameter);

            WriteRequestBody(writer, operation.RequestBody);

            WriteMap<Response>(writer, "responses", operation.Responses, WriteResponse);

            WriteMap<Callback>(writer, "callbacks", operation.Callbacks, WriteCallback);

            if (operation.Deprecated == true)
            {
                writer.WritePropertyName("deprecated");
                writer.WriteValue(operation.Deprecated);
            }

            //operation.Security

            WriteServer(writer, operation.Server);

            writer.WriteEndMap();
        }

        void WriteParameter(IParseNodeWriter writer, Parameter parameter)
        {
            writer.WriteStartMap();
            WriteStringProperty(writer, "name", parameter.Name);
            WriteStringProperty(writer, "in", parameter.In.ToString());
            WriteStringProperty(writer, "description", parameter.Description);
            WriteBoolProperty(writer, "required", parameter.Required);
            WriteBoolProperty(writer, "deprecated", parameter.Deprecated);
            WriteBoolProperty(writer, "allowEmptyValue", parameter.AllowEmptyValue);
            WriteStringProperty(writer, "style", parameter.Style);
            WriteBoolProperty(writer, "explode", parameter.Explode);
            WriteBoolProperty(writer, "allowReserved", parameter.AllowReserved);
            WriteSchema(writer, parameter.Schema);
            WriteExamples(writer, parameter.Examples);
            WriteExample(writer, parameter.Example);
            WriteContent(writer, parameter.Content);
            writer.WriteEndMap();
        }

        void WriteContent(IParseNodeWriter writer, Content content)
        {
            if (content == null) return;
            WriteMap<MediaType>(writer, "content", content.ContentTypes, WriteMediaType);
        }

        void WriteMediaType(IParseNodeWriter writer, MediaType mediaType)
        {

            writer.WriteStartMap();

            WriteSchema(writer, mediaType.Schema);
            WriteExample(writer, mediaType.Example);
            WriteExamples(writer, mediaType.Examples);

            writer.WriteEndMap();

        }

        void WriteExample(IParseNodeWriter writer, AnyNode example)
        {
            if (example != null)
            {
                writer.WritePropertyName("example");
                writer.WriteStartMap();

                writer.WriteEndMap();
            }
        }

        void WriteExamples(IParseNodeWriter writer, List<AnyNode> examples)
        {
            if (examples != null)
            {
                writer.WritePropertyName("examples");
                writer.WriteStartList();
                foreach (var example in examples)
                {
                    
                }
                writer.WriteEndList();
            }
        }

        void WriteSchema(IParseNodeWriter writer, Schema schema)
        {
            if (schema != null)
            {
                writer.WritePropertyName("schema");
                writer.WriteStartMap();

                WriteStringProperty(writer, "type", schema.Type);
                WriteStringProperty(writer, "format", schema.Format);
                WriteStringProperty(writer, "description", schema.Description);

                writer.WriteEndMap();
            }
        }

        void WriteRequestBody(IParseNodeWriter writer, RequestBody requestBody)
        {
            if (requestBody != null)
            {
                writer.WritePropertyName("requestBody");
                writer.WriteStartMap();
                WriteStringProperty(writer, "description", requestBody.Description);
                WriteBoolProperty(writer, "required", requestBody.Required);
                WriteContent(writer, requestBody.Content);
                writer.WriteEndMap();
            }
        }

        void WriteResponse(IParseNodeWriter writer, Response response)
        {
            writer.WriteStartMap();

            WriteStringProperty(writer, "description", response.Description);
            WriteContent(writer, response.Content);

            WriteMap(writer, "headers",response.Headers, WriteHeader);
            WriteMap(writer, "links",response.Links, WriteLink);

            //Links
            writer.WriteEndMap();
        }
        
        void WriteCallback(IParseNodeWriter writer, Callback callback)
        {
            writer.WriteStartMap();
            writer.WriteEndMap();
        }

        void WriteHeader(IParseNodeWriter writer, Header header)
        {
            writer.WriteStartMap();

            writer.WriteEndMap();
        }

        void WriteLink(IParseNodeWriter writer, Link link)
        {
            writer.WriteStartMap();
            WriteStringProperty(writer, "href", link.Href);
            WriteStringProperty(writer, "operationId", link.OperationId);
            WriteMap(writer, "parameters", link.Parameters, (w,x)=> { w.WriteValue(x.ToString()); });

            writer.WriteEndMap();
        }

        private void WriteTag(IParseNodeWriter writer, Tag tag)
        {
            writer.WriteStartMap();
            WriteStringProperty(writer, "name", tag.Name);
            WriteStringProperty(writer, "description", tag.Description);
            writer.WriteEndMap();
        }

        void WriteComponents(IParseNodeWriter writer, Components components)
        {
            if (!components.IsEmpty())
            {
                writer.WritePropertyName("components");
                writer.WriteStartMap();
                WriteMap(writer, "schemas",components.Schemas,WriteSchema);
                WriteMap(writer, "parameters", components.Parameters, WriteParameter);

                
                writer.WriteEndMap();
            }
        }

        void WriteExternalDocs(IParseNodeWriter writer, ExternalDocs externalDocs)
        {

        }

        void WriteSecurity(IParseNodeWriter writer, List<SecurityRequirement> securityRequirements)
        {
            if (securityRequirements != null && securityRequirements.Count > 0)
            {
                writer.WritePropertyName("security");
                writer.WriteStartList();

                writer.WriteEndList();
            }

        }


        public static void WriteList<T>(IParseNodeWriter writer, string propertyName, IList<T> list, Action<IParseNodeWriter, T> parser)
        {
            if (list != null && list.Count() > 0)
            {
                writer.WritePropertyName(propertyName);
                foreach (var item in list)
                {
                    writer.WriteStartList();
                    parser(writer, item);
                    writer.WriteEndList();
                }
            }

        }

        public static void WriteMap<T>(IParseNodeWriter writer, string propertyName, IDictionary<string, T> list, Action<IParseNodeWriter, T> parser)
        {
            if (list != null && list.Count() > 0)
            {
                writer.WritePropertyName(propertyName);
                foreach (var item in list)
                {
                    writer.WriteStartMap();
                    writer.WritePropertyName(item.Key);

                    parser(writer, item.Value);
                    writer.WriteEndMap();
                }
            }

        }


        public static void WriteStringProperty(IParseNodeWriter writer, string name, string value)
        {
            if (!String.IsNullOrEmpty(value))
            {
                writer.WritePropertyName(name);
                writer.WriteValue(value);
            }
        }
        public static void WriteBoolProperty(IParseNodeWriter writer, string name, bool value)
        {
                writer.WritePropertyName(name);
                writer.WriteValue(value);
        }

        public static void WriteNumberProperty(IParseNodeWriter writer, string name, decimal value)
        {
            writer.WritePropertyName(name);
            writer.WriteValue(value);
        }

        public static void WriteNumberProperty(IParseNodeWriter writer, string name, int value)
        {
            writer.WritePropertyName(name);
            writer.WriteValue(value);
        }
    }
}
