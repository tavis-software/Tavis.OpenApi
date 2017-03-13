using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tavis.OpenApi.Export
{
    public class YamlParseNodeWriter : IParseNodeWriter
    {

        StreamWriter writer;

        public YamlParseNodeWriter(Stream stream)
        {
            this.writer = new StreamWriter(stream);
        }

        public void Flush()
        {
            this.writer.Flush();
        }

        string Indent = "";
        bool inList = false;
        void IncreaseIndent()
        {
            Indent += "  ";
        }
        void DecreaseIndent()
        {
            Indent = Indent.Substring(0, Indent.Length - 2);
        }


        public void WriteEndDocument()
        {
        }

        public void WriteEndList()
        {
            inList = false;
            DecreaseIndent();
        }

        public void WriteEndMap()
        {
            DecreaseIndent();
        }

        public void WritePropertyName(string name)
        {
            writer.Write(Indent + name + ": ");

        }

        public void WriteStartDocument()
        {
        }

        public void WriteStartList()
        {
            if (inList) writer.Write(Indent + "- ");
            IncreaseIndent();
            inList = true;
            writer.WriteLine();
        }

        public void WriteStartMap()
        {
            if (inList) writer.Write(Indent + "- ");
            IncreaseIndent();
            writer.WriteLine();
        }

        public void WriteValue(string value)
        {
            if (inList) writer.Write(Indent + "- ");
            writer.WriteLine( value );
        }

        public void WriteValue(Decimal value)
        {
            writer.WriteLine(value.ToString());  //TODO deal with culture issues
        }

        public void WriteValue(int value)
        {
            writer.WriteLine(value.ToString());  //TODO deal with culture issues
        }

        public void WriteValue(bool value)
        {
            writer.WriteLine(value.ToString().ToLower());  //TODO deal with culture issues
        }
    }
}
