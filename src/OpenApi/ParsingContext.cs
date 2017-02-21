using System;
using System.Collections.Generic;

namespace Tavis.OpenApi
{


    public class ParsingContext
    {
        public string Version { get; set; }
        public List<OpenApiError> ParseErrors { get; set; } = new List<OpenApiError>();

        private Dictionary<string, object> referenceStore = new Dictionary<string, object>();

        private Func<string, object> referenceLoader;
        public ParsingContext(Func<string,object> referenceLoader)
        {
            this.referenceLoader = referenceLoader;
        }

        public object GetReferencedObject(string pointer)
        {
            object returnValue = null;
            referenceStore.TryGetValue(pointer, out returnValue);

            if (returnValue == null)
            {
                returnValue = this.referenceLoader(pointer);
                referenceStore.Add(pointer, returnValue);
            }

            return returnValue;
        }

        
    }


}
