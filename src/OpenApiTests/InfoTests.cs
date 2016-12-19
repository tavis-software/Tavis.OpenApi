﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tavis.OpenApi.Model;
using Xunit;

namespace OpenApiTests.Samples
{
    public class InfoTests
    {


        [Fact]
        public void CheckPetStoreApiInfo()
        {
            var stream = this.GetType().Assembly.GetManifestResourceStream("OpenApiTests.Samples.petstore30.yaml");

            var openApiDoc = OpenApiDocument.Parse(stream);
            var info = openApiDoc.Info;
            Assert.Equal("Swagger Petstore (Simple)", openApiDoc.Info.Title);
            Assert.Equal("A sample API that uses a petstore as an example to demonstrate features in the swagger-2.0 specification", info.Description);
            Assert.Equal("1.0.0", info.Version);

        }


        [Fact]
        public void ParseCompleteHeaderOpenApi()
        {

            var stream = this.GetType().Assembly.GetManifestResourceStream("OpenApiTests.Samples.CompleteHeader.yaml");

            var openApiDoc = OpenApiDocument.Parse(stream);

            Assert.Equal("1.0.0", openApiDoc.Version);

            Assert.Equal(0, openApiDoc.Paths.PathMap.Count());
            Assert.Equal("The Api", openApiDoc.Info.Title);
            Assert.Equal("0.9.1", openApiDoc.Info.Version);
            Assert.Equal("This is an api", openApiDoc.Info.Description);
            Assert.Equal("Do what you want!", openApiDoc.Info.TermsOfService);
            Assert.Equal("Darrel Miller", openApiDoc.Info.Contact.Name);
            Assert.Equal("@darrel_miller", openApiDoc.Info.Contact.Extensions["x-twitter"]);
        }

    }
}
