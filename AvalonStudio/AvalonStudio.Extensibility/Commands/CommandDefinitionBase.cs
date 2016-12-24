using System;
using System.Windows.Input;
using Avalonia.Controls.Shapes;

namespace AvalonStudio.Extensibility.Commands
{
	public abstract class CommandDefinitionBase
	{
		public abstract string Text { get; }
		public abstract string ToolTip { get; }
		public abstract Uri IconSource { get; }
		public abstract Path IconPath { get; }
		public abstract bool IsList { get; }
		public abstract ICommand Command { get; }
	}
}