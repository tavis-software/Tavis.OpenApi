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
    public class OAIExampleTests
    {
        public const string ExamplesFolder = "C:\\Users\\Darrel\\Documents\\Source\\GitHub\\OAI\\OpenAPI-Specification\\";

        [Fact]
        public void SimplePetStore()
        {
            var stream = new FileStream(ExamplesFolder + "examples\\v3.0\\Petstore.yaml",FileMode.Open);
            var parser = new OpenApiParser();
            var openApiDoc = parser.Parse(stream);

            Assert.Equal(0,parser.ParseErrors.Count());
        }


    }
}
