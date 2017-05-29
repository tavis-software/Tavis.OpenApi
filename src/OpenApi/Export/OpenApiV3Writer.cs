using SharpYaml.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tavis.OpenApi.Export;
using Tavis.OpenApi.Model;

namespace Tavis.OpenApi
{
    public class OpenApiV3Writer
    {
        OpenApiDocument document;
        public OpenApiV3Writer(OpenApiDocument document)
        {
            this.document = document;
        }

        public void Writer(Stream stream)
        {
            var writer = new YamlParseNodeWriter(stream);
            writer.WriteStartDocument();
            this.document.Write(writer);
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
                        writer.WriteValue(null);
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
        public static void WriteBoolProperty(this IParseNodeWriter writer, string name, bool value)
        {
            writer.WritePropertyName(name);
            writer.WriteValue(value);
        }

        public static void WriteNumberProperty(this IParseNodeWriter writer, string name, decimal value)
        {
            writer.WritePropertyName(name);
            writer.WriteValue(value);
        }

        public static void WriteNumberProperty(this IParseNodeWriter writer, string name, int value)
        {
            writer.WritePropertyName(name);
            writer.WriteValue(value);
        }
    }
}
