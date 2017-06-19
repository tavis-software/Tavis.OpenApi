using System;
using System.Collections.Generic;
using System.IO;
using Tavis.OpenApi.Model;

namespace Tavis.OpenApi
{
    public class OpenApiParser
    {

        ParsingContext context;
        RootNode rootNode;
        string inputVersion;

        public List<OpenApiError> ParseErrors
        {
            get
            {
                return context.ParseErrors;
            }
        }

        public OpenApiDocument Parse(string document)
        {
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(document);
            writer.Flush();
            ms.Position = 0;
            return Parse(ms);
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

            inputVersion = GetVersion(this.rootNode);

            switch (inputVersion)
            {
                case "2.0":
                    return OpenApiV2.LoadOpenApi(this.rootNode);
                default:
                    return OpenApiV3.LoadOpenApi(this.rootNode);

            }

        }

        private string GetVersion(RootNode rootNode)
        {
            var versionNode = rootNode.Find(new JsonPointer("/openapi"));
            if (versionNode != null)
            {
                return versionNode.GetScalarValue();
            }

            versionNode = rootNode.Find(new JsonPointer("/swagger"));
            if (versionNode != null)
            {
                return versionNode.GetScalarValue();
            }
            return null;
        }

        private IReference LoadReference(string pointer)
        {
            var rootNode = this.rootNode;
            switch (this.inputVersion)
            {
                case "2.0":
                    return OpenApiV2.LoadReference(pointer, rootNode);

                case "3.0.0":
                    return OpenApiV3.LoadReference(pointer, rootNode);

                default:
                    return null;
            }
        }
    }


}



