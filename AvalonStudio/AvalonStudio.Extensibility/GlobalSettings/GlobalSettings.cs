﻿using AvalonStudio.Platforms;
using AvalonStudio.Utils;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Text;

namespace AvalonStudio.GlobalSettings
{
    public class SettingsBase
    {
        public static T GetSettings<T>() where T : new()
        {
            try
            {
                return Settings.Instance.GetSettings<T>();
            }
            catch (Exception e)
            {
                return ProvisionSettings<T>();
            }
        }

        public static T ProvisionSettings<T>() where T : new()
        {
            return Settings.Instance.ProvisionSettings<T>();            
        }

        public void Save()
        {
            Settings.Instance.Save();
        }
    }

    public class Settings
    {
        private dynamic _root = new ExpandoObject();

        private IDictionary<string, object> _rootIndex => ((IDictionary<string, object>)_root);

        private static string GlobalSettingsFile => Path.Combine(Platform.SettingsDirectory, "GlobalSettings.json");

        public static Settings Instance { get; private set; }

        static Settings ()
        {
            Instance = Load();
        }

        private static Settings Load ()
        {
            if(File.Exists(GlobalSettingsFile))
            {
                return SerializedObject.Deserialize<Settings>(GlobalSettingsFile);
            }

            return new Settings();
        }

        public void Save()
        {
            SerializedObject.Serialize(GlobalSettingsFile, _root);
        }
        
        public T GetSettings<T>()
        {
            T result = default(T);
            
            if (_rootIndex[typeof(T).FullName] is ExpandoObject)
            {
                result = (_rootIndex[typeof(T).FullName] as ExpandoObject).GetConcreteType<T>();
            }
            else
            {
                result = (T)_rootIndex[typeof(T).FullName];
            }

            return result;
        }

        public T ProvisionSettings<T>() where T : new()
        {
            _rootIndex[typeof(T).FullName] = new T();

            Save();

            return (T)_rootIndex[typeof(T).FullName];
        }
    }
}