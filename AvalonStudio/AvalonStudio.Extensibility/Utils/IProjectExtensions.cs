using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace AvalonStudio.Utils
{
	public static class Mapper
	{
		public static void Map(ExpandoObject source, Type resultType, object destination)
		{
			var _propertyMap =
				resultType
					.GetProperties()
					.ToDictionary(
						p => p.Name.ToLower(),
						p => p
					);

			// Might as well take care of null references early.
			if (source == null)
				throw new ArgumentNullException("source");

			// By iterating the KeyValuePair<string, object> of
			// source we can avoid manually searching the keys of
			// source as we see in your original code.
			foreach (var kv in source)
			{
				PropertyInfo p;
				if (_propertyMap.TryGetValue(kv.Key.ToLower(), out p))
				{
					var propType = p.PropertyType;
					if (kv.Value == null)
					{
						if (!propType.IsByRef && propType.Name != "Nullable`1")
						{
							// Throw if type is a value type 
							// but not Nullable<>
							throw new ArgumentException("not nullable");
						}
					}
					else if (kv.Value.GetType() == typeof (ExpandoObject))
					{
						var obj = Activator.CreateInstance(propType);
						Map(kv.Value as ExpandoObject, propType, obj);
						p.SetValue(destination, obj);
					}
					else if (kv.Value.GetType() == typeof (List<object>))
					{
						if (p.PropertyType == typeof (List<string>))
						{
							p.SetValue(destination, (kv.Value as List<object>).Cast<string>().ToList(), null);
						}
					}
					else
					{                        
						if (p.PropertyType.GetTypeInfo().IsEnum)
						{
							p.SetValue(destination, Enum.Parse(p.PropertyType, kv.Value as string));
						}
						else if (kv.Value is IConvertible)
						{
							p.SetValue(destination, Convert.ChangeType(kv.Value, p.PropertyType), null);
						}
						else
						{
							p.SetValue(destination, kv.Value, null);
						}
					}
				}
			}
		}
	}

	public static class IProjectExtensions
	{
		public static T GetConcreteType<T>(this ExpandoObject obj)
		{
			//Note: this doesnt work on linux, if object has properties that themselves are expandoobjects, event though it works on windows.
			//var jsSerializer = new JavaScriptSerializer();
			//return jsSerializer.ConvertToType<T>(obj);

			var result = (T) Activator.CreateInstance(typeof (T));

			Mapper.Map(obj, typeof (T), result);

			return result;
		}
	}
}