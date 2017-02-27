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
            var refPointer = new JsonPointer(pointer);
            ParseNode node = this.rootNode.Find(refPointer);
            node.DomainType = pointer.Split('/')[2];
            IReference referencedObject = null;
            switch (node.DomainType) // Not sure how to get this...
            {
                case "definitions":
                    referencedObject = Schema.Load(node);
                    break;
                case "parameters":
                    referencedObject = Parameter.Load(node);
                    break;
                case "callbacks":
                    referencedObject = Callbacks.Load(node);
                    break;

                // etc
                default:
                    throw new DomainParseException($"Unknown $ref {node.DomainType}");
            }
            return referencedObject;
        }


    }


}
