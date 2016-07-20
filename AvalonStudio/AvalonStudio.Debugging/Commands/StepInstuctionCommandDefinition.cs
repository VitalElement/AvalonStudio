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
	internal class StepInstructionCommandDefinition : CommandDefinition
	{
		[Export] public static CommandKeyboardShortcut KeyGesture =
			new CommandKeyboardShortcut<StepInstructionCommandDefinition>(new KeyGesture {Key = Key.F9});

		private readonly ReactiveCommand<object> command;

		public StepInstructionCommandDefinition()
		{
			command = ReactiveCommand.Create();
			command.Subscribe(_ =>
			{
				var manager = IoC.Get<IDebugManager>();

				manager.StepInstruction();
			});
		}

		public override Path IconPath
		{
			get
			{
				return new Path
				{
					Fill = Brush.Parse("#FF8DD28A"),
					UseLayoutRounding = false,
					Stretch = Stretch.Uniform,
					Data = StreamGeometry.Parse("M8,5.14V19.14L19,12.14L8,5.14Z")
				};
			}
		}

		public override ICommand Command
		{
			get { return command; }
		}

		public override string Text
		{
			get { return "Step Instruction"; }
		}

		public override string ToolTip
		{
			get { return "Steps a single instruction."; }
		}
	}
}