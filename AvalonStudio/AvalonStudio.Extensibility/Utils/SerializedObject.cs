using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AvalonStudio.Utils
{
    public class SerializedObject
	{
		public static void Serialize(string filename, object item)
		{
			var writer = new StreamWriter(filename);
			writer.Write(JsonConvert.SerializeObject(item, Formatting.Indented,
				new JsonSerializerSettings
				{
					NullValueHandling = NullValueHandling.Ignore,
					Converters = new[] {new StringEnumConverter()}
				}));
			writer.Close();
		}

		public static T FromString<T>(string data)
		{
			return JsonConvert.DeserializeObject<T>(data);
		}

		public static T Deserialize<T>(string filename)
		{
			var reader = new StreamReader(filename);

			var result = JsonConvert.DeserializeObject<T>(reader.ReadToEnd());

			reader.Close();

			return result;
		}
	}
}