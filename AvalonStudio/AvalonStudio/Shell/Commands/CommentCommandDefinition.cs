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
using AvalonStudio.Controls;

namespace AvalonStudio.Shell.Commands
{
	[CommandDefinition]
	public class CommentCommandDefinition : CommandDefinition
	{
		[Export] public static CommandKeyboardShortcut KeyGesture =
			new CommandKeyboardShortcut<SaveFileCommandDefinition>(new KeyGesture
			{
				Key = Key.S,
				Modifiers = InputModifiers.Control
			});

		private readonly ReactiveCommand<object> _command;

		public CommentCommandDefinition()
		{
			_command = ReactiveCommand.Create();

			_command.Subscribe(_ =>
			{
				var shell = IoC.Get<IShell>();

				(shell.SelectedDocument as EditorViewModel)?.Comment();
			});
		}

		public override string Text => "Comment";

		public override string ToolTip => "Comments the selected code.";

		public override Path IconPath
			=>
				new Path
				{
					Fill = Brush.Parse("#FF8DD28A"),
					UseLayoutRounding = false,
					Stretch = Stretch.Uniform,
					Data = StreamGeometry.Parse("M3,3H21V5H3V3M9,7H21V9H9V7M3,11H21V13H3V11M9,15H21V17H9V15M3,19H21V21H3V19Z")
				};

		public override Uri IconSource => new Uri("");
		public override ICommand Command => _command;
	}
}