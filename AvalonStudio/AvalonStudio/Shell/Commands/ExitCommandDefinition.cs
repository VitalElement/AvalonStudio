using System;
using System.Windows.Input;
using Avalonia.Input;
using AvalonStudio.Extensibility.Commands;
using ReactiveUI;


namespace AvalonStudio.Shell.Commands
{
	
	public class ExitCommandDefinition : CommandDefinition
	{
        public override KeyGesture Gesture => KeyGesture.Parse("ALT+F4");

        private readonly ReactiveCommand<object> _command;

		public ExitCommandDefinition()
		{
			_command = ReactiveCommand.Create();

			_command.Subscribe(_ => { Environment.Exit(0); });
		}

		public override string Text => "Exit";

		public override string ToolTip => "Exit Tool Tip";
		public override ICommand Command => _command;
	}
}