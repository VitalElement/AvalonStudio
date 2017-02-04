using System;
using System.ComponentModel;
using System.Windows.Input;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using AvalonStudio.Extensibility.Commands;
using ReactiveUI;

namespace AvalonStudio.Extensibility.ToolBars.Models
{
	public class CommandToolBarItem : ToolBarItemBase
	{
		private readonly ICommand _command;
		private readonly KeyGesture _keyGesture;
		private readonly IToolBar _parent;
		private readonly ToolBarItemDefinition _toolBarItem;

		public CommandToolBarItem(ToolBarItemDefinition toolBarItem, ICommand command, ICommand actualCommand, IToolBar parent)
		{
			_toolBarItem = toolBarItem;
			_command = command;
			Command = actualCommand;
			//_keyGesture = IoC.Get<ICommandKeyGestureService>().GetPrimaryKeyGesture(_command.CommandDefinition);
			_parent = parent;

			//command.PropertyChanged += OnCommandPropertyChanged;
		}

		//public string Text => _command.Text;

		public ToolBarItemDisplay Display => _toolBarItem.Display;

		//public Uri IconSource => _command.IconSource;

		//public Path IconPath => _command.CommandDefinition.IconPath;

		//public string ToolTip => $"{_command.ToolTip}{string.Empty}".Trim();

		//public bool HasToolTip => !string.IsNullOrWhiteSpace(ToolTip);

		public ICommand Command { get; }

		//public bool IsChecked => _command.Checked;        
	}
}