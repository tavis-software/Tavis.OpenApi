using SharpYaml.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections;

namespace Tavis.OpenApi.Model
{

    
    public class OpenApiDocument
    {
        public string Version { get; set; } // Swagger

        public Info Info { get; set; } = new Info();
        public List<Server> Servers { get; set; } = new List<Server>();
        public List<SecurityRequirement> SecurityRequirements { get; set; }

        public Paths Paths { get; set; }
        public Components Components { get; set; } = new Components();
        public List<Tag> Tags { get; set; } = new List<Tag>();
        public ExternalDocs ExternalDocs { get; set; } = new ExternalDocs();
        public Dictionary<string, string> Extensions { get; set; } = new Dictionary<string, string>();

        public List<string> ParseErrors { get; set; } = new List<string>();

 
        private static FixedFieldMap<OpenApiDocument> fixedFields = new FixedFieldMap<OpenApiDocument> {
            { "openapi", (o,n) => { o.Version = n.GetScalarValue(); } },
            { "info", (o,n) => o.Info = Info.Load((YamlMappingNode)n) },
            { "paths", (o,n) => o.Paths = Paths.Load((YamlMappingNode)n) },
            { "components", (o,n) => o.Components = Components.Load((YamlMappingNode)n) },
            { "tags", (o,n) => o.Tags = n.CreateList(Tag.Load)},
            { "externalDocs", (o,n) => o.ExternalDocs = ExternalDocs.Load((YamlMappingNode)n) },
            { "security", (o,n) => o.SecurityRequirements = n.CreateList(SecurityRequirement.Load)}

            };

        private static PatternFieldMap<OpenApiDocument> patternFields = new PatternFieldMap<OpenApiDocument>
        {
                   { (s)=> s.StartsWith("x-"), (o,k,n)=> o.Extensions.Add(k, n.GetScalarValue()) }
        };


        public static OpenApiDocument Parse(Stream stream)
        {
            var rootNode = new RootNode(stream);
            return Load(rootNode);
        }

        public static OpenApiDocument Load(RootNode rootNode)
        {
            var openApidoc = new OpenApiDocument();

            var rootMap = rootNode.GetMap();
            
            foreach (var node in rootMap)
            {
                ParseHelper.ParseField<OpenApiDocument>(node.Name, node.Value, openApidoc, OpenApiDocument.fixedFields, OpenApiDocument.patternFields);
            }

            openApidoc.ParseErrors = openApidoc.Validate();
            return openApidoc;
        }

        public List<string> Validate()
        {
            var errors = new List<string>();

            Validate(errors);
            return errors;
        }

        private Regex versionRegex = new Regex(@"\d+\.\d+\.\d+");

        private void Validate(List<string> errors)
        {
            if (!versionRegex.IsMatch(Version))
            {
                errors.Add("`openapi` property does not match the required format major.minor.patch");
            }

            Info.Validate(errors);

            if (Paths == null)
            {
                errors.Add("`paths` is a required property");
            } else
            {
                Paths.Validate(errors);
            }

        }
    }

    
    public class FixedFieldMap<T> : Dictionary<string, Action<T, ParseNode>>
    {
   
    }

    public class PatternFieldMap<T> : Dictionary<Func<string, bool>, Action<T, string, ParseNode>>
    {
    }

    public class ParsingContext
    {

    }

    public abstract class ParseNode
    {
        public ParseNode(ParsingContext ctx)
        {
            this.Context = ctx;
        }
        public ParsingContext Context { get; }
    }

    public class PropertyNode : ParseNode
    {
        public PropertyNode(ParsingContext ctx, string name, YamlNode node) : base(ctx)
        {
            this.Name = name;
            var listNode = node as YamlSequenceNode;
            if (listNode != null)
            {
                Value = new ListNode(Context,listNode);
            } else
            {
                var mapNode = node as YamlMappingNode;
                if (mapNode != null)
                {
                    Value = new MapNode(Context,mapNode);
                } else
                {
                    Value = new ValueNode(Context,node as YamlScalarNode);
                }
            } 
        }
        public string Name { get; private set; }
        public ParseNode Value { get; private set; }
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
            return new MapNode(Context,(YamlMappingNode)yamlDocument.RootNode);
        }
    }

    public class ValueNode : ParseNode
    {
        public ValueNode(ParsingContext ctx, YamlScalarNode scalarNode) : base(ctx)
        {

        }
    }
    public class ListNode :  ParseNode
    {
        public ListNode(ParsingContext ctx, YamlSequenceNode sequenceNode) : base(ctx)
        {

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
    }

}
