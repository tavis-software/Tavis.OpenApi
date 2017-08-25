
namespace Tavis.OpenApi
{
    using SharpYaml.Serialization;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Tavis.OpenApi.Export;
    using Tavis.OpenApi.Model;

    public interface IOpenApiWriter {
        void Write(Stream stream, OpenApiDocument document);
    }

    public class OpenApiV3Writer : IOpenApiWriter
    {
        Func<Stream, IParseNodeWriter> defaultWriterFactory = s => new YamlParseNodeWriter(s);
        Func<Stream, IParseNodeWriter> writerFactory;

        public OpenApiV3Writer(Func<Stream, IParseNodeWriter> writerFactory = null)
        {
            this.writerFactory = writerFactory ?? defaultWriterFactory;
        }

        public void Write(Stream stream, OpenApiDocument document)
        {

            var writer = writerFactory(stream);
            writer.WriteStartDocument();
            ModelHelper.Write(writer,document);
            writer.WriteEndDocument();
            writer.Flush();
        }

    }

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
            if (list != null && list.Count() > 0)
            {
                writer.WritePropertyName(propertyName);
                foreach (var item in list)
                {
                    writer.WriteStartList();
                    parser(writer, item);
                    writer.WriteEndList();
                }
            }

        }

        public static void WriteMap<T>(this IParseNodeWriter writer, string propertyName, IDictionary<string, T> list, Action<IParseNodeWriter, T> parser)
        {
            if (list != null && list.Count() > 0)
            {
                writer.WritePropertyName(propertyName);
                foreach (var item in list)
                {
                    writer.WriteStartMap();
                    writer.WritePropertyName(item.Key);
                    if (item.Value != null)
                    {
                        parser(writer, item.Value);
                    } else
                    {
                        writer.WriteNull();
                    }
                    writer.WriteEndMap();
                }
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
        public static void WriteNumberProperty(this IParseNodeWriter writer, string name, int value, int? defaultValue = null)
        {
            if (defaultValue == null || value != defaultValue)
            {
                writer.WritePropertyName(name);
                writer.WriteValue(value);
            }
        }
    }
}
