namespace AvalonStudio.Shell.Commands
{
	using System;
	using System.ComponentModel.Composition;
	using Avalonia.Controls.Shapes;
	using Avalonia.Input;
	using Avalonia.Media;
	using Extensibility;
	using Extensibility.Commands;
	using ReactiveUI;

	[CommandDefinition]
	public class CleanCommandDefinition : CommandDefinition
	{
		public CleanCommandDefinition()
		{
			_command = ReactiveCommand.Create();

			_command.Subscribe(_ =>
			{
				var shell = IoC.Get<IShell>();

				shell.Clean();
			});
		}

		public override string Text => "Clean";

		public override string ToolTip => "Cleans the Startup Project.";

		public override Path IconPath => new Path() { Fill = Brush.Parse("#FF0F8CD3"), UseLayoutRounding = false, Stretch = Stretch.Uniform, Data = StreamGeometry.Parse("M15,16H19V18H15V16M15,8H22V10H15V8M15,12H21V14H15V12M3,18A2,2 0 0,0 5,20H11A2,2 0 0,0 13,18V8H3V18M14,5H11L10,4H6L5,5H2V7H14V5Z") };

		public override Uri IconSource => new Uri("");

		readonly ReactiveCommand<object> _command;
		public override System.Windows.Input.ICommand Command => _command;

		[Export]
		public static CommandKeyboardShortcut KeyGesture = new CommandKeyboardShortcut<SaveFileCommandDefinition>(new KeyGesture() { Key = Key.S, Modifiers = InputModifiers.Control });
	}
}