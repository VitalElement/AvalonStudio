using AvalonStudio.Utils;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace AvalonStudio.Projects
{
    public static class IProjectExtensions
    {
        public static T GetSettings<T>(this IProject project)
        {
            T result = default(T);

            try
            {
                var rootIndex = (IDictionary<string, object>)project.ToolchainSettings;
                var root = project.ToolchainSettings;

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
            }

            return result;
        }

        public static T ProvisionSettings<T>(this IProject project) where T : new()
        {
            var result = project.GetSettings<T>();

            if (result == null)
            {
                result = new T();

                var rootIndex = (IDictionary<string, object>)project.ToolchainSettings;
                rootIndex[typeof(T).FullName] = result;

                project.Save();
            }

            return result;
        }
    }
}
