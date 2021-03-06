﻿
using SharpYaml;
using SharpYaml.Serialization;

namespace Tavis.OpenApi
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    public static class WriterExtensions {


        public static void WriteObject<T>(this IParseNodeWriter writer, string propertyName, T entity, Action<IParseNodeWriter, T> parser)
        {
            if (entity != null)
            {
                writer.WritePropertyName(propertyName);
                parser(writer, entity);
            }

        }

        public static void WriteList<T>(this IParseNodeWriter writer, string propertyName, IList<T> list, Action<IParseNodeWriter, T> parser)
        {
            if (list != null && list.Any())
            {
                writer.WritePropertyName(propertyName);
                writer.WriteStartList();
                foreach (var item in list)
                {
                    writer.WriteListItem(item, parser);
                }
                writer.WriteEndList();
            }

        }

        public static void WriteMap<T>(this IParseNodeWriter writer, string propertyName, IDictionary<string, T> list, Action<IParseNodeWriter, T> parser)
        {
            if (list != null && list.Count() > 0)
            {
                writer.WritePropertyName(propertyName);
                writer.WriteStartMap();
                foreach (var item in list)
                {
                    writer.WritePropertyName(item.Key);
                    if (item.Value != null)
                    {
                        parser(writer, item.Value);
                    }
                    else
                    {
                        writer.WriteNull();
                    }
                }
                writer.WriteEndMap();
            }

        }

        public static void WriteStringProperty(this IParseNodeWriter writer, string name, string value)
        {
            if (!String.IsNullOrEmpty(value))
            {
                writer.WritePropertyName(name);
                writer.WriteValue(value);
            }
        }
        public static void WriteBoolProperty(this IParseNodeWriter writer, string name, bool value, bool? defaultValue = null)
        {
            if (defaultValue == null || value != defaultValue)
            {
                writer.WritePropertyName(name);
                writer.WriteValue(value);
            }
        }

        public static void WriteNumberProperty(this IParseNodeWriter writer, string name, decimal value, decimal? defaultValue = null)
        {
            if (defaultValue == null || value != defaultValue)
            {
                writer.WritePropertyName(name);
                writer.WriteValue(value);
            }
        }

        public static void WriteNumberProperty(this IParseNodeWriter writer, string name, int? value)
        {
            if (value != null)
            {
                writer.WritePropertyName(name);
                writer.WriteValue((int)value);
            }
        }

        public static void WriteNumberProperty(this IParseNodeWriter writer, string name, decimal? value)
        {
            if (value != null)
            {
                writer.WritePropertyName(name);
                writer.WriteValue((decimal)value);
            }
        }

        public static void WriteExtensions(this IParseNodeWriter writer, Dictionary<string, string> extensions)
        {
            if (extensions != null && extensions.Count > 0)
            {
                foreach (KeyValuePair<string, string> extension in extensions)
                {
                    writer.WritePropertyName(extension.Key);
                    writer.WriteValue(extension.Value);
                }
            }
        }

        public static void WriteExtensions(this IParseNodeWriter writer, Dictionary<string, AnyNode> extensions)
        {
            if (extensions != null && extensions.Count > 0)
            {
                foreach (KeyValuePair<string, AnyNode> extension in extensions)
                {
                    writer.WritePropertyName(extension.Key);
                    extension.Value.Write(writer);   
                }
            }
        }
    }
}
