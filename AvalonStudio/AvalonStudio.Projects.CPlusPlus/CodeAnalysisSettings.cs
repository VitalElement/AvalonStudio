using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AvalonStudio.Projects.CPlusPlus
{
    public class CodeAnalysisSettings
    {
        public CodeAnalysisSettings()
        {
            Enabled = true;
        }

        [DefaultValue(true)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool Enabled { get; set; }
    }
}
