using SharpYaml.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tavis.OpenApi.Model;

namespace Tavis.OpenApi
{

    public abstract class ParseNode
    {
        public ParseNode(ParsingContext ctx)
        {
            this.Context = ctx;
        }
        public ParsingContext Context { get; }

        public virtual string GetScalarValue()
        {
            throw new Exception();
        }
    }

    public class PropertyNode : ParseNode
    {
        public PropertyNode(ParsingContext ctx, string name, YamlNode node) : base(ctx)
        {
            this.Name = name;
            var listNode = node as YamlSequenceNode;
            if (listNode != null)
            {
                Value = new ListNode(Context, listNode);
            }
            else
            {
                var mapNode = node as YamlMappingNode;
                if (mapNode != null)
                {
                    Value = new MapNode(Context, mapNode);
                }
                else
                {
                    Value = new ValueNode(Context, node as YamlScalarNode);
                }
            }
        }
        public string Name { get; private set; }
        public ParseNode Value { get; private set; }


        public void ParseField<T>(
                            T parentInstance,
                            IDictionary<string, Action<T, ParseNode>> fixedFields,
                            IDictionary<Func<string, bool>, Action<T, string, ParseNode>> patternFields
            )
        {

            Action<T, ParseNode> fixedFieldMap;
            var found = fixedFields.TryGetValue(this.Name, out fixedFieldMap);

            if (fixedFieldMap != null)
            {
                fixedFieldMap(parentInstance, this.Value);
            }
            else
            {
                var map = patternFields.Where(p => p.Key(this.Name)).Select(p => p.Value).FirstOrDefault();
                if (map != null)
                {
                    map(parentInstance, this.Name, this.Value);
                }
            }
        }

    }

    public class RootNode : ParseNode
    {
        YamlDocument yamlDocument;
        public RootNode(ParsingContext ctx, Stream stream) : base(ctx)
        {
            var yamlStream = new YamlStream();

            yamlStream.Load(new StreamReader(stream));
            this.yamlDocument = yamlStream.Documents.First();
        }

        public MapNode GetMap()
        {
            return new MapNode(Context, (YamlMappingNode)yamlDocument.RootNode);
        }
    }

    public class ValueNode : ParseNode
    {
        YamlScalarNode node;
        public ValueNode(ParsingContext ctx, YamlScalarNode scalarNode) : base(ctx)
        {
            this.node = scalarNode;
        }

        public string GetScalarValue()
        {

            var scalarNode = this.node as YamlScalarNode;
            if (scalarNode is null) throw new OpenApiParseException($"Expected scalar at line {node.Start.Line}");

            return scalarNode.Value;
        }

    }
    public class ListNode : ParseNode
    {
        YamlSequenceNode nodeList;
        public ListNode(ParsingContext ctx, YamlSequenceNode sequenceNode) : base(ctx)
        {
            nodeList = sequenceNode;
        }

        public List<T> CreateList<T>(Func<YamlMappingNode, T> map)
        {
            var yamlSequence = nodeList as YamlSequenceNode;
            if (yamlSequence is null) throw new OpenApiParseException($"Expected list at line {nodeList.Start.Line} while parsing {typeof(T).Name}");

            return yamlSequence.Select(n => map((YamlMappingNode)n)).ToList();
        }

        public List<T> CreateSimpleList<T>(Func<YamlScalarNode, T> map)
        {
            var yamlSequence = nodeList as YamlSequenceNode;
            if (yamlSequence is null) throw new OpenApiParseException($"Expected list at line {nodeList.Start.Line} while parsing {typeof(T).Name}");

            return yamlSequence.Select(n => map((YamlScalarNode)n)).ToList();
        }

    }
    public class MapNode : ParseNode, IEnumerable<PropertyNode>
    {
        YamlMappingNode node;
        private List<PropertyNode> nodes;
        public MapNode(ParsingContext ctx, YamlMappingNode node) : base(ctx)
        {
            this.node = node;
            nodes = this.node.Children.Select(kvp => new PropertyNode(Context, kvp.Key.GetScalarValue(), kvp.Value)).ToList();
        }

        public IEnumerator<PropertyNode> GetEnumerator()
        {
            return this.nodes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.nodes.GetEnumerator();
        }

        public string GetScalarValue(ValueNode key)
        {
            var scalarNode = this.node.Children[new YamlScalarNode(key.GetScalarValue())] as YamlScalarNode;
            if (scalarNode is null) throw new OpenApiParseException($"Expected scalar at line {this.node.Start.Line} for key {key.GetScalarValue()}");

            return scalarNode.Value;
        }

        public Dictionary<string, T> CreateMap<T>(Func<YamlMappingNode, T> map)
        {
            var yamlMap = this.node;
            if (yamlMap is null) throw new OpenApiParseException($"Expected map at line {yamlMap.Start.Line} while parsing {typeof(T).Name}");
            var nodes = yamlMap.Select(n => new { key = n.Key.GetScalarValue(), value = map((YamlMappingNode)n.Value) });
            return nodes.ToDictionary(k => k.key, v => v.value);
        }

    }
}
