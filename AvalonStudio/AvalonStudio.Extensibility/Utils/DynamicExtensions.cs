using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;

namespace AvalonStudio.Extensibility.Utils
{
	public static class DynamicExtensions
	{
		public static T FromDynamic<T>(this IDictionary<string, object> dictionary)
		{
			var bindings = new List<MemberBinding>();
			foreach (var sourceProperty in typeof (T).GetProperties().Where(x => x.CanWrite))
			{
				var key = dictionary.Keys.SingleOrDefault(x => x.Equals(sourceProperty.Name, StringComparison.OrdinalIgnoreCase));
				if (string.IsNullOrEmpty(key)) continue;
				var propertyValue = dictionary[key];
				bindings.Add(Expression.Bind(sourceProperty, Expression.Constant(propertyValue)));
			}
			Expression memberInit = Expression.MemberInit(Expression.New(typeof (T)), bindings);
			return Expression.Lambda<Func<T>>(memberInit).Compile().Invoke();
		}

		public static dynamic ToDynamic<T>(this T obj)
		{
			IDictionary<string, object> expando = new ExpandoObject();

			foreach (var propertyInfo in typeof (T).GetProperties())
			{
				var propertyExpression = Expression.Property(Expression.Constant(obj), propertyInfo);
				var currentValue = Expression.Lambda<Func<string>>(propertyExpression).Compile().Invoke();
				expando.Add(propertyInfo.Name.ToLower(), currentValue);
			}
			return expando as ExpandoObject;
		}
	}
}