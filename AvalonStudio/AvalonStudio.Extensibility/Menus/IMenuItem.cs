using System.Windows.Input;

namespace AvalonStudio.Extensibility.Menus
{
	public interface IMenuItem
	{
		string Title { get; }
		ICommand Command { get; }
	}
}