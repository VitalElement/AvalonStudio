using AvalonStudio.Utils;
using System;
using System.Collections.Generic;
using System.Dynamic;

namespace AvalonStudio.Projects
{
    public static class IProjectExtensions
    {
        public static T GetSettings<T>(this IProject project, Func<dynamic> getRoot) where T : new()
        {
            T result = default(T);

            try
            {
                var rootIndex = (IDictionary<string, object>)getRoot();
                var root = getRoot();

                if (!rootIndex.ContainsKey(typeof(T).FullName))
                {
                    return project.ProvisionSettings<T>(getRoot);
                }

                if (rootIndex[typeof(T).FullName] is ExpandoObject)
                {
                    result = (rootIndex[typeof(T).FullName] as ExpandoObject).GetConcreteType<T>();
                }
                else
                {
                    result = (T)rootIndex[typeof(T).FullName];
                }
            }
            catch (Exception)
            {
                return project.ProvisionSettings<T>(getRoot);
            }

            return result;
        }

        public static void SetSettings<T>(this IProject project, T value, Func<dynamic> getRoot)
        {
            var rootIndex = (IDictionary<string, object>)getRoot();
            rootIndex[typeof(T).FullName] = value;

            project.Save();
        }

        private static T ProvisionSettings<T>(this IProject project, Func<dynamic> getRoot) where T : new()
        {
            var result = new T();

            var rootIndex = (IDictionary<string, object>)getRoot();
            rootIndex[typeof(T).FullName] = result;

            project.Save();

            return result;
        }

        private static T ProvisionToolchainSettings<T>(this IProject project) where T : new()
        {
            return ProvisionSettings<T>(project, () => project.ToolchainSettings);
        }

        public static T GetToolchainSettings<T>(this IProject project) where T : new()
        {
            return GetSettings<T>(project, () => project.ToolchainSettings);
        }

        public static void SetToolchainSettings<T>(this IProject project, T value)
        {
            SetSettings<T>(project, value, () => project.ToolchainSettings);
        }

        private static T ProvisionDebuggerSettings<T>(this IProject project) where T : new()
        {
            return ProvisionSettings<T>(project, () => project.DebugSettings);
        }

        public static T GetDebuggerSettings<T>(this IProject project) where T : new()
        {
            return GetSettings<T>(project, () => project.DebugSettings);
        }

        public static void SetDebuggerSettings<T>(this IProject project, T value)
        {
            SetSettings<T>(project, value, () => project.DebugSettings);
        }
    }
}