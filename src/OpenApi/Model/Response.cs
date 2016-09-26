using System;
using SharpYaml.Serialization;

namespace Tavis.OpenApi.Model
{

    public class Response
    {
        public static Response Load(YamlMappingNode node)
        {
            var response = new Response();
            return response;
        }
    }
}