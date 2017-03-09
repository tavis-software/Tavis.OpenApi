﻿using System;
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

        internal static Tag Load(ParseNode n)
        {
            var mapNode = n.CheckMapNode("tag");

            var obj = new Tag();

            foreach (var node in mapNode)
            {
                var key = node.Name;
                switch (key)
                {
                    case "description":
                        obj.Description = node.Value.GetScalarValue();
                        break;
                    case "name":
                        obj.Name = node.Value.GetScalarValue();
                        break;

                }
            }
            return obj;
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
    }
}