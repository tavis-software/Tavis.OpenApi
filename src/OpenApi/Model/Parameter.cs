using System;
using SharpYaml.Serialization;
using System.Collections.Generic;

namespace Tavis.OpenApi.Model
{


    public enum InEnum
    {
        path,
        query,
        header
    }

    public class Parameter
    {
        public string Name { get; set; }
        public InEnum In { get; set; }  
        public string Description { get; set; }
        public bool Required { get; set; }
        public bool Deprecated { get; set; }
        public string Type { get; set; }

        public string Format { get; set; } 
        public bool AllowReserved { get; set; }
        public string Style { get; set; }
        public Dictionary<string, string> Extensions { get; set; }


        private static FixedFieldMap<Parameter> fixedFields = new FixedFieldMap<Parameter>
        {
            { "name", (o,n) => { o.Name = n.GetScalarValue(); } },
            { "in", (o,n) => { o.In = (InEnum)Enum.Parse(typeof(InEnum), n.GetScalarValue()); } },
            { "description", (o,n) => { o.Description = n.GetScalarValue(); } },
            { "required", (o,n) => { o.Required = bool.Parse(n.GetScalarValue()); } },
            { "deprecated", (o,n) => { o.Deprecated = bool.Parse(n.GetScalarValue()); } },
            { "allowReserved", (o,n) => { o.AllowReserved = bool.Parse(n.GetScalarValue()); } },
            { "type", (o,n) => { o.Type = n.GetScalarValue(); } },
            { "format", (o,n) => { o.Type = n.GetScalarValue(); } },
            { "style", (o,n) => { o.Style = n.GetScalarValue(); } },

        };

        private static PatternFieldMap<Parameter> patternFields = new PatternFieldMap<Parameter>
        {
            { (s)=> s.StartsWith("x-"), (o,k,n)=> o.Extensions.Add(k, n.GetScalarValue()) },
        };


        public static Parameter Load(YamlMappingNode value)
        {
            var parameter = new Parameter();

            foreach (var node in value.Children)
            {
                var key = (YamlScalarNode)node.Key;
                ParseHelper.ParseField(key.Value, node.Value, parameter, fixedFields, patternFields);
            }

            return parameter;
        }
    }
}