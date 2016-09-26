using SharpYaml.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tavis.OpenApi.Model
{
    public class RequestBody
    {
        public static RequestBody Load(YamlMappingNode node)
        {
            var requestBody = new RequestBody();

            return requestBody;
        }
    }
}
