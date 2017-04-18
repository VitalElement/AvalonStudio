using System;
using System.Dynamic;

namespace AvalonStudio.Utils
{
    public static class IProjectExtensions
    {
        public static T GetConcreteType<T>(this ExpandoObject obj)
        {
            var result = (T)Activator.CreateInstance(typeof(T));

            Mapper.Map(obj, typeof(T), result);

            return result;
        }
    }
}