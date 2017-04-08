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


namespace AvalonStudio.Debugging.Commands
{
	
	//internal class StartDebuggingCommandDefinition : CommandDefinition
	//{
	//	private readonly ReactiveCommand<object> command;

	//	public StartDebuggingCommandDefinition()
	//	{
	//		command = ReactiveCommand.Create();
	//		command.Subscribe(_ =>
	//		{
	//			var manager = IoC.Get<IDebugManager2>();

	//			if (manager.CurrentDebugger == null)
	//			{
	//				var shell = IoC.Get<IShell>();

	//				var project = shell.GetDefaultProject();

	//				if (project != null)
	//				{
	//					manager.StartDebug(project);
	//				}
	//			}
	//			else
	//			{
	//				manager.Continue();
	//			}
	//		});
	//	}

	//	public override ICommand Command
	//	{
	//		get { return command; }
	//	}

	//	public override string Text
	//	{
	//		get { return "Start Debugging"; }
	//	}

	//	public override string ToolTip
	//	{
	//		get { return "Starts a debug session."; }
	//	}

	//	public override Path IconPath
	//	{
	//		get
	//		{
	//			return new Path
	//			{
	//				Fill = Brush.Parse("#FF8DD28A"),
	//				UseLayoutRounding = false,
	//				Stretch = Stretch.Uniform,
	//				Data = StreamGeometry.Parse("M8,5.14V19.14L19,12.14L8,5.14Z")
	//			};
	//		}
	//	}

 //       public override KeyGesture Gesture => KeyGesture.Parse("F5");
 //   }
}