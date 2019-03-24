using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace AvalonStudio.Sdk.Build.Tasks
{
    public class GenerateExtensionManifest : Task
    {
        [Required]
        public string Source { get; set; }

        [Required]
        public string Target { get; set; }

        public override bool Execute()
        {
            if (!File.Exists(Source))
            {
                Log.LogError("The specified extension manifest path doesn't exist!");
                return false;
            }

            try
            {
                using (var reader = new StreamReader(File.OpenRead(Source)))
                {
                    using (var jsonReader = new JsonTextReader(reader))
                    {
                        var root = JObject.Load(jsonReader);

                        ValidateProperty(
                            root,
                            "name",
                            JTokenType.String,
                            t => !String.IsNullOrWhiteSpace((string)t));

                        ValidateProperty(
                            root,
                            "version",
                            JTokenType.String,
                            t => !String.IsNullOrWhiteSpace((string)t) && Version.TryParse((string)t, out _));

                        if (Log.HasLoggedErrors)
                        {
                            return false;
                        }
                    }
                }

                // todo: support tokens
                File.Copy(Source, Target, true);
                return true;
            }
            catch (Exception e)
            {
                Log.LogError(null, "AsSdk0003", null, null, 0, 0, 0, 0, e.ToString());
                return false;
            }
        }

        private void ValidateProperty(
            JObject parent,
            string name,
            JTokenType tokenType,
            Func<JToken, bool> validate)
        {
            var value = parent.Property(name)?.Value;

            if (value == null
                || value.Type != tokenType
                || !validate(value))
            {
                Log.LogError(
                    null, "AsSdk0002", null, null, 0, 0, 0, 0, $"Invalid value for extension property {name}!");
            }
        }
    }
}
