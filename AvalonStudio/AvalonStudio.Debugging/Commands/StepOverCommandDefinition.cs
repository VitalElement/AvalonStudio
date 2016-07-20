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

namespace AvalonStudio.Debugging.Commands
{
	[CommandDefinition]
	internal class StepOverCommandDefinition : CommandDefinition
	{
		[Export] public static CommandKeyboardShortcut KeyGesture =
			new CommandKeyboardShortcut<StepOverCommandDefinition>(new KeyGesture {Key = Key.F10});

		private readonly ReactiveCommand<object> command;

		public StepOverCommandDefinition()
		{
			command = ReactiveCommand.Create();
			command.Subscribe(_ =>
			{
				var manager = IoC.Get<IDebugManager>();

				manager.StepOver();
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
							"M12,14A2,2 0 0,1 14,16A2,2 0 0,1 12,18A2,2 0 0,1 10,16A2,2 0 0,1 12,14M23.46,8.86L21.87,15.75L15,14.16L18.8,11.78C17.39,9.5 14.87,8 12,8C8.05,8 4.77,10.86 4.12,14.63L2.15,14.28C2.96,9.58 7.06,6 12,6C15.58,6 18.73,7.89 20.5,10.72L23.46,8.86Z")
				};
			}
		}

		public override ICommand Command
		{
			get { return command; }
		}

		public override string Text
		{
			get { return "Step Over"; }
		}

		public override string ToolTip
		{
			get { return "Steps over the current line."; }
		}
	}
}