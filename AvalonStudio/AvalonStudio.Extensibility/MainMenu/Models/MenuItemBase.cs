using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AvalonStudio.Extensibility.Utils;
using ReactiveUI;

namespace AvalonStudio.Extensibility.MainMenu.Models
{
	public class MenuItemBase : ReactiveObject, IEnumerable<MenuItemBase>
	{
		#region Constructors

		protected MenuItemBase()
		{
			Children = new ObservableCollection<MenuItemBase>();
		}

		#endregion

		#region Static stuff

		public static MenuItemBase Separator
		{
			get { return new MenuItemSeparator(); }
		}

		#endregion

		#region Properties

		public ObservableCollection<MenuItemBase> Children { get; }

		#endregion

		public IEnumerator<MenuItemBase> GetEnumerator()
		{
			return Children.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void Add(params MenuItemBase[] menuItems)
		{
			menuItems.Apply(Children.Add);
		}
	}
}