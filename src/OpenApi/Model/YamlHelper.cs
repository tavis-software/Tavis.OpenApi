using SharpYaml.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tavis.OpenApi.Model
{

    public static class YamlHelper
    {
        public static string GetScalarValue(this YamlMappingNode parentNode, YamlScalarNode key)
        {
            var scalarNode = parentNode.Children[key] as YamlScalarNode;
            if (scalarNode is null) throw new OpenApiParseException($"Expected scalar at line {parentNode.Start.Line} for key {key.Value}");

            return scalarNode.Value;
        }

        public static string GetScalarValue(this YamlNode node)
        {

            var scalarNode = node as YamlScalarNode;
            if (scalarNode is null) throw new OpenApiParseException($"Expected scalar at line {node.Start.Line}");

            return scalarNode.Value;
        }

        public static List<T> CreateList<T>(this YamlNode nodeList, Func<YamlMappingNode, T> map)
        {
            var yamlSequence = nodeList as YamlSequenceNode;
            if (yamlSequence is null) throw new OpenApiParseException($"Expected list at line {nodeList.Start.Line} while parsing {typeof(T).Name}");

            return yamlSequence.Select(n => map((YamlMappingNode)n)).ToList();
        }

        public static List<T> CreateSimpleList<T>(this YamlNode nodeList, Func<YamlScalarNode, T> map)
        {
            var yamlSequence = nodeList as YamlSequenceNode;
            if (yamlSequence is null) throw new OpenApiParseException($"Expected list at line {nodeList.Start.Line} while parsing {typeof(T).Name}");

            return yamlSequence.Select(n => map((YamlScalarNode)n)).ToList();
        }

        public static Dictionary<string, T> CreateMap<T>(this YamlNode nodeMap, Func<YamlMappingNode, T> map)
        {
            var yamlMap = nodeMap as YamlMappingNode;
            if (yamlMap is null) throw new OpenApiParseException($"Expected map at line {nodeMap.Start.Line} while parsing {typeof(T).Name}");
            var nodes = yamlMap.Select(n => new { key = n.Key.GetScalarValue(), value = map((YamlMappingNode)n.Value) });
            return nodes.ToDictionary(k => k.key, v => v.value);
        }

    }

    public class OpenApiParseException : Exception {
        public OpenApiParseException(string message) : base (message)
        {

        }
    }
}