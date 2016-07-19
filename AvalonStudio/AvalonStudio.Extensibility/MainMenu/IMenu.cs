namespace AvalonStudio.Extensibility.MainMenu
{
	using Models;
	using MVVM;
	using System.Collections;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Collections.Specialized;
	using System.ComponentModel;

	public interface IMenu : IObservableCollection<MenuItemBase>
	{
		
	}
}