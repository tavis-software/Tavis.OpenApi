using SharpYaml.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Tavis.OpenApi
{

    public static class YamlHelper
    {
        public static string GetScalarValue(this YamlNode node)
        {
            var scalarNode = node as YamlScalarNode;
            if (scalarNode == null) throw new DomainParseException($"Expected scalar at line {node.Start.Line}");

            return scalarNode.Value;
        }
    }

    public class DomainParseException : Exception
    {
        public DomainParseException(string message) : base(message)
        {

        }
    }
    public class FixedFieldMap<T> : Dictionary<string, Action<T, ParseNode>>
    {
    }

    public class PatternFieldMap<T> : Dictionary<Func<string, bool>, Action<T, string, ParseNode>>
    {
    }

    public interface IReferenceStore
    {
        object GetReferencedObject(string pointer);
    }

    public class ParsingContext
    {
        public string Version { get; set; }
        public List<OpenApiError> ParseErrors { get; set; } = new List<OpenApiError>();

        IReferenceStore referenceStore;
        public ParsingContext(IReferenceStore referenceStore)
        {
            this.referenceStore = referenceStore;
        }
        public Object GetReferencedObject(string pointer)
        {
            return this.referenceStore.GetReferencedObject(pointer);
        }

        internal object GetInstance(string pointer)
        {
            throw new NotImplementedException();
        }
    }
    public abstract class ParseNode
    {
        public ParseNode(ParsingContext ctx)
        {
            this.Context = ctx;
        }
        public ParsingContext Context { get; }

        public MapNode CheckMapNode(string nodeName)
        {
            var mapNode = this as MapNode;
            if (mapNode == null)
            {
                this.Context.ParseErrors.Add(new OpenApiError("", $"{nodeName} must be a map/object"));
            }

            return mapNode;
        }

        public static ParseNode Create(ParsingContext context, YamlNode node)
        {
            var listNode = node as YamlSequenceNode;
            if (listNode != null)
            {
                return new ListNode(context, listNode);
            }
            else
            {
                var mapNode = node as YamlMappingNode;
                if (mapNode != null)
                {
                    return new MapNode(context, mapNode);
                }
                else
                {
                    return new ValueNode(context, node as YamlScalarNode);
                }
            }
        }

        public virtual string GetScalarValue()
        {
            throw new Exception();
        }

        public virtual Dictionary<string, T> CreateMap<T>(Func<MapNode, T> map)
        {
            throw new Exception();
        }
        public virtual List<T> CreateList<T>(Func<MapNode, T> map)
        {
            throw new Exception();
        }
        public virtual List<T> CreateSimpleList<T>(Func<ValueNode, T> map)
        {
            throw new Exception();
        }

        internal string CheckRegex(string value, Regex versionRegex, string defaultValue)
        {
            if (!versionRegex.IsMatch(value))
            {
                this.Context.ParseErrors.Add(new OpenApiError("", "Value does not match regex: " + versionRegex.ToString()));
                return defaultValue;
            }
            return value;
        }
    }

    public class PropertyNode : ParseNode
    {
        public PropertyNode(ParsingContext ctx, string name, YamlNode node) : base(ctx)
        {
            this.Name = name;
            Value = ParseNode.Create(ctx,node);
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
                try
                {
                    fixedFieldMap(parentInstance, this.Value);
                } catch (DomainParseException ex)
                {
                    this.Context.ParseErrors.Add(new OpenApiError(ex));
                }
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

        public override string GetScalarValue()
        {

            var scalarNode = this.node as YamlScalarNode;
            if (scalarNode == null) throw new DomainParseException($"Expected scalar at line {node.Start.Line}");

            return scalarNode.Value;
        }

    }
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

            return yamlSequence.Select(n => map(new MapNode(this.context,(YamlMappingNode)n))).ToList();
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
            if (scalarNode == null) throw new DomainParseException($"Expected scalar at line {this.node.Start.Line} for key {key.GetScalarValue()}");

            return scalarNode.Value;
        }

        public T CreateOrReferenceDomainObject<T>(Func<T> factory)
        {
            T domainObject;
            var refPointer = GetReferencePointer(); // What should the DOM of a reference look like?
            // Duplicated object - poor perf/more memory/unsynchronized changes
            // Intermediate object - require common base class/client code has to explicitly code for it.
            // Delegating object - lot of setup work/maintenance/ require full interfaces
            // **current favourite***Shared object - marker to indicate its a reference/serialization code must serialize as reference everywhere except components.
            if (refPointer != null)
            {
                domainObject = (T)this.Context.GetReferencedObject(refPointer);
            }
            else
            {
                domainObject = factory();
            }

            return domainObject;
        }

        public override Dictionary<string, T> CreateMap<T>(Func<MapNode, T> map)
        {
            var yamlMap = this.node;
            if (yamlMap == null) throw new DomainParseException($"Expected map at line {yamlMap.Start.Line} while parsing {typeof(T).Name}");
            var nodes = yamlMap.Select(n => new { key = n.Key.GetScalarValue(), value = map(new MapNode(this.Context,(YamlMappingNode)n.Value)) });
            return nodes.ToDictionary(k => k.key, v => v.value);
        }

        public string GetReferencePointer()
        {
            YamlNode refNode;

            if (!this.node.Children.TryGetValue(new YamlScalarNode("$ref"), out refNode))
            {
                return null;
            }
            return refNode.GetScalarValue();
        }
    }


}
