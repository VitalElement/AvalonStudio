using AvalonStudio.Platforms;
using AvalonStudio.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;

namespace AvalonStudio.GlobalSettings
{
    public class SettingsSerializer
    {
        public static T GetSettings<T>(Func<dynamic> getRoot, Action save) where T : new()
        {
            T result = default(T);

            var rootIndex = (IDictionary<string, object>)getRoot();
            var root = getRoot();

            if (!rootIndex.ContainsKey(typeof(T).FullName))
            {
                return ProvisionSettings<T>(getRoot, save);
            }
            if (rootIndex[typeof(T).FullName] is ExpandoObject)
            {
                result = (rootIndex[typeof(T).FullName] as ExpandoObject).GetConcreteType<T>();
            }
            else
            {
                result = (T)rootIndex[typeof(T).FullName];
            }

            return result;
        }

        public static void SetSettings<T>(Func<dynamic> getRoot, Action save, T value) where T : new()
        {
            var rootIndex = (IDictionary<string, object>)getRoot();
            rootIndex[typeof(T).FullName] = value;

            save();
        }

        public static T ProvisionSettings<T>(Func<dynamic> getRoot, Action save) where T : new()
        {
            var result = new T();
            var rootIndex = (IDictionary<string, object>)getRoot();

            rootIndex[typeof(T).FullName] = result;

            save();

            return (T)rootIndex[typeof(T).FullName];
        }
    }

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
                return SerializedObject.Deserialize<Settings>(GlobalSettingsFile);
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