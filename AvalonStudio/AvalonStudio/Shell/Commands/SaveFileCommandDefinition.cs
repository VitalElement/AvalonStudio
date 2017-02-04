using System;
using System.Windows.Input;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Media;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Commands;
using ReactiveUI;

namespace AvalonStudio.Shell.Commands
{
	
	public class SaveFileCommandDefinition : CommandDefinition
	{
        public override KeyGesture Gesture => KeyGesture.Parse("CTRL+S");

        private readonly ReactiveCommand<object> _command;

		public SaveFileCommandDefinition()
		{
			_command = ReactiveCommand.Create();

			_command.Subscribe(_ =>
			{
				var shell = IoC.Get<IShell>();
				shell?.Save();
			});
		}

		public override string Text => "Save";

		public override string ToolTip => "Saves the current changes.";

		public override Path IconPath
			=>
				new Path
				{
					Fill = Brush.Parse("#FF7AC1FF"),
					UseLayoutRounding = false,
					Stretch = Stretch.Uniform,
					Data =
						StreamGeometry.Parse(
							"M15,9H5V5H15M12,19A3,3 0 0,1 9,16A3,3 0 0,1 12,13A3,3 0 0,1 15,16A3,3 0 0,1 12,19M17,3H5C3.89,3 3,3.9 3,5V19A2,2 0 0,0 5,21H19A2,2 0 0,0 21,19V7L17,3Z")
				};
		
		public override ICommand Command => _command;
	}
}