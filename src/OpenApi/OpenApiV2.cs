using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tavis.OpenApi.Model;

namespace Tavis.OpenApi
{
    public static class OpenApiV2
    {


        #region OpenApiObject
        public static FixedFieldMap<OpenApiDocument> OpenApiFixedFields = new FixedFieldMap<OpenApiDocument> {
            { "swagger", (o,n) => { /* Ignore it */} },
            { "info", (o,n) => o.Info = LoadInfo(n) },
            { "consumes", (o,n) => n.Context.SetTempStorage("globalconsumes", n.CreateSimpleList<String>((s) => s.GetScalarValue()))},
            { "produces", (o,n) => n.Context.SetTempStorage("globalproduces", n.CreateSimpleList<String>((s) => s.GetScalarValue()))},
            { "host", (o,n) => n.Context.SetTempStorage("host", n.GetScalarValue()) },
            { "basePath", (o,n) => n.Context.SetTempStorage("basePath",n.GetScalarValue()) },
            { "schemes", (o,n) => n.Context.SetTempStorage("schemes", n.CreateSimpleList<String>((s) => { return s.GetScalarValue(); })) },
            { "paths", (o,n) => o.Paths = LoadPaths(n) },
            { "definitions", (o,n) => o.Components.Schemas = n.CreateMap(LoadSchema)  },
            { "parameters", (o,n) => o.Components.Parameters = n.CreateMap(LoadParameter) },
            { "responses", (o,n) => o.Components.Responses = n.CreateMap(LoadResponse) },
            { "securityDefinitions", (o,n) => o.Components.SecuritySchemes = n.CreateMap(LoadSecurityScheme) },
            { "tags", (o,n) => o.Tags = n.CreateList(LoadTag)},
            { "externalDocs", (o,n) => o.ExternalDocs = LoadExternalDocs(n) },
            { "security", (o,n) => o.SecurityRequirements = n.CreateList(LoadSecurityRequirement)}
            };



        private static void MakeServers(List<Server> servers, ParsingContext context)
        {
            string host = context.GetTempStorage<string>("host");
            string basePath = context.GetTempStorage<string>("basePath");
            List<string> schemes = context.GetTempStorage<List<string>>("schemes");

            if (schemes != null)
            {
                foreach (var scheme in schemes)
                {
                    var server = new Server();
                    server.Url = scheme + "://" + (host ?? "example.org/") + (basePath ?? "/");
                    servers.Add(server);
                }
            }
        }

        public static PatternFieldMap<OpenApiDocument> OpenApiPatternFields = new PatternFieldMap<OpenApiDocument>
        {
            // We have no semantics to verify X- nodes, therefore treat them as just values.
            { (s)=> s.StartsWith("x-"), (o,k,n)=> o.Extensions.Add(k, new AnyNode(n)) }
        };

        public static OpenApiDocument LoadOpenApi(RootNode rootNode)
        {
            var openApidoc = new OpenApiDocument();

            var rootMap = rootNode.GetMap();

            bool haspaths = false;
            foreach (var node in rootMap)
            {
                node.ParseField(openApidoc, OpenApiV2.OpenApiFixedFields, OpenApiV2.OpenApiPatternFields);
                if (node.Name == "paths")
                {
                    haspaths = true;
                }
            }

            if (!haspaths)
            {
                rootMap.Context.ParseErrors.Add(new OpenApiError("", "`paths` is a required property"));
            }

            // Post Process OpenApi Object
            MakeServers(openApidoc.Servers, rootMap.Context);

            return openApidoc;
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

            ParseMap(mapNode, info, InfoFixedFields, InfoPatternFields, required);

            ReportMissing(node, required);

            return info;
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
            var mapNode = node as MapNode;
            var contact = new Contact();

            ParseMap(mapNode, contact, ContactFixedFields,ContactPatternFields);

            return contact;
        }

        #endregion

        #region LicenseObject

        public static FixedFieldMap<License> LicenseFixedFields = new FixedFieldMap<License> {
            { "name", (o,n) => { o.Name = n.GetScalarValue(); } },
            { "url", (o,n) => { o.Url = new Uri(n.GetScalarValue()); } },
        };

        public static PatternFieldMap<License> LicensePatternFields = new PatternFieldMap<License>
        {
            { (s)=> s.StartsWith("x-"), (o,k,n)=> o.Extensions.Add(k, n.GetScalarValue()) }
        };

        internal static License LoadLicense(ParseNode node)
        {
            var mapNode = node.CheckMapNode("License");

            var license = new License();

            ParseMap(mapNode, license, LicenseFixedFields, LicensePatternFields);

            return license;
        }

        #endregion

 
        #region PathsObject

        public static FixedFieldMap<Paths> PathsFixedFields = new FixedFieldMap<Paths>
        {
        };

        public static PatternFieldMap<Paths> PathsPatternFields = new PatternFieldMap<Paths>
        {
            { (s)=> s.StartsWith("/"), (o,k,n)=> o.PathItems.Add(k, OpenApiV2.LoadPathItem(n)    ) },
            { (s)=> s.StartsWith("x-"), (o,k,n)=> o.Extensions.Add(k, n.GetScalarValue()) }
        };

        public static Paths LoadPaths(ParseNode node)
        {
            MapNode mapNode = node.CheckMapNode("Paths");

            Paths domainObject = new Paths();

            ParseMap(mapNode, domainObject, PathsFixedFields, PathsPatternFields);

            return domainObject;
        }
        #endregion

        #region PathItemObject

        private static FixedFieldMap<PathItem> PathItemFixedFields = new FixedFieldMap<PathItem>
        {
            // $ref
            { "summary", (o,n) => { o.Summary = n.GetScalarValue(); } },
            { "description", (o,n) => { o.Description = n.GetScalarValue(); } },
            { "parameters", (o,n) => { o.Parameters = n.CreateList(OpenApiV2.LoadParameter); } },

        };

        private static PatternFieldMap<PathItem> PathItemPatternFields = new PatternFieldMap<PathItem>
        {
            { (s)=> s.StartsWith("x-"), (o,k,n)=> o.Extensions.Add(k, n.GetScalarValue()) },
            { (s)=> "get,put,post,delete,patch,options,head,patch,trace".Contains(s),
                (o,k,n)=> o.Operations.Add(k, OpenApiV2.LoadOperation(n)    ) }
        };


        public static PathItem LoadPathItem(ParseNode node)
        {
            var mapNode = node.CheckMapNode("PathItem");

            var pathItem = new PathItem();

            ParseMap(mapNode, pathItem, PathItemFixedFields, PathItemPatternFields);

            return pathItem;
        }

        #endregion

        #region OperationObject

        private static FixedFieldMap<Operation> OperationFixedFields = new FixedFieldMap<Operation>
        {
            { "tags", (o,n) => o.Tags = n.CreateSimpleList(Tag.LoadByReference)},
            { "summary", (o,n) => { o.Summary = n.GetScalarValue(); } },
            { "description", (o,n) => { o.Description = n.GetScalarValue(); } },
            { "externalDocs", (o,n) => { o.ExternalDocs = LoadExternalDocs(n); } },
            { "operationId", (o,n) => { o.OperationId = n.GetScalarValue(); } },
            { "parameters", (o,n) => { o.Parameters = n.CreateList(LoadParameter); } },
            { "consumes", (o,n) => n.Context.SetTempStorage("operationconsumes", n.CreateSimpleList<String>((s) => s.GetScalarValue()))},
            { "produces", (o,n) => n.Context.SetTempStorage("operationproduces", n.CreateSimpleList<String>((s) => s.GetScalarValue()))},
            { "responses", (o,n) => { o.Responses = n.CreateMap(LoadResponse); } },
            { "deprecated", (o,n) => { o.Deprecated = bool.Parse(n.GetScalarValue()); } },
            { "security", (o,n) => { o.Security = n.CreateList(LoadSecurityRequirement); } },
          };

        private static PatternFieldMap<Operation> OperationPatternFields = new PatternFieldMap<Operation>
        {
            { (s)=> s.StartsWith("x-"), (o,k,n)=> o.Extensions.Add(k, new AnyNode(n)) },
        };

        internal static Operation LoadOperation(ParseNode node)
        {
            var mapNode = node.CheckMapNode("Operation");

            Operation operation = new Operation();

            ParseMap(mapNode, operation, OperationFixedFields, OperationPatternFields);


            return operation;
        }

        #endregion

        #region ExternalDocsObject

        private static FixedFieldMap<ExternalDocs> ExternalDocsFixedFields = new FixedFieldMap<ExternalDocs>
        {
            // $ref
            { "description", (o,n) => { o.Description = n.GetScalarValue(); } },
            { "url", (o,n) => { o.Url = new Uri(n.GetScalarValue()); } },
        };

        private static PatternFieldMap<ExternalDocs> ExternalDocsPatternFields = new PatternFieldMap<ExternalDocs>
        {
        };


        public static ExternalDocs LoadExternalDocs(ParseNode node)
        {
            var mapNode = node.CheckMapNode("externalDocs");

            var externalDocs = new ExternalDocs();

            ParseMap(mapNode, externalDocs, ExternalDocsFixedFields, ExternalDocsPatternFields);

            return externalDocs;
        }

        #endregion

        #region ParameterObject

        private static FixedFieldMap<Parameter> ParameterFixedFields = new FixedFieldMap<Parameter>
        {
            { "name",           (o,n) => { o.Name = n.GetScalarValue(); } },
            { "in",             (o,n) => { ProcessIn(o,n); } },
            { "description",    (o,n) => { o.Description = n.GetScalarValue(); } },
            { "required",       (o,n) => { o.Required = bool.Parse(n.GetScalarValue()); } },
            { "deprecated",     (o,n) => { o.Deprecated = bool.Parse(n.GetScalarValue()); } },
            { "allowEmptyValue", (o,n) => { o.AllowEmptyValue = bool.Parse(n.GetScalarValue()); } },
            { "examples",       (o,n) => { o.Examples = ((ListNode)n).Select(s=> new AnyNode(s)).ToList(); } },
            { "example",        (o,n) => { o.Example = new AnyNode(n); } },
            { "type", (o,n) => { GetOrCreateSchema(n.Context).Type = n.GetScalarValue(); } },
            { "items", (o,n) => { GetOrCreateSchema(n.Context).Items = LoadSchema(n); } },
            { "collectionFormat", (o,n) => { /* Convert to style */ } },
            { "format", (o,n) => { GetOrCreateSchema(n.Context).Format = n.GetScalarValue(); } },
            { "minimum", (o,n) => { GetOrCreateSchema(n.Context).Minimum = n.GetScalarValue(); } },
            { "maximum", (o,n) => { GetOrCreateSchema(n.Context).Maximum = n.GetScalarValue(); } },
            { "default", (o,n) => { GetOrCreateSchema(n.Context).Default = n.GetScalarValue(); } },
            { "enum", (o,n) => { GetOrCreateSchema(n.Context).Enum = n.CreateSimpleList<String>(l=>l.GetScalarValue()); } },
            { "schema", (o,n) => { n.Context.SetTempStorage("bodyschema",LoadSchema(n)); } },
        };

        private static Schema GetOrCreateSchema(ParsingContext context)
        {
            var schema = context.GetTempStorage<Schema>("schema");
            if (schema == null)
            {
                schema = new Schema();
                context.SetTempStorage("schema", schema);
            }
            return schema;
        }

        private static void ProcessIn(Parameter o, ParseNode n)
        {

            string value = n.GetScalarValue();
            switch(value)
            {
                case "body":
                    n.Context.SetTempStorage("bodyType", "body");
                    break;
                case "form":
                    n.Context.SetTempStorage("bodyType", "form");
                    break;
                default:
                    o.In = (InEnum)Enum.Parse(typeof(InEnum), value);
                    break;
            }
            
        }

        private static PatternFieldMap<Parameter> ParameterPatternFields = new PatternFieldMap<Parameter>
        {
            { (s)=> s.StartsWith("x-"), (o,k,n)=> o.Extensions.Add(k, new AnyNode(n)) },
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

            ParseMap(mapNode, parameter, ParameterFixedFields, ParameterPatternFields);

            var schema = node.Context.GetTempStorage<Schema>("schema");
            if (schema != null)
            {
                parameter.Schema = schema;
                node.Context.SetTempStorage("schema", null);
            }

            var bodyType = node.Context.GetTempStorage<string>("bodyType");
            switch(bodyType)
            {
                case "body":
                    var consumes = node.Context.GetTempStorage<List<string>>("operationproduces")
                          ?? node.Context.GetTempStorage<List<string>>("globalproduces")
                          ?? new List<string>() { "application/json" };


                    parameter.Content = new Dictionary<string, MediaType>();
                    foreach (var consume in consumes)
                    {
                        parameter.Content.Add(consume, new MediaType()
                        {
                            Schema = node.Context.GetTempStorage<Schema>("bodyschema")
                        });
                    }
                    parameter.Schema = null;
                    break;
                case "form":

                    break;

            }

            return parameter;
        }
        #endregion
        
        #region ResponseObject

        private static FixedFieldMap<Response> ResponseFixedFields = new FixedFieldMap<Response>
        {
            { "description", (o,n) => { o.Description = n.GetScalarValue(); } },
            { "headers", (o,n) => { o.Headers = n.CreateMap(LoadHeader); } },
            { "examples",       (o,n) => { /*o.Examples = ((ListNode)n).Select(s=> new AnyNode(s)).ToList();*/ } },
            { "schema", (o,n) => { n.Context.SetTempStorage("operationschema", LoadSchema(n)); } },

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

            ProcessProduces(response, node.Context);

            return response;
        }

        private static void ProcessProduces(Response response, ParsingContext context)
        {

            var produces = context.GetTempStorage<List<string>>("operationproduces") 
                  ?? context.GetTempStorage<List<string>>("globalproduces") 
                  ?? new List<string>() { "application/json" };

            response.Content = new Dictionary<string, MediaType>();
            foreach (var mt in produces)
            {
                response.Content.Add(mt, new MediaType()
                {
                    Schema = context.GetTempStorage<Schema>("operationschema")
                });
            }

        }

        #endregion

        #region HeaderObject
        private static FixedFieldMap<Header> HeaderFixedFields = new FixedFieldMap<Header>
        {
            { "description", (o,n) => { o.Description = n.GetScalarValue(); } },
            { "required", (o,n) => { o.Required = bool.Parse(n.GetScalarValue()); } },
            { "deprecated", (o,n) => { o.Deprecated = bool.Parse(n.GetScalarValue()); } },
            { "allowReserved", (o,n) => { o.AllowReserved = bool.Parse(n.GetScalarValue()); } },
            { "style", (o,n) => { o.Style = n.GetScalarValue(); } },
            { "type", (o,n) => { GetOrCreateSchema(n.Context).Type = n.GetScalarValue(); } },
            { "format", (o,n) => { GetOrCreateSchema(n.Context).Format = n.GetScalarValue(); } },

        };

        private static PatternFieldMap<Header> HeaderPatternFields = new PatternFieldMap<Header>
        {
            { (s)=> s.StartsWith("x-"), (o,k,n)=> o.Extensions.Add(k, n.GetScalarValue()) },
        };


        public static Header LoadHeader(ParseNode node)
        {
            var mapNode = node.CheckMapNode("header");
            var header = new Header();
            foreach (var property in mapNode)
            {
                property.ParseField(header, HeaderFixedFields, HeaderPatternFields);
            }

            var schema = node.Context.GetTempStorage<Schema>("schema");
            if (schema != null)
            {
                header.Schema = schema;
                node.Context.SetTempStorage("schema", null);
            }

            return header;
        }

        #endregion

        #region ExampleObject
        private static FixedFieldMap<Example> ExampleFixedFields = new FixedFieldMap<Example>
        {
        };

        private static PatternFieldMap<Example> ExamplePatternFields = new PatternFieldMap<Example>
        {
            { (s)=> s.StartsWith("x-"), (o,k,n)=> o.Extensions.Add(k, n.GetScalarValue()) }
        };

        public static Example LoadExample(ParseNode node)
        {
            var mapNode = node.CheckMapNode("Example");
            var example = new Example();
            foreach (var property in mapNode)
            {
                property.ParseField(example, ExampleFixedFields, ExamplePatternFields);
            }

            return example;
        }


        #endregion

        #region TagObject
        internal static Tag LoadTag(ParseNode n)
        {
            var mapNode = n.CheckMapNode("tag");

            var obj = new Tag();

            foreach (var node in mapNode)
            {
                var key = node.Name;
                switch (key)
                {
                    case "description":
                        obj.Description = node.Value.GetScalarValue();
                        break;
                    case "name":
                        obj.Name = node.Value.GetScalarValue();
                        break;

                }
            }
            return obj;
        }


        #endregion

        #region SchemaObject

        private static FixedFieldMap<Schema> SchemaFixedFields = new FixedFieldMap<Schema>
        {
                { "type", (o,n) => { o.Type = n.GetScalarValue(); } },
                { "format", (o,n) => { o.Description = n.GetScalarValue(); } },
                { "description", (o,n) => { o.Type = n.GetScalarValue(); } },
                { "required", (o,n) => { o.Required = n.CreateSimpleList<string>(n2 => n2.GetScalarValue()).ToArray(); } },
                { "items", (o,n) => { o.Items = LoadSchema(n); } },
                { "properties", (o,n) => { o.Properties = n.CreateMap(LoadSchema); } },
                { "allOf", (o,n) => { o.AllOf = n.CreateList(LoadSchema); } },
                { "examples", (o,n) => { o.Examples = ((ListNode)n).Select(s=> new AnyNode(s)).ToList(); } },
                { "example", (o,n) => { o.Example = new AnyNode(n); } },
                { "enum", (o,n) => { o.Enum =  n.CreateSimpleList<string>((s)=> s.GetScalarValue()); } },


        };

        private static PatternFieldMap<Schema> SchemaPatternFields = new PatternFieldMap<Schema>
        {
            { (s)=> s.StartsWith("x-"), (o,k,n)=> o.Extensions.Add(k, new AnyNode(n)) }
        };


        public static Schema LoadSchema(ParseNode node)
        {
            MapNode mapNode = node.CheckMapNode("schema");

            var refpointer = mapNode.GetReferencePointer();
            if (refpointer != null)
            {
                return mapNode.GetReferencedObject<Schema>(refpointer);
            }

            var domainObject = new Schema();

            foreach (var propertyNode in mapNode)
            {
                propertyNode.ParseField(domainObject, SchemaFixedFields, SchemaPatternFields);
            }

            return domainObject;
        }


        #endregion

        #region SecuritySchemeObject

        private static FixedFieldMap<SecurityScheme> SecuritySchemeFixedFields = new FixedFieldMap<SecurityScheme>
        {
            { "type", (o,n) => { o.Type = n.GetScalarValue();  } },
            { "description", (o,n) => { o.Description = n.GetScalarValue();  } },
            { "name", (o,n) => { o.Name = n.GetScalarValue();  } },
            { "in", (o,n) => { o.In = n.GetScalarValue();  } },
            { "scheme", (o,n) => { o.Scheme = n.GetScalarValue();  } },
            { "bearerFormat", (o,n) => { o.BearerFormat = n.GetScalarValue();  } },
            { "openIdConnectUrl", (o,n) => { o.OpenIdConnectUrl = new Uri(n.GetScalarValue(), UriKind.RelativeOrAbsolute);  } },
            { "flow", (o,n) => { o.Flow = n.GetScalarValue();  } },
            { "authorizationUrl", (o,n) => { o.AuthorizationUrl = new Uri(n.GetScalarValue(), UriKind.RelativeOrAbsolute);  } },
            { "tokenUrl", (o,n) => { o.TokenUrl = new Uri(n.GetScalarValue(), UriKind.RelativeOrAbsolute);  } },
            { "scopes", (o,n) => { o.Scopes= n.CreateSimpleMap<string>(v => v.GetScalarValue()  ); } },
        };

        private static PatternFieldMap<SecurityScheme> SecuritySchemePatternFields = new PatternFieldMap<SecurityScheme>
        {
            { (s)=> s.StartsWith("x-"), (o,k,n)=> o.Extensions.Add(k, new AnyNode(n)) }
        };

        public static SecurityScheme LoadSecurityScheme(ParseNode node)
        {
            var mapNode = node.CheckMapNode("securityScheme");

            var securityScheme = new SecurityScheme();
            foreach (var property in mapNode)
            {
                property.ParseField(securityScheme, SecuritySchemeFixedFields, SecuritySchemePatternFields);
            }

            return securityScheme;
        }

        #endregion

        #region SecurityRequirement
        public static SecurityRequirement LoadSecurityRequirement(ParseNode node)
        {

            var mapNode = node.CheckMapNode("security");

            var obj = new SecurityRequirement();

            foreach (var property in mapNode)
            {
                var scheme = SecurityScheme.LoadByReference(new ValueNode(mapNode.Context, property.Name));
                if (scheme != null)
                {
                    obj.Schemes.Add(scheme, property.Value.CreateSimpleList<string>(n2 => n2.GetScalarValue()));
                } else
                {
                    node.Context.ParseErrors.Add(new OpenApiError(node.Context.GetLocation(), $"Scheme {property.Name} is not found"));
                }
            }
            return obj;
        }

        #endregion



        public static IReference LoadReference(string pointer, RootNode rootNode)
        {
            var parts = pointer.Split('/').Reverse().Take(2).ToArray();

            string refType;
            if ("securityDefinitions|parameters|tags".Contains(parts.Last()))
            {
                refType = parts.Last();
            } else
            {
                refType = "definitions";
            }

            IReference referencedObject = null;

            if ("securityDefinitions|parameters|definitions".Contains(refType))
            {
                var refPointer = new JsonPointer(pointer.StartsWith("#") ? pointer: "/definitions/" + parts[0]);
                ParseNode node = rootNode.Find(refPointer);
                if (node == null) return null;
                node.DomainType = refType;

                switch (refType)
                {
                    case "definitions":
                        referencedObject = OpenApiV3.LoadSchema(node);
                        break;
                    case "parameters":
                        referencedObject = OpenApiV3.LoadParameter(node);
                        break;
                    case "securityDefinitions":
                        referencedObject = OpenApiV3.LoadSecurityScheme(node);
                        break;
                }
            }
            else if ("tags".Contains(refType))
            {

                ListNode list = (ListNode)rootNode.Find(new JsonPointer("/tags"));
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        var tag = OpenApiV3.LoadTag(item);

                        if (tag.Name == parts[0])
                        {
                            return tag;
                        }
                    }
                }
                else
                {
                    return new Tag() { Name = parts[0] };
                }
            }
            else
            {
                throw new DomainParseException($"Unknown $ref {refType} at {pointer}");

            }


            return referencedObject;
        }


        private static void ParseMap<T>(MapNode mapNode, T domainObject, FixedFieldMap<T> fixedFieldMap, PatternFieldMap<T> patternFieldMap, List<string> requiredFields  = null)
        {
            if (mapNode == null) return;

            foreach (var propertyNode in mapNode)
            {
                propertyNode.ParseField<T>(domainObject, fixedFieldMap, patternFieldMap);
                if (requiredFields != null) requiredFields.Remove(propertyNode.Name);
            }
        }

        private static void ReportMissing(ParseNode node, List<string> required)
        {
            node.Context.ParseErrors.AddRange(required.Select(r => new OpenApiError("", $"{r} is a required property")));
        }

    }
}
