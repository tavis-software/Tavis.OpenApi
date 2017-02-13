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

        public List<OpenApiError> ParseErrors
        {
            get
            {
                return context.ParseErrors;
            }
        }

        public OpenApiDocument Parse(Stream stream)
        {
            IReferenceStore referenceStore = new ReferenceStore();
            this.context = new ParsingContext(referenceStore);
            var rootNode = new RootNode(this.context, stream);
            return Load(rootNode);
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


    }
}
