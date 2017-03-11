using System;
using System.Collections.Generic;
using SharpYaml.Serialization;

namespace Tavis.OpenApi.Model
{

    public class Contact
    {
        public string Name { get; set; }
        public Uri Url { get; set; }
        public string Email { get; set; }
        public Dictionary<string,string> Extensions { get; set; }


        public Contact()
        {
            Extensions = new Dictionary<string, string>();
        }


        public static Contact Load(ParseNode node)
        {
            var contactNode = node as MapNode;
            if (contactNode == null)
            {
                throw new Exception("Contact node should be a map");
            }
            var contact = new Contact();

            foreach (var propertyNode in contactNode)
            {
                propertyNode.ParseField(contact, OpenApiParser.ContactFixedFields, OpenApiParser.ContactPatternFields);
            }

            return contact;
        }
    }
}