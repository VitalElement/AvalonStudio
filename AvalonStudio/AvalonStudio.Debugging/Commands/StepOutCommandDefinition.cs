using System;
using System.Windows.Input;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Media;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Commands;
using ReactiveUI;
using Key = Avalonia.Input.Key;

namespace AvalonStudio.Debugging.Commands
{
	[CommandDefinition]
	internal class StepOutCommandDefinition : CommandDefinition
	{
		public static CommandKeyboardShortcut KeyGesture =
			new CommandKeyboardShortcut<StepOverCommandDefinition>(new KeyGesture
			{
				Key = Key.F11,
				Modifiers = InputModifiers.Shift
			});

		private readonly ReactiveCommand<object> command;

		public StepOutCommandDefinition()
		{
			command = ReactiveCommand.Create();
			command.Subscribe(_ =>
			{
				var manager = IoC.Get<IDebugManager>();

				manager.StepOut();
			});
		}

		public override Path IconPath
		{
			get
			{
				return new Path
				{
					Fill = Brush.Parse("#FF7AC1FF"),
					UseLayoutRounding = false,
					Stretch = Stretch.Uniform,
					Data =
						StreamGeometry.Parse(
							"M12,22A2,2 0 0,1 10,20A2,2 0 0,1 12,18A2,2 0 0,1 14,20A2,2 0 0,1 12,22M13,16H11V6L6.5,10.5L5.08,9.08L12,2.16L18.92,9.08L17.5,10.5L13,6V16Z")
				};
			}
		}

		public override ICommand Command
		{
			get { return command; }
		}

		public override string Text
		{
			get { return "Step Out"; }
		}

		public override string ToolTip
		{
			get { return "Steps out of the current function or method."; }
		}
	}
}