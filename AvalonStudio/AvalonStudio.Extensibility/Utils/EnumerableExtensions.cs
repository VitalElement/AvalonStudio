using System;
using System.Collections.Generic;

namespace AvalonStudio.Extensibility.Utils
{
	public static class EnumerableExtensions
	{
		//
		// Summary:
		//     Applies the action to each element in the list.
		//
		// Parameters:
		//   enumerable:
		//     The elements to enumerate.
		//
		//   action:
		//     The action to apply to each item in the list.
		//
		// Type parameters:
		//   T:
		//     The enumerable item's type.
		public static void Apply<T>(this IEnumerable<T> enumerable, Action<T> action)
		{
			foreach (var item in enumerable)
			{
				action(item);
			}
		}
	}
}