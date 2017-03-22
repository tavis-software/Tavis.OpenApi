using System.Collections.Generic;
using SharpYaml.Serialization;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Tavis.OpenApi.Model
{

    public class Info
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string TermsOfService
        {
            get { return this.termsOfService; }
            set
            {
                if (!Uri.IsWellFormedUriString(value, UriKind.RelativeOrAbsolute))
                {
                    throw new DomainParseException("`info.termsOfService` MUST be a URL");
                };
                this.termsOfService = value;
            }
        }
        string termsOfService;
        public Contact Contact { get; set; }
        public License License { get; set; }
        public string Version { get; set; }
        public Dictionary<string, string> Extensions { get; set; } = new Dictionary<string, string>();

        private static Regex versionRegex = new Regex(@"\d+\.\d+\.\d+");

        public void Write(IParseNodeWriter writer)
        {
            writer.WriteStartMap();

            writer.WriteStringProperty("title", Title);
            writer.WriteStringProperty("description", Description);
            writer.WriteStringProperty("termsOfService", TermsOfService);
            writer.WriteObject("contact", Contact, Contact.Write);
            writer.WriteObject("license", License, License.Write);
            writer.WriteStringProperty("version", Version);

            writer.WriteEndMap();
        }

        public static void Write(IParseNodeWriter writer, Info info)
        {
            info.Write(writer);
        }
    }

}
