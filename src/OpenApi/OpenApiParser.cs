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
            try
            {
                this.rootNode = new RootNode(this.context, stream);
            } catch(SharpYaml.SyntaxErrorException ex)
            {
                ParseErrors.Add(new OpenApiError("", ex.Message));
                return new OpenApiDocument();
            }
            return OpenApiV3.LoadOpenApi(this.rootNode);
        }

        private IReference LoadReference(string pointer)
        {
            var parts = pointer.Split('/').Reverse().Take(2).ToArray();
            var refType = parts[1];  
            IReference referencedObject = null;

            if ("schemas|parameters|callbacks|securitySchemes|links".Contains(refType))
            {
                var refPointer = new JsonPointer(pointer);
                ParseNode node = this.rootNode.Find(refPointer);
                if (node == null) return null;
                node.DomainType = refType;

                switch (refType)
                {
                    case "schemas":
                        referencedObject = OpenApiV3.LoadSchema(node);
                        break;
                    case "parameters":
                        referencedObject = OpenApiV3.LoadParameter(node);
                        break;
                    case "callbacks":
                        referencedObject = OpenApiV3.LoadCallback(node);
                        break;
                    case "securitySchemes":
                        referencedObject = OpenApiV3.LoadSecurityScheme(node);
                        break;
                    case "links":
                        referencedObject = OpenApiV3.LoadLink(node);
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
                        var tag = OpenApiV3.LoadTag(item);

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
                throw new DomainParseException($"Unknown $ref {refType} at {pointer}");

            }


            return referencedObject;
        }


    }


}
