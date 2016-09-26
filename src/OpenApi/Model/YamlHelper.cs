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
            var childNode = (YamlScalarNode)parentNode.Children[key];
            return childNode.Value;
        }

        public static string GetScalarValue(this YamlNode node)
        {
            var scalarNode = (YamlScalarNode)node;
            return scalarNode.Value;
        }

        public static List<T> CreateList<T>(this YamlNode nodeList, Func<YamlMappingNode, T> map)
        {
            return ((YamlSequenceNode)nodeList).Select(n => map((YamlMappingNode)n)).ToList();
        }

        public static Dictionary<string, T> CreateMap<T>(this YamlNode nodeMap, Func<YamlMappingNode, T> map)
        {
            return ((YamlMappingNode)nodeMap).Select(n => new { key = n.Key.GetScalarValue(), value = map((YamlMappingNode)n.Value) }).ToDictionary(k => k.key, v => v.value);
        }

    }
}