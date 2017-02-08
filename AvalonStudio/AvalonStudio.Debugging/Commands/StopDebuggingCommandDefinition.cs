using System;
using System.Windows.Input;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Commands;
using ReactiveUI;

namespace AvalonStudio.Debugging.Commands
{
	
	internal class StopDebuggingCommandDefinition : CommandDefinition
	{
		private readonly ReactiveCommand<object> command;

		public StopDebuggingCommandDefinition()
		{
			command = ReactiveCommand.Create();
			command.Subscribe(_ =>
			{
				var manager = IoC.Get<IDebugManager>();

				manager.Stop();
			});
		}

		public override Path IconPath
		{
			get
			{
				return new Path
				{
					Fill = Brush.Parse("#FFF38B76"),
					UseLayoutRounding = false,
					Stretch = Stretch.Uniform,
					Data = StreamGeometry.Parse("M18,18H6V6H18V18Z")
				};
			}
		}

		public override ICommand Command
		{
			get { return command; }
		}

		public override string Text
		{
			get { return "Stop"; }
		}

		public override string ToolTip
		{
			get { return "Stops the current debug session."; }
		}
	}
}