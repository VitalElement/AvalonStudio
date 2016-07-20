using AvalonStudio.Extensibility.MainMenu.Models;
using AvalonStudio.Extensibility.MVVM;

namespace AvalonStudio.Extensibility.MainMenu
{
	public interface IMenu : IObservableCollection<MenuItemBase>
	{
	}
}