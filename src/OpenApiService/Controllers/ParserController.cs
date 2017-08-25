using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using Tavis.OpenApi;

namespace OpenApiService.Controllers
{
    public class ParserController : ApiController
    {
        public async Task<HttpResponseMessage> Post()
        {

            var context = OpenApiParser.Parse(await this.Request.Content.ReadAsStreamAsync());

            var errors = context.ParseErrors;
            if (errors.Count == 0)
            {
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new OpenApiContent(context.OpenApiDocument)
                };
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(String.Join("\n", errors.Select(e => e.ToString()).ToArray()))
                };
            }
        }
    }
}