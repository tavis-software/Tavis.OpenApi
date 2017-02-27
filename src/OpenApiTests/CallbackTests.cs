using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tavis.OpenApi;
using Xunit;

namespace OpenApiTests
{

    public class CallbackTests
    {
        [Fact]
        public void LoadSimpleCallback()
        {

            var stream = this.GetType().Assembly.GetManifestResourceStream("OpenApiTests.Samples.CallbackSample.yaml");
            var parser = new OpenApiParser();
            var openApiDoc = parser.Parse(stream);

            var path = openApiDoc.Paths.PathMap.First().Value;
            var operation = path.Operations.First().Value;

            var callback = operation.Callbacks.Items.First().Value.Operations.First();

            Assert.NotNull(callback);

        }

        [Fact]
        public void LoadSimpleCallbackWithRefs()
        {

            var stream = this.GetType().Assembly.GetManifestResourceStream("OpenApiTests.Samples.CallbackSampleWithRef.yaml");
            var parser = new OpenApiParser();
            var openApiDoc = parser.Parse(stream);

            var path = openApiDoc.Paths.PathMap.First().Value;
            var operation = path.Operations.First().Value;

            var callback = operation.Callbacks.Items.First();
            var cboperation = callback.Value.Operations.First();

            Assert.NotNull(callback);

        }

    }
}
