namespace AvalonStudio.Utils
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System.IO;

    public class SerializedObject<T>
    {
        public void Serialize(string filename)
        {
            var writer = new StreamWriter(filename);
            writer.Write(JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, Converters = new[] { new StringEnumConverter() } }));
            writer.Close();
        }

        public static T FromString (string data)
        {
            return JsonConvert.DeserializeObject<T>(data);
        }

        public static T Deserialize (string filename)
        {
            var reader = new StreamReader(filename);

            var result = JsonConvert.DeserializeObject<T>(reader.ReadToEnd());

            reader.Close();

            return result;
        }
    }
}
