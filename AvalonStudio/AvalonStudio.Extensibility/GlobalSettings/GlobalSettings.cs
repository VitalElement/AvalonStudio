using AvalonStudio.Platforms;
using AvalonStudio.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;

namespace AvalonStudio.GlobalSettings
{
    public class Settings
    {
        private dynamic _root = new ExpandoObject();

        private IDictionary<string, object> _rootIndex => ((IDictionary<string, object>)_root);

        [JsonConverter(typeof(ExpandoObjectConverter))]
        public ExpandoObject Root
        {
            get
            {
                return _root;
            }
            set
            {
                _root = value;
            }
        }

        private static string GlobalSettingsFile => Path.Combine(Platform.SettingsDirectory, "GlobalSettings.json");

        public static Settings Instance { get; set; }

        static Settings()
        {
            Instance = Load();
        }

        private static Settings Load()
        {
            if (File.Exists(GlobalSettingsFile))
            {
                var deserialized = SerializedObject.Deserialize<Settings>(GlobalSettingsFile);

                if(deserialized != null)
                {
                    return deserialized;
                }
            }

            var result = new Settings();

            result.Save();

            return result;
        }

        public void Save()
        {
            SerializedObject.Serialize(GlobalSettingsFile, this);
        }

        private T GetSettingsImpl<T>() where T : new()
        {
            return SettingsSerializer.GetSettings<T>(() => Root, () => Save());
        }

        private void SetSettingsImpl<T>(T value) where T : new()
        {
            SettingsSerializer.SetSettings<T>(() => Root, () => Save(), value);
        }

        public static T GetSettings<T>() where T : new()
        {
            return Instance.GetSettingsImpl<T>();
        }

        public static void SetSettings<T>(T value) where T : new()
        {
            Instance.SetSettingsImpl(value);
        }
    }
}