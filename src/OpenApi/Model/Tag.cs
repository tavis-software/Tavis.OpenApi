using System;
using SharpYaml.Serialization;

namespace Tavis.OpenApi.Model
{
    public class Tag : IReference
    {
        public string Name { get; set; }
        public string Description { get; set; }

        string IReference.Pointer
        {
            get; set;
        }

        internal static Tag LoadByReference(ParseNode node)
        {
            var tagName = node.GetScalarValue();
            var context = node.Context;
            var tagObject = (Tag)context.GetReferencedObject($"#/tags/{tagName}");

            if (tagObject == null)
            {
                tagObject = new Tag() { Name = tagName };
            }
            return tagObject;
        }

        public void Write(IParseNodeWriter writer)
        {
            writer.WriteStartMap();
            writer.WriteStringProperty("name", Name);
            writer.WriteStringProperty("description", Description);
            writer.WriteEndMap();
        }

        public static void Write(IParseNodeWriter writer, Tag tag)
        {
            tag.Write(writer);
        }
    }
}