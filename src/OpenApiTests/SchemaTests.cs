using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tavis.OpenApi;
using Xunit;

namespace OpenApiTests
{
    public class SchemaTests
    {

        [Fact]
        public void CheckPetStoreApiInfo()
        {
            var stream = this.GetType().Assembly.GetManifestResourceStream("OpenApiTests.Samples.petstore30.yaml");

            var parser = new OpenApiParser();
            var openApiDoc = parser.Parse(stream);
            var operation = openApiDoc.Paths.PathItems["/pets"].Operations["get"];
            var schema = operation.Responses["200"].Content.MediaTypes["application/json"].Schema;
            Assert.NotNull(schema);

        }
    }
}
