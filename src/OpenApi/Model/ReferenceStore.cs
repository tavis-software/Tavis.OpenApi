using System.Collections.Generic;

namespace Tavis.OpenApi.Model
{

    public class ReferenceStore : IReferenceStore
    {
        private Dictionary<string, object> store = new Dictionary<string, object>();
        public object GetReferencedObject(string pointer)
        {
            object returnValue = null; 
            store.TryGetValue(pointer, out returnValue);
            return returnValue;
        }
    }


}
