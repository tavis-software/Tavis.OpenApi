using SharpYaml.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
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
            var schema = operation.Responses["200"].Content["application/json"].Schema;
            Assert.NotNull(schema);

        }

        [Fact]
        public void CreateSchemaFromInlineJsonSchema()
        {
            var jsonSchema = " { \"type\" : \"int\" } ";

            var mapNode = MapNode.Create(jsonSchema);

            var schema = OpenApiV3.LoadSchema(mapNode);

            Assert.NotNull(schema);
            Assert.Equal("int", schema.Type);
        }
    }
}
