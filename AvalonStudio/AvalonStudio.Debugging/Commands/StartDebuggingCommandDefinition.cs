using System;
using System.Composition;
using System.Windows.Input;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Media;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Commands;
using AvalonStudio.Shell;
using ReactiveUI;
using Key = Avalonia.Input.Key;

namespace AvalonStudio.Debugging.Commands
{
	[CommandDefinition]
	internal class StartDebuggingCommandDefinition : CommandDefinition
	{
		public static CommandKeyboardShortcut KeyGesture =
			new CommandKeyboardShortcut<StartDebuggingCommandDefinition>(new KeyGesture {Key = Key.F5});

		private readonly ReactiveCommand<object> command;

		public StartDebuggingCommandDefinition()
		{
			command = ReactiveCommand.Create();
			command.Subscribe(_ =>
			{
				var manager = IoC.Get<IDebugManager>();

				if (manager.CurrentDebugger == null)
				{
					var shell = IoC.Get<IShell>();

					var project = shell.GetDefaultProject();

					if (project != null)
					{
						manager.StartDebug(project);
					}
				}
				else
				{
					manager.Continue();
				}
			});
		}

		public override ICommand Command
		{
			get { return command; }
		}

		public override string Text
		{
			get { return "Start Debugging"; }
		}

		public override string ToolTip
		{
			get { return "Starts a debug session."; }
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
	}
}