using SharpYaml.Serialization;
using System;

namespace Tavis.OpenApi.Model
{

    public class ExternalDocs
    {
        public string Description { get; set; }
        public Uri Url { get; set; }



        public void Write(IParseNodeWriter writer)
        {

        }

        public static void Write(IParseNodeWriter writer, ExternalDocs externalDocs)
        {
            externalDocs.Write(writer);
        }

    }
}