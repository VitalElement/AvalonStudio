using AvalonStudio.Utils;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace AvalonStudio.Projects
{
    public static class IProjectExtensions
    {
        public static T GetSettings<T>(this IProject project, Func<dynamic, T> selector)
        {
            T result = default(T);

            try
            {
                if (selector(project.ToolchainSettings) is ExpandoObject)
                {
                    result = (selector(project.ToolchainSettings) as ExpandoObject).GetConcreteType<T>();
                }
                else
                {
                    result = selector(project.ToolchainSettings);
                }
            }
            catch (Exception)
            {
            }

            return result;
        }

        public static T ProvisionSettings<T>(this IProject project, Func<dynamic, T> selector, Action<dynamic, T> setter)
        {
            var result = project.GetSettings<T>(selector);

            if (result == null)
            {
                result = default(T);

                setter(project.ToolchainSettings, result);

                project.Save();
            }

            return result;
        }
    }
}
