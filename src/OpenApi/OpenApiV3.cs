using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tavis.OpenApi.Model;

namespace Tavis.OpenApi
{
    public static class OpenApiV3
    {

        #region OpenApiObject
        public static FixedFieldMap<OpenApiDocument> OpenApiFixedFields = new FixedFieldMap<OpenApiDocument> {
            { "openapi", (o,n) => { o.Version = n.GetScalarValue(); } },
            { "info", (o,n) => o.Info = OpenApiV3.LoadInfo(n) },
            { "servers", (o,n) => o.Servers = n.CreateList<Server>(Server.Load) },
            { "paths", (o,n) => o.Paths = LoadPaths(n) },
            { "components", (o,n) => o.Components = OpenApiV3.LoadComponents(n) },
            { "tags", (o,n) => o.Tags = n.CreateList(Tag.Load)},
            { "externalDocs", (o,n) => o.ExternalDocs = ExternalDocs.Load(n) },
            { "security", (o,n) => o.SecurityRequirements = n.CreateList(SecurityRequirement.Load)}

            };

        public static PatternFieldMap<OpenApiDocument> OpenApiPatternFields = new PatternFieldMap<OpenApiDocument>
        {
            // We have no semantics to verify X- nodes, therefore treat them as just values.
            { (s)=> s.StartsWith("x-"), (o,k,n)=> o.Extensions.Add(k, ((ValueNode)n).GetScalarValue()) }
        };

        public static OpenApiDocument LoadOpenApi(RootNode rootNode)
        {
            var openApidoc = new OpenApiDocument();

            var rootMap = rootNode.GetMap();

            bool haspaths = false;
            foreach (var node in rootMap)
            {
                node.ParseField(openApidoc, OpenApiV3.OpenApiFixedFields, OpenApiV3.OpenApiPatternFields);
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

        #endregion

        #region ComponentsObject

        public static FixedFieldMap<Components> ComponentsFixedFields = new FixedFieldMap<Components> {
            { "schemas", (o,n) => { o.Schemas = n.CreateMap(Schema.Load); } },
            { "parameters", (o,n) => o.Parameters = n.CreateMap(OpenApiV3.LoadParameter) },
            { "responses", (o,n) => o.Responses = n.CreateMap(OpenApiV3.LoadResponse) },
            { "responseHeaders", (o,n) => o.ResponseHeaders = n.CreateMap(Headers.Load) },
            { "securitySchemes", (o,n) => o.SecuritySchemes = n.CreateMap(SecurityScheme.Load) },
            { "callbacks", (o,n) => o.Callbacks = n.CreateMap(Model.Callback.Load) },
            { "links", (o,n) => o.Links = n.CreateMap(Link.Load) },
         };

        public static PatternFieldMap<Components> ComponentsPatternFields = new PatternFieldMap<Components>
        {
            { (s)=> s.StartsWith("x-"), (o,k,n)=> o.Extensions.Add(k, n.GetScalarValue()) }
        };


        public static Components LoadComponents(ParseNode node)
        {
            var mapNode = node.CheckMapNode("components");

            var components = new Components();

            foreach (var itemNode in mapNode)
            {
                itemNode.ParseField(components, ComponentsFixedFields, ComponentsPatternFields);
            }
            return components;
        }

        #endregion

        #region ContactObject

        public static FixedFieldMap<Contact> ContactFixedFields = new FixedFieldMap<Contact> {
            { "name", (o,n) => { o.Name = n.GetScalarValue(); } },
            { "email", (o,n) => { o.Email = n.GetScalarValue(); } },
            { "url", (o,n) => { o.Url = new Uri(n.GetScalarValue()); } },
        };

        public static PatternFieldMap<Contact> ContactPatternFields = new PatternFieldMap<Contact>
        {
            { (s)=> s.StartsWith("x-"), (o,k,n)=> o.Extensions.Add(k, n.GetScalarValue()) }
        };

        public static Contact LoadContact(ParseNode node)
        {
            var contactNode = node as MapNode;
            if (contactNode == null)
            {
                throw new Exception("Contact node should be a map");
            }
            var contact = new Contact();

            foreach (var propertyNode in contactNode)
            {
                propertyNode.ParseField(contact, OpenApiV3.ContactFixedFields, OpenApiV3.ContactPatternFields);
            }

            return contact;
        }

        #endregion

        #region InfoObject

        public static FixedFieldMap<Info> InfoFixedFields = new FixedFieldMap<Info>
        {
            { "title",      (o,n) => { o.Title = n.GetScalarValue(); } },
            { "version",    (o,n) => { o.Version = n.GetScalarValue(); } },
            { "description", (o,n) => { o.Description = n.GetScalarValue(); } },
            { "termsOfService", (o,n) => { o.TermsOfService = n.GetScalarValue(); } },
            { "contact",    (o,n) => { o.Contact = LoadContact(n); } },
            { "license",    (o,n) => { o.License = LoadLicense(n); } }
        };

        public static PatternFieldMap<Info> InfoPatternFields = new PatternFieldMap<Info>
        {
            { (s)=> s.StartsWith("x-"), (o,k,n)=> o.Extensions.Add(k, n.GetScalarValue()) }
        };


        public static Info LoadInfo(ParseNode node)
        {
            var mapNode = node.CheckMapNode("Info");
            var info = new Info();

            var required = new List<string>() { "title", "version" };

            foreach (var propertyNode in mapNode)
            {
                propertyNode.ParseField(info, OpenApiV3.InfoFixedFields, OpenApiV3.InfoPatternFields);
                required.Remove(propertyNode.Name);
            }
            node.Context.ParseErrors.AddRange(required.Select(r => new OpenApiError("", $"{r} is a required property")));

            return info;
        }

        #endregion

        #region LicenseObject

        internal static License LoadLicense(ParseNode node)
        {
            var mapNode = node.CheckMapNode("License");
            var license = new License();

            foreach (var property in mapNode)
            {
                switch (property.Name)
                {
                    case "name":
                        license.Name = property.Value.GetScalarValue();
                        break;

                    case "url":
                        license.Url = new Uri(property.Value.GetScalarValue());
                        break;

                    default:
                        if (property.Name.StartsWith("x-"))
                        {
                            license.Extensions.Add(property.Name, property.Value.GetScalarValue());
                        }
                        break;
                }
            }

            return license;
        }

        #endregion

        #region PathsObject

        public static FixedFieldMap<Paths> PathsFixedFields = new FixedFieldMap<Paths>
        {
            //{ "name", (o,n) => { o.Name = n.GetScalarValue(); } },
            //{ "email", (o,n) => { o.Email = n.GetScalarValue(); } },
            //{ "url", (o,n) => { o.Url = new Uri(n.GetScalarValue()); } },
            // Parameters
            // host
            // scheme
            // basepath
        };

        public static PatternFieldMap<Paths> PathsPatternFields = new PatternFieldMap<Paths>
        {
            { (s)=> s.StartsWith("/"), (o,k,n)=> o.PathItems.Add(k, OpenApiV3.LoadPathItem(n)    ) },
            { (s)=> s.StartsWith("x-"), (o,k,n)=> o.Extensions.Add(k, n.GetScalarValue()) }
        };

        public static Paths LoadPaths(ParseNode node)
        {
            MapNode mapNode = node.CheckMapNode("Paths");

            //Schema domainObject = CreateOrReferenceDomainObject(mapNode, () => new Paths());

            Paths domainObject = new Paths();
            foreach (var propertyNode in mapNode)
            {
                propertyNode.ParseField(domainObject, PathsFixedFields, PathsPatternFields);
            }

            return domainObject;
        }
        #endregion

        #region PathItemObject

        private static FixedFieldMap<PathItem> PathItemFixedFields = new FixedFieldMap<PathItem>
        {
            { "summary", (o,n) => { o.Summary = n.GetScalarValue(); } },
            { "description", (o,n) => { o.Description = n.GetScalarValue(); } },
            { "server", (o,n) => { o.Server = Server.Load(n)    ; } },
            { "parameters", (o,n) => { o.Parameters = n.CreateList(OpenApiV3.LoadParameter); } },

        };

        private static PatternFieldMap<PathItem> PathItemPatternFields = new PatternFieldMap<PathItem>
        {
            { (s)=> s.StartsWith("x-"), (o,k,n)=> o.Extensions.Add(k, n.GetScalarValue()) },
            { (s)=> "get,put,post,delete,patch,options,head".Contains(s),
                (o,k,n)=> o.Operations.Add(k, OpenApiV3.LoadOperation(n)    ) }
        };


        public static PathItem LoadPathItem(ParseNode node)
        {
            var mapNode = node.CheckMapNode("PathItem");
            if (mapNode == null) return null;

            var pathItem = new PathItem();


            foreach (var property in mapNode)
            {
                property.ParseField(pathItem, PathItemFixedFields, PathItemPatternFields);
            }

            return pathItem;
        }

        #endregion

        #region ContentObject

        private static FixedFieldMap<Content> ContentFixedFields = new FixedFieldMap<Content>
        {
        };

        private static PatternFieldMap<Content> ContentPatternFields = new PatternFieldMap<Content>
        {
            { (s)=> s.StartsWith("x-"), (o,k,n)=> o.Extensions.Add(k, n.GetScalarValue()) },
            { (s) => true, (o,k,n)=> o.ContentTypes.Add(k, MediaType.Load(n)    ) }
        };

        public static Content LoadContent(ParseNode node)
        {
            var mapNode = node.CheckMapNode("content");

            var content = new Content();
            foreach (var property in mapNode)
            {
                property.ParseField(content, ContentFixedFields, ContentPatternFields);
            }

            return content;
        }

        #endregion

        #region ResponseObject

        private static FixedFieldMap<Response> ResponseFixedFields = new FixedFieldMap<Response>
        {
            { "description", (o,n) => { o.Description = n.GetScalarValue(); } },
            { "content", (o,n) => { o.Content = OpenApiV3.LoadContent(n); } },
            { "headers", (o,n) => { o.Headers = n.CreateMap(Header.Load); } },
            { "links", (o,n) => { o.Links = n.CreateMap(Link.Load); } }
        };

        private static PatternFieldMap<Response> ResponsePatternFields = new PatternFieldMap<Response>
        {
            { (s)=> s.StartsWith("x-"), (o,k,n)=> o.Extensions.Add(k, n.GetScalarValue()) },
        };

        public static Response LoadResponse(ParseNode node)
        {
            var mapNode = node.CheckMapNode("response");

            var response = new Response();
            foreach (var property in mapNode)
            {
                property.ParseField(response, ResponseFixedFields, ResponsePatternFields);
            }

            return response;
        }

        #endregion

        #region ParameterObject

        private static FixedFieldMap<Parameter> ParameterFixedFields = new FixedFieldMap<Parameter>
        {
            { "name", (o,n) => { o.Name = n.GetScalarValue(); } },
            { "in", (o,n) => { o.In = (InEnum)Enum.Parse(typeof(InEnum), n.GetScalarValue()); } },
            { "description", (o,n) => { o.Description = n.GetScalarValue(); } },
            { "required", (o,n) => { o.Required = bool.Parse(n.GetScalarValue()); } },
            { "deprecated", (o,n) => { o.Deprecated = bool.Parse(n.GetScalarValue()); } },
            { "allowReserved", (o,n) => { o.AllowReserved = bool.Parse(n.GetScalarValue()); } },
            { "style", (o,n) => { o.Style = n.GetScalarValue(); } },
            { "schema", (o,n) => { o.Schema = Schema.Load(n); } },
            { "examples", (o,n) => { o.Examples = ((ListNode)n).Select(s=> new AnyNode(s)).ToList(); } },
            { "example", (o,n) => { o.Example = new AnyNode(n); } },


        };

        private static PatternFieldMap<Parameter> ParameterPatternFields = new PatternFieldMap<Parameter>
        {
            { (s)=> s.StartsWith("x-"), (o,k,n)=> o.Extensions.Add(k, n.GetScalarValue()) },
        };


        public static Parameter LoadParameter(ParseNode node)
        {
            var mapNode = node.CheckMapNode("parameter");

            var refpointer = mapNode.GetReferencePointer();
            if (refpointer != null)
            {
                return mapNode.GetReferencedObject<Parameter>(refpointer);
            }

            var parameter = new Parameter();

            foreach (var item in mapNode)
            {
                item.ParseField(parameter, ParameterFixedFields, ParameterPatternFields);
            }

            return parameter;
        }
        #endregion

        #region OperationObject

        private static FixedFieldMap<Operation> OperationFixedFields = new FixedFieldMap<Operation>
        {
            { "operationId", (o,n) => { o.OperationId = n.GetScalarValue(); } },
            { "summary", (o,n) => { o.Summary = n.GetScalarValue(); } },
            { "description", (o,n) => { o.Description = n.GetScalarValue(); } },
            { "deprecated", (o,n) => { o.Deprecated = bool.Parse(n.GetScalarValue()); } },
            { "requestBody", (o,n) => { o.RequestBody = RequestBody.Load(n)    ; } },
            { "responses", (o,n) => { o.Responses = n.CreateMap(OpenApiV3.LoadResponse); } },
            { "callbacks", (o,n) => { o.Callbacks = n.CreateMap(Model.Callback.Load); } },
            { "server", (o,n) => { o.Server = Server.Load(n); }},
            { "tags", (o,n) => o.Tags = n.CreateSimpleList(Tag.LoadByReference)},
            { "security", (o,n) => { o.Security = n.CreateList(SecurityRequirement.Load); } },
            { "parameters", (o,n) => { o.Parameters = n.CreateList(OpenApiV3.LoadParameter); } },
        };

        private static PatternFieldMap<Operation> OperationPatternFields = new PatternFieldMap<Operation>
        {
            { (s)=> s.StartsWith("x-"), (o,k,n)=> o.Extensions.Add(k, n.GetScalarValue()) },
        };

        internal static Operation LoadOperation(ParseNode node)
        {
            var mapNode = node.CheckMapNode("Operation");
            if (mapNode == null) return null;

            Operation domainObject = new Operation();
            foreach (var property in mapNode)
            {
                property.ParseField(domainObject, OperationFixedFields, OperationPatternFields);
            }

            return domainObject;
        }

        #endregion


    }
}
