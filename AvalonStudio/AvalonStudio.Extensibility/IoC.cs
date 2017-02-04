using System;
using Splat;

namespace AvalonStudio.Extensibility
{
	public static class IoC
	{
		public static object Get(Type t, string contract = null)
		{
			return Locator.CurrentMutable.GetService(t, contract);
		}

		public static T Get<T>(string contract = null)
		{
			return (T) Get(typeof (T), contract);
		}

        public static void RegisterConstant<T> (T instance, string contract = "")
        {
            RegisterConstant(instance, typeof(T), contract);
        }

		public static void RegisterConstant<T>(T instance, Type type, string contract = "")
		{
			Locator.CurrentMutable.RegisterConstant(instance, type, contract);    
		}
	}
}