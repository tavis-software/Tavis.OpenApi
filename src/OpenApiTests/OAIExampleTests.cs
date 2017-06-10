using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tavis.OpenApi;
using Xunit;

namespace OpenApiTests
{
    public class OAIExampleTests
    {
        HttpClient client;
        public OAIExampleTests()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://raw.githubusercontent.com/OAI/OpenAPI-Specification/OpenAPI.next/examples/v3.0/");
        }
        [Fact]
        public async Task SimplePetStore()
        {
            var stream = await client.GetStreamAsync("petstore.yaml");
            var parser = new OpenApiParser();
            var openApiDoc = parser.Parse(stream);

            Assert.Equal(0,parser.ParseErrors.Count());
        }

        [Fact]
        public async Task UberExample()
        {
            var stream = await client.GetStreamAsync("uber.yaml");
            var parser = new OpenApiParser();
            var openApiDoc = parser.Parse(stream);

            Assert.Equal(0, parser.ParseErrors.Count());
        }

        [Fact]
        public async Task PetStoreExpandedExample()
        {
            var stream = await client.GetStreamAsync("petstore-expanded.yaml");
            var parser = new OpenApiParser();
            var openApiDoc = parser.Parse(stream);

            Assert.Equal(0, parser.ParseErrors.Count());
        }

        [Fact(Skip = "Example is not updated yet")]
        public async Task ApiWithExamples()
        {
            var stream = await client.GetStreamAsync("api-with-examples.yaml");
            var parser = new OpenApiParser();
            var openApiDoc = parser.Parse(stream);

            Assert.Equal(0, parser.ParseErrors.Count());
        }
    }
}
