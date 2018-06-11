using AvalonStudio.Utils;
using System;
using System.Collections.Generic;
using System.Dynamic;

namespace AvalonStudio.GlobalSettings
{
    public static class ExpandoObjectExtensions
    {
        public static T GetConcreteType<T>(this ExpandoObject obj)
        {
            var result = (T)Activator.CreateInstance(typeof(T));

            Mapper.Map(obj, typeof(T), result);

            return result;
        }
    }

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

        public static T GetSettingsIfExists<T>(Func<dynamic> getRoot) where T : new()
        {
            T result = default(T);

            var rootIndex = (IDictionary<string, object>)getRoot();
            var root = getRoot();

            if (rootIndex.ContainsKey(typeof(T).FullName))
            {
                if (rootIndex[typeof(T).FullName] is ExpandoObject)
                {
                    result = (rootIndex[typeof(T).FullName] as ExpandoObject).GetConcreteType<T>();
                }
                else
                {
                    result = (T)rootIndex[typeof(T).FullName];
                }
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
}