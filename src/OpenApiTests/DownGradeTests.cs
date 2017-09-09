using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tavis.OpenApi;
using Tavis.OpenApi.Model;
using Xunit;

namespace OpenApiTests
{
    public class DownGradeTests
    {
        public void SimpleTest()
        {
            var stream = this.GetType().Assembly.GetManifestResourceStream("OpenApiTests.Samples.Simplest.yaml");

            var openApiDoc = OpenApiParser.Parse(stream).OpenApiDocument;

            var outputStream = new MemoryStream();
            openApiDoc.Save(outputStream, new OpenApiV2Writer());


        }

        [Fact]
        public void EmptyTest()
        {
            var openApiDoc = new OpenApiDocument();

            JObject jObject = ExportV2ToJObject(openApiDoc);

            Assert.Equal("2.0", jObject["swagger"]);
            Assert.NotNull(jObject["info"]);

        }


        [Fact]
        public void HostTest()
        {
            var openApiDoc = new OpenApiDocument();
            openApiDoc.Servers.Add(new Server() { Url = "http://example.org/api" });
            openApiDoc.Servers.Add(new Server() { Url = "https://example.org/api" });

            JObject jObject = ExportV2ToJObject(openApiDoc);

            Assert.Equal("example.org", (string)jObject["host"]);
            Assert.Equal("/api", (string)jObject["basePath"]);

        }

        private static JObject ExportV2ToJObject(OpenApiDocument openApiDoc)
        {
            var outputStream = new MemoryStream();
            openApiDoc.Save(outputStream, new OpenApiV2Writer((s) => new JsonParseNodeWriter(s)));
            outputStream.Position = 0;
            var json = new StreamReader(outputStream).ReadToEnd();
            var jObject = JObject.Parse(json);
            return jObject;
        }
    }
}
