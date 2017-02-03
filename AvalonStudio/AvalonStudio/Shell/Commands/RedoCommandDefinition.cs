using System;
using System.Windows.Input;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Media;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Commands;
using ReactiveUI;
using Key = Avalonia.Input.Key;
using AvalonStudio.Controls;

namespace AvalonStudio.Shell.Commands
{
	[CommandDefinition]
	public class RedoCommandDefinition : CommandDefinition
	{
		public static CommandKeyboardShortcut KeyGesture =
			new CommandKeyboardShortcut<RedoCommandDefinition>(new KeyGesture
			{
				Key = Key.Y,
				Modifiers = InputModifiers.Control
			});

		private readonly ReactiveCommand<object> _command;

		public RedoCommandDefinition()
		{
			_command = ReactiveCommand.Create();

			_command.Subscribe(_ =>
			{
				var shell = IoC.Get<IShell>();

                (shell.SelectedDocument as EditorViewModel)?.Redo();
			});
		}

		public override string Text => "Redo";

		public override string ToolTip => "Redo the last undo command.";

		public override Path IconPath
			=>
				new Path
				{
					Fill = Brush.Parse("#FF7AC1FF"),
					UseLayoutRounding = false,
					Stretch = Stretch.Uniform,
					Data =
						StreamGeometry.Parse(
							"M18.4,10.6C16.55,9 14.15,8 11.5,8C6.85,8 2.92,11.03 1.54,15.22L3.9,16C4.95,12.81 7.95,10.5 11.5,10.5C13.45,10.5 15.23,11.22 16.62,12.38L13,16H22V7L18.4,10.6Z")
				};

		public override Uri IconSource => new Uri("");
		public override ICommand Command => _command;
	}
}