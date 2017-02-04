using System;
using System.Windows.Input;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using AvalonStudio.Extensibility.Commands;
using ReactiveUI;

namespace AvalonStudio.Shell.Commands
{
	public class CloseFileCommandDefinition : CommandDefinition
	{
		private ReactiveCommand<object> _command;
		public override string Text => "Close";

		public override string ToolTip => "Close ToolTip";
		public override ICommand Command => _command;

        public override Path IconPath => null;

        public override KeyGesture Gesture => null;
    }
}