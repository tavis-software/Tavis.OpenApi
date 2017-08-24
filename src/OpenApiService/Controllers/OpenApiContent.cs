using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Tavis.OpenApi;
using Tavis.OpenApi.Model;

namespace OpenApiService.Controllers
{
    public class OpenApiContent : HttpContent
    {
        OpenApiDocument document; 
        public OpenApiContent(OpenApiDocument doc)
        {
            this.document = doc;
            this.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/openapi+yaml");

        }
        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            var writer = new OpenApiV3Writer(this.document);
            writer.Write(stream);
            stream.Flush();
            return Task.FromResult<object>(null);
        }

        protected override bool TryComputeLength(out long length)
        {
            length = -1;
            return false;
        }
    }
}