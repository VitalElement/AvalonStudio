using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AvalonStudio.Utils
{
    public class SerializedObject
	{
		public static void Serialize(string filename, object item)
		{
            using (var writer =File.OpenWrite(filename))
            {
                throw new System.Exception("Not compatible .net core");
                /*writer.Write(JsonConvert.SerializeObject(item, Formatting.Indented,
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        Converters = new[] { new StringEnumConverter() }
                    }));*/
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
	}
}