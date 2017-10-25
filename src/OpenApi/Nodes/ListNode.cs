using SharpYaml.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SharpYaml;

namespace Tavis.OpenApi
{
    public class ListNode : ParseNode, IEnumerable<ParseNode>
    {
        YamlSequenceNode nodeList;
        ParsingContext context;
        public ListNode(ParsingContext ctx, YamlSequenceNode sequenceNode) : base(ctx)
        {
            this.context = ctx;
            nodeList = sequenceNode;
        }

        public override List<T> CreateList<T>(Func<MapNode, T> map)
        {
            var yamlSequence = nodeList as YamlSequenceNode;
            if (yamlSequence == null) throw new DomainParseException($"Expected list at line {nodeList.Start.Line} while parsing {typeof(T).Name}");

            return yamlSequence.Select(n => map(new MapNode(this.context,n as YamlMappingNode))).Where(i => i != null).ToList();
        }

        public override List<T> CreateSimpleList<T>(Func<ValueNode, T> map)
        {
            var yamlSequence = this.nodeList as YamlSequenceNode;
            if (yamlSequence == null) throw new DomainParseException($"Expected list at line {nodeList.Start.Line} while parsing {typeof(T).Name}");

            return yamlSequence.Select(n => map(new ValueNode(this.Context,(YamlScalarNode)n))).ToList();
        }

        public IEnumerator<ParseNode> GetEnumerator()
        {
            return nodeList.Select(n => ParseNode.Create(this.Context,n)).ToList().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public override void Write(IParseNodeWriter writer)
        {
            if (nodeList == null || !nodeList.Children.Any()) return;

            writer.WriteStartList();

            YamlNode yamlNode = nodeList.Children.First();

            if (yamlNode is YamlScalarNode)
            {
                YamlScalarNode scalarNode = (YamlScalarNode) yamlNode;

                bool stringFormat = scalarNode.Style == ScalarStyle.SingleQuoted ||
                                    scalarNode.Style == ScalarStyle.DoubleQuoted;

                var items = nodeList.Children.Select(x => (x as YamlScalarNode).Value).ToList();
                items.ForEach(s =>
                {
                    if (stringFormat)
                        writer.WriteListItem(s, (nodeWriter, item) => nodeWriter.WriteValue(item));
                    else
                        writer.WriteListItem((object)s, (nodeWriter, item) => nodeWriter.WriteValue(item));
                });
            }
            else if (yamlNode is YamlSequenceNode || yamlNode is YamlMappingNode)
            {
                var items = nodeList.Children.ToList();
                items.ForEach(x =>
                {
                    writer.WriteListItem(x,
                        (nodeWriter, item) => new AnyNode(Create(new ParsingContext(), x)).Write(nodeWriter));
                });
            }

            writer.WriteEndList();
        }
    }


}
