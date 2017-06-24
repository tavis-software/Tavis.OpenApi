using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tavis.OpenApi.Model
{
    public interface IModel
    {
        void Write(IParseNodeWriter writer);
    }

    public static class ModelHelper
    {
        public static void WriteFull(IParseNodeWriter writer, IModel model)
        {
            model.Write(writer);
        }
        public static void Write(IParseNodeWriter writer, IModel model)
        {
            var referencable = model as IReference;
            if (referencable != null && referencable.IsReference())
            {
                referencable.WriteRef(writer);
            }
            else
            {
                model.Write(writer);
            }
        }
    }
}
