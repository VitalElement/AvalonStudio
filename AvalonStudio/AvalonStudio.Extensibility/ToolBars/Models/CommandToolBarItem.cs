namespace AvalonStudio.Extensibility.ToolBars.Models
{
	using System;
	using System.ComponentModel;
	using System.Windows.Input;
	using Avalonia.Controls.Shapes;
	using Avalonia.Input;
	using Commands;
	using Extensibility;
	using ReactiveUI;
	using ToolBars;

	public class CommandToolBarItem : ToolBarItemBase, ICommandUiItem
	{
		private readonly ToolBarItemDefinition _toolBarItem;
		private readonly Command _command;
		private readonly KeyGesture _keyGesture;
		private readonly IToolBar _parent;

		public string Text => _command.Text;

		public ToolBarItemDisplay Display => _toolBarItem.Display;

		public Uri IconSource => _command.IconSource;

		public Path IconPath => _command.CommandDefinition.IconPath;

		public string ToolTip => $"{_command.ToolTip}{string.Empty}".Trim();

		public bool HasToolTip => !string.IsNullOrWhiteSpace(ToolTip);

		public ICommand Command { get; }

		public bool IsChecked => _command.Checked;

		public CommandToolBarItem(ToolBarItemDefinition toolBarItem, Command command, ICommand actualCommand, IToolBar parent)
		{
			_toolBarItem = toolBarItem;
			_command = command;
			Command = actualCommand;
			_keyGesture = IoC.Get<ICommandKeyGestureService>().GetPrimaryKeyGesture(_command.CommandDefinition);
			_parent = parent;

			command.PropertyChanged += OnCommandPropertyChanged;
		}

		private void OnCommandPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this.RaisePropertyChanged(nameof(Text));
			this.RaisePropertyChanged(nameof(IconSource));
			this.RaisePropertyChanged(nameof(ToolTip));
			this.RaisePropertyChanged(nameof(HasToolTip));
			this.RaisePropertyChanged(nameof(IsChecked));
		}

		CommandDefinitionBase ICommandUiItem.CommandDefinition => _command.CommandDefinition;

		void ICommandUiItem.Update(CommandHandlerWrapper commandHandler)
		{
			// TODO
		}
	}
}