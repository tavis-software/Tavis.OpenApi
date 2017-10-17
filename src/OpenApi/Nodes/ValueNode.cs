using SharpYaml;
using SharpYaml.Serialization;

namespace Tavis.OpenApi
{
    public class ValueNode : ParseNode
    {
        YamlScalarNode node;
        public ValueNode(ParsingContext ctx, YamlScalarNode scalarNode) : base(ctx)
        {
            this.node = scalarNode;
        }

        public override string GetScalarValue()
        {

            var scalarNode = this.node as YamlScalarNode;
            if (scalarNode == null) throw new DomainParseException($"Expected scalar at line {node.Start.Line}");

            return scalarNode.Value;
        }

        public override void Write(IParseNodeWriter writer)
        {
            if (node.Style == ScalarStyle.DoubleQuoted || node.Style == ScalarStyle.SingleQuoted)
            {
                writer.WriteValue(node.Value);
                return;
            }

            writer.WriteValue((object)node.Value);
        }
    }
}
