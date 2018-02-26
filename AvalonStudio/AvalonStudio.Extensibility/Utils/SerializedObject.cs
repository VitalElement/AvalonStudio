using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.IO;

namespace AvalonStudio.Utils
{
    public class SerializedObject
    {
        private static JsonSerializerSettings DefaultSettings =
            new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Converters = new JsonConverter[] { new StringEnumConverter(), new VersionConverter() }
            };

        private static JsonSerializer DefaultSerializer = JsonSerializer.Create(DefaultSettings);

        public static void Serialize(string filename, object item)
        {
            using (var writer = File.CreateText(filename))
            {
                writer.Write(JsonConvert.SerializeObject(item, Formatting.Indented, DefaultSettings));
            }
        }

        public static T FromString<T>(string data)
        {
            return JsonConvert.DeserializeObject<T>(data);
        }

        public static T Deserialize<T>(string filename)
        {
            return JsonConvert.DeserializeObject<T>(File.ReadAllText(filename));
        }

        public static void PopulateObject(TextReader reader, object target)
        {
            DefaultSerializer.Populate(reader, target);
        }
    }
}