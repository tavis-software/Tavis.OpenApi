using System;
using System.Collections.Generic;
using System.Linq;
using Tavis.OpenApi.Model;

namespace Tavis.OpenApi
{


    public class ParsingContext
    {
        public string Version { get; set; }
        public List<OpenApiError> ParseErrors { get; set; } = new List<OpenApiError>();

        private Dictionary<string, IReference> referenceStore = new Dictionary<string, IReference>();

        private Func<string, IReference> referenceLoader;

        private Stack<string> currentLocation = new Stack<string>();
        public ParsingContext(Func<string,IReference> referenceLoader)
        {
            this.referenceLoader = referenceLoader;
        }

        internal void StartObject(string objectName)
        {
            this.currentLocation.Push(objectName);
        }

        internal void EndObject()
        {
            this.currentLocation.Pop();
        }
        public string GetLocation() {
            return "#/" + String.Join("/", this.currentLocation.Reverse().ToArray());
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
