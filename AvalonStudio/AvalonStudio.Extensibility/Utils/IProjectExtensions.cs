using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace AvalonStudio.Extensibility.Utils
{
    public static class IProjectExtensions
    {
        public static T GetConcreteType<T>(this ExpandoObject obj)
        {
            var jsSerializer = new JavaScriptSerializer();

            return jsSerializer.ConvertToType<T>(obj);
        }
    }
}
