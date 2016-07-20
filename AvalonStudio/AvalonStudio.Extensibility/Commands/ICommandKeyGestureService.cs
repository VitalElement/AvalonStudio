using Avalonia.Controls;
using Avalonia.Input;

namespace AvalonStudio.Extensibility.Commands
{
	public interface ICommandKeyGestureService
	{
		void BindKeyGestures(Control uiElement);
		KeyGesture GetPrimaryKeyGesture(CommandDefinitionBase commandDefinition);
	}
}