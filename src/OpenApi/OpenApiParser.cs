using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tavis.OpenApi.Model;

namespace Tavis.OpenApi
{
    public class OpenApiParser
    {

        ParsingContext context;
        private RootNode rootNode;
        
        public List<OpenApiError> ParseErrors
        {
            get
            {
                return context.ParseErrors;
            }
        }

        public OpenApiDocument Parse(Stream stream)
        {
            this.context = new ParsingContext(LoadReference);
            this.rootNode = new RootNode(this.context, stream);
            return OpenApiDocument.Load(this.rootNode);
        }


        private IReference LoadReference(string pointer)
        {
            var parts = pointer.Split('/').Reverse().Take(2).ToArray();
            var refType = parts[1];  
            IReference referencedObject = null;

            if ("schemas|parameters|callbacks|securitySchemes".Contains(refType))
            {
                var refPointer = new JsonPointer(pointer);
                ParseNode node = this.rootNode.Find(refPointer);
                if (node == null) return null;
                node.DomainType = refType;

                switch (refType)
                {
                    case "schemas":
                        referencedObject = Schema.Load(node);
                        break;
                    case "parameters":
                        referencedObject = Parameter.Load(node);
                        break;
                    case "callbacks":
                        referencedObject = Callback.Load(node);
                        break;
                    case "securitySchemes":
                        referencedObject = SecurityScheme.Load(node);
                        break;
                }
            }
            else if ("tags".Contains(refType))
            {
                
                ListNode list = (ListNode)this.rootNode.Find(new JsonPointer("/tags"));
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        var tag = Tag.Load(item);

                        if (tag.Name == parts[0])
                        {
                            return tag;
                        }
                    }
                } else
                {
                    return new Tag() { Name = parts[0] };
                }
            }
            else
            {
                throw new DomainParseException($"Unknown $ref {refType}");

            }


            return referencedObject;
        }

        public static FixedFieldMap<OpenApiDocument> OpenApiFixedFields = new FixedFieldMap<OpenApiDocument> {
            { "openapi", (o,n) => { o.Version = n.GetScalarValue(); } },
            { "info", (o,n) => o.Info = Info.Load(n) },
            { "servers", (o,n) => o.Servers = n.CreateList<Server>(Server.Load) },
            { "paths", (o,n) => o.Paths = Paths.Load(n) },
            { "components", (o,n) => o.Components = Components.Load(n) },
            { "tags", (o,n) => o.Tags = n.CreateList(Tag.Load)},
            { "externalDocs", (o,n) => o.ExternalDocs = ExternalDocs.Load(n) },
            { "security", (o,n) => o.SecurityRequirements = n.CreateList(SecurityRequirement.Load)}

            };

        public static PatternFieldMap<OpenApiDocument> OpenApiPatternFields = new PatternFieldMap<OpenApiDocument>
        {
            // We have no semantics to verify X- nodes, therefore treat them as just values.
            { (s)=> s.StartsWith("x-"), (o,k,n)=> o.Extensions.Add(k, ((ValueNode)n).GetScalarValue()) }
        };


        public static FixedFieldMap<Components> ComponentsFixedFields = new FixedFieldMap<Components> {
            { "schemas", (o,n) => { o.Schemas = n.CreateMap(Schema.Load); } },
            { "parameters", (o,n) => o.Parameters = n.CreateMap(Parameter.Load) },
            { "responses", (o,n) => o.Responses = n.CreateMap(Response.Load) },
            { "responseHeaders", (o,n) => o.ResponseHeaders = n.CreateMap(Headers.Load) },
            { "securitySchemes", (o,n) => o.SecuritySchemes = n.CreateMap(SecurityScheme.Load) },
            { "callbacks", (o,n) => o.Callbacks = n.CreateMap(Model.Callback.Load) },
            { "links", (o,n) => o.Links = n.CreateMap(Link.Load) },
            };

        public static PatternFieldMap<Components> ComponentsPatternFields = new PatternFieldMap<Components>
        {
            { (s)=> s.StartsWith("x-"), (o,k,n)=> o.Extensions.Add(k, n.GetScalarValue()) }
        };

        public static FixedFieldMap<Contact> ContactFixedFields = new FixedFieldMap<Contact> {
            { "name", (o,n) => { o.Name = n.GetScalarValue(); } },
            { "email", (o,n) => { o.Email = n.GetScalarValue(); } },
            { "url", (o,n) => { o.Url = new Uri(n.GetScalarValue()); } },
        };

        public static PatternFieldMap<Contact> ContactPatternFields = new PatternFieldMap<Contact>
        {
            { (s)=> s.StartsWith("x-"), (o,k,n)=> o.Extensions.Add(k, n.GetScalarValue()) }
        };

        public static FixedFieldMap<Info> InfoFixedFields = new FixedFieldMap<Info>
        {
            { "title", (o,n) => { o.Title = n.GetScalarValue(); } },
            { "version", (o,n) => { o.Version = n.GetScalarValue(); } },
            { "description", (o,n) => { o.Description = n.GetScalarValue(); } },
            { "termsOfService", (o,n) => { o.TermsOfService = n.GetScalarValue(); } },
            { "contact", (o,n) => { o.Contact = Contact.Load(n); } },
            { "license", (o,n) => { o.License = License.Load(n); } }
        };

        public static PatternFieldMap<Info> InfoPatternFields = new PatternFieldMap<Info>
        {
            { (s)=> s.StartsWith("x-"), (o,k,n)=> o.Extensions.Add(k, n.GetScalarValue()) }
        };

    }


}
