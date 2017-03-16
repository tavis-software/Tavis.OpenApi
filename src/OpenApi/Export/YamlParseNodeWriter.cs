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
        enum State
        {
            InDocument,
            InList,
            InMap, 
            InProperty
        }

        Stack<State> state = new Stack<State>();
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
            state.Pop();
        }

        public void WriteEndList()
        {
            state.Pop();
            DecreaseIndent();
        }

        public void WriteEndMap()
        {
            state.Pop();
            DecreaseIndent();
        }

        private bool InList()
        {
            return state.Peek() == State.InList;
        }
        public void WritePropertyName(string name)
        {
            writer.Write(Indent + name + ": ");
            state.Push(State.InProperty);
        }

        public void WriteStartDocument()
        {
            state.Push(State.InDocument);
        }

        public void WriteStartList()
        {
            if (InList()) writer.Write(Indent + "- ");
            IncreaseIndent();
            state.Push(State.InList);
            writer.WriteLine();

        }

        public void WriteStartMap()
        {
            if (InList())
            {
                writer.Write(Indent + "- ");
            }
            writer.WriteLine();
            IncreaseIndent();
            state.Push(State.InMap);
        }

        public void WriteValue(string value)
        {
            if (InList()) writer.Write(Indent + "- ");
            writer.WriteLine( value );
            state.Pop();
        }

        public void WriteValue(Decimal value)
        {
            if (InList()) writer.Write(Indent + "- ");
            writer.WriteLine(value.ToString());  //TODO deal with culture issues
            state.Pop();
        }

        public void WriteValue(int value)
        {
            if (state.Peek() == State.InList) writer.Write(Indent + "- ");
            writer.WriteLine(value.ToString());  //TODO deal with culture issues
            state.Pop();
        }

        public void WriteValue(bool value)
        {
            if (state.Peek() == State.InList) writer.Write(Indent + "- ");
            writer.WriteLine(value.ToString().ToLower());  //TODO deal with culture issues
            state.Pop();
        }
    }
}
