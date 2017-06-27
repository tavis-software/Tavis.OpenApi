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

        private Dictionary<string, object> tempStorage = new Dictionary<string, object>();

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
                if(previousPointers.Contains(pointer))
                {
                    return null; // Return reference object?
                }
                previousPointers.Push(pointer);
                returnValue = this.referenceLoader(pointer);
                previousPointers.Pop();
                if (returnValue != null)
                {
                    returnValue.Pointer = pointer;
                    referenceStore.Add(pointer, returnValue);
                } else
                {
                    ParseErrors.Add(new OpenApiError(this.GetLocation(), $"Cannot resolve $ref {pointer}"));
                }
            }

            return returnValue;
        }
        private Stack<string> previousPointers = new Stack<string>();

        public void SetTempStorage(string key, object value)
        {
            this.tempStorage[key] = value;
        }
        public T GetTempStorage<T>(string key)  where T:class
        {
            object value;
            if (this.tempStorage.TryGetValue(key,out value))
            {
                return (T)value;
            }
            return null;
        }
    }


}
