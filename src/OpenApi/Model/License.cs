﻿using System;
using System.Collections.Generic;

namespace Tavis.OpenApi.Model
{
    public class License : IModel
    {
        public string Name { get; set; }
        public Uri Url { get; set; }
        public Dictionary<string, string> Extensions { get; set; } = new Dictionary<string, string>();
        
        void IModel.Write(IParseNodeWriter writer)
        {
            writer.WriteStartMap();

            writer.WriteStringProperty("name", Name);
            writer.WriteStringProperty("url", Url?.OriginalString);

            writer.WriteEndMap();
        }

        
    }
}