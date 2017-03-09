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
            return Load(this.rootNode);
        }

        public static OpenApiDocument Load(RootNode rootNode)
        {
            var openApidoc = new OpenApiDocument();

            var rootMap = rootNode.GetMap();
            bool haspaths = false;
            foreach (var node in rootMap)
            {
                node.ParseField<OpenApiDocument>(openApidoc, OpenApiDocument.fixedFields, OpenApiDocument.patternFields);
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


    }


}
