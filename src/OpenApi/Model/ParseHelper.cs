using SharpYaml.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tavis.OpenApi.Model
{

    public static class ParseHelper
    {
        

        public static void ParseField<T>(string key,
                            YamlNode currentNode,
                            T parentInstance,
                            IDictionary<string, Action<T, YamlNode>> fixedFields,
                            IDictionary<Func<string, bool>, Action<T, string, YamlNode>> patternFields
            )
        {
            
            Action<T, YamlNode> fixedFieldMap;
            var found = fixedFields.TryGetValue(key, out fixedFieldMap);

            if (fixedFieldMap != null)
            {
                fixedFieldMap(parentInstance, currentNode);
            }
            else
            {
                var map = patternFields.Where(p => p.Key(key)).Select(p => p.Value).FirstOrDefault();
                if (map != null)
                {
                    map(parentInstance, key, currentNode);
                }
            }
        }
    }
}