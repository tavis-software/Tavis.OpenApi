using System;
using System.Collections.Generic;

namespace Tavis.OpenApi.Model
{
    public class Paths : IModel
    {
        public IDictionary<string, PathItem> PathItems { get; set; } = new Dictionary<string, PathItem>();
        public Dictionary<string, AnyNode> Extensions { get; set; } = new Dictionary<string, AnyNode>();
        
        public PathItem GetPath(string key)
        {
            return PathItems[key];
        }
        internal void Validate(List<string> errors)
        {
            //TODO:
        }

        void IModel.Write(IParseNodeWriter writer)
        {
            writer.WriteStartMap();
            foreach (var pathItem in PathItems)
            {
                writer.WritePropertyName(pathItem.Key);
                pathItem.Value.Write(writer);
            }
            writer.WriteEndMap();
        }

    }
}
