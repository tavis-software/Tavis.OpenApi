using System;
using System.Collections.Generic;
using Tavis.OpenApi.Model;

namespace Tavis.OpenApi
{


    public class ParsingContext
    {
        public string Version { get; set; }
        public List<OpenApiError> ParseErrors { get; set; } = new List<OpenApiError>();

        private Dictionary<string, IReference> referenceStore = new Dictionary<string, IReference>();

        private Func<string, IReference> referenceLoader;
        public ParsingContext(Func<string,IReference> referenceLoader)
        {
            this.referenceLoader = referenceLoader;
        }

        public IReference GetReferencedObject(string pointer)
        {
            IReference returnValue = null;
            referenceStore.TryGetValue(pointer, out returnValue);

            if (returnValue == null)
            {
                returnValue = this.referenceLoader(pointer);
                returnValue.Pointer = pointer;
                referenceStore.Add(pointer, returnValue);
            }

            return returnValue;
        }

        
    }


}
