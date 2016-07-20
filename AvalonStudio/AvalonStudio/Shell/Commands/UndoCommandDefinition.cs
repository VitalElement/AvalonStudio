using System;
using System.ComponentModel.Composition;
using System.Windows.Input;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Media;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Commands;
using ReactiveUI;
using Key = Avalonia.Input.Key;

namespace AvalonStudio.Shell.Commands
{
	[CommandDefinition]
	public class UndoCommandDefinition : CommandDefinition
	{
		[Export] public static CommandKeyboardShortcut KeyGesture =
			new CommandKeyboardShortcut<SaveFileCommandDefinition>(new KeyGesture
			{
				Key = Key.S,
				Modifiers = InputModifiers.Control
			});

		private readonly ReactiveCommand<object> _command;

		public UndoCommandDefinition()
		{
			_command = ReactiveCommand.Create();

			_command.Subscribe(_ =>
			{
				var shell = IoC.Get<IShell>();

				shell.SelectedDocument.Undo();
			});
		}

		public override string Text => "Undo";

		public override string ToolTip => "Undo the last change.";

		public override Path IconPath
			=>
				new Path
				{
					Fill = Brush.Parse("#FF7AC1FF"),
					UseLayoutRounding = false,
					Stretch = Stretch.Uniform,
					Data =
						StreamGeometry.Parse(
							"M12.5,8C9.85,8 7.45,9 5.6,10.6L2,7V16H11L7.38,12.38C8.77,11.22 10.54,10.5 12.5,10.5C16.04,10.5 19.05,12.81 20.1,16L22.47,15.22C21.08,11.03 17.15,8 12.5,8Z")
				};

		public override Uri IconSource => new Uri("");
		public override ICommand Command => _command;
	}
}