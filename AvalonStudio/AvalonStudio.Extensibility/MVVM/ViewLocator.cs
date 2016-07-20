using System;
using System.Linq;
using Avalonia.Controls;

namespace AvalonStudio.MVVM
{
	public class ViewLocator
	{
		public static IControl Build(object data)
		{
			var name = data.GetType().FullName.Replace("ViewModel", "View");

			var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a => name.Contains(a.GetName().Name));

			Type type = null;

			foreach (var assembly in assemblies)
			{
				type = assembly.GetType(name);

				if (type != null)
				{
					break;
				}
			}

			if (type != null)
			{
				return (Control) Activator.CreateInstance(type);
			}
			return new TextBlock {Text = data.GetType().FullName};
		}
	}
}