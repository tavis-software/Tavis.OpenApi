using SharpYaml.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tavis.OpenApi.Model
{

    public static class YamlHelper
    {
        public static string GetScalarValue(this YamlNode node)
        {
            var scalarNode = node as YamlScalarNode;
            if (scalarNode == null) throw new OpenApiParseException($"Expected scalar at line {node.Start.Line}");

            return scalarNode.Value;
        }
    }

    public class OpenApiParseException : Exception {
        public OpenApiParseException(string message) : base (message)
        {

        }
    }
}