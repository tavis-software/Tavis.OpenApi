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
        IReferenceService referenceService;

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

            this.context = new ParsingContext();
            try
            {
                this.rootNode = new RootNode(this.context, stream);

            }
            catch (SharpYaml.SyntaxErrorException ex)
            {
                ParseErrors.Add(new OpenApiError("", ex.Message));
                return new OpenApiDocument();
            }

            inputVersion = GetVersion(this.rootNode);

            switch (inputVersion)
            {
                case "2.0":
                    this.context.SetReferenceService(new ReferenceService(this.rootNode)
                    {
                        loadReference = OpenApiV2.LoadReference,
                        parseReference = p => OpenApiV2.ParseReference(p)
                    });
                    return OpenApiV2.LoadOpenApi(this.rootNode);
                default:
                    this.context.SetReferenceService(new ReferenceService(this.rootNode)
                    {
                        loadReference = OpenApiV3.LoadReference,
                        parseReference = p => new OpenApiReference(p)
                    });
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

    }

    public class ReferenceService : IReferenceService
    {
        internal Func<OpenApiReference, RootNode, IReference> loadReference { get; set; }
        internal Func<string, OpenApiReference> parseReference { get; set; }

        private RootNode rootNode;

        public ReferenceService(RootNode rootNode)
        {
            this.rootNode = rootNode;
        }
        public IReference LoadReference(OpenApiReference reference)
        {
            var referenceObject = this.loadReference(reference,this.rootNode);
            if (referenceObject == null)
            {
                throw new DomainParseException($"Cannot locate $ref {reference.ToString()}");
            }
            return referenceObject;
        }

        public OpenApiReference ParseReference(string pointer)
        {
            return this.parseReference(pointer);
        }
    }
}



