using System;
using SharpYaml.Serialization;

namespace Tavis.OpenApi.Model
{

    public class Parameter
    {
        public static Parameter Load(YamlMappingNode node)
        {
            var parameter = new Parameter();

            return parameter;
        }
    }
}