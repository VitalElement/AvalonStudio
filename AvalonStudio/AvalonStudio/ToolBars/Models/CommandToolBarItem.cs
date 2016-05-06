namespace AvalonStudio.ToolBars.Models
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using Extensibility.ToolBars;
    using Extensibility.Commands;
    using Perspex.Input;
    using System.Windows.Input;
    using Extensibility;
    public class CommandToolBarItem : ToolBarItemBase, ICommandUiItem
    {
	    private readonly ToolBarItemDefinition _toolBarItem;
	    private readonly Command _command;
        private readonly KeyGesture _keyGesture;
        private readonly IToolBar _parent;

		public string Text
		{
			get { return _command.Text; }
		}

        public ToolBarItemDisplay Display
        {
            get { return _toolBarItem.Display; }
        }

	    public Uri IconSource
	    {
	        get { return _command.IconSource; }
	    }

	    public string ToolTip
	    {
	        get
	        {
                // TODO parse keygesture for tool tip.

                return string.Format("{0}{1}", _command.ToolTip, string.Empty).Trim();
	        }
	    }

	    public bool HasToolTip
	    {
            get { return !string.IsNullOrWhiteSpace(ToolTip); }
	    }

        private ICommand __command;
        public ICommand Command
        {
            get { return __command; }
        }

        public bool IsChecked
        {
            get { return _command.Checked; }
        }

		public CommandToolBarItem(ToolBarItemDefinition toolBarItem, Command command, IToolBar parent)
		{
		    _toolBarItem = toolBarItem;
		    _command = command;
            _keyGesture = IoC.Get<ICommandKeyGestureService>().GetPrimaryKeyGesture(_command.CommandDefinition);
            _parent = parent;

            command.PropertyChanged += OnCommandPropertyChanged;
		}

        private void OnCommandPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            NotifyOfPropertyChange(() => Text);
            NotifyOfPropertyChange(() => IconSource);
            NotifyOfPropertyChange(() => ToolTip);
            NotifyOfPropertyChange(() => HasToolTip);
            NotifyOfPropertyChange(() => IsChecked);
        }

	    CommandDefinitionBase ICommandUiItem.CommandDefinition
	    {
	        get { return _command.CommandDefinition; }
	    }

        void ICommandUiItem.Update(CommandHandlerWrapper commandHandler)
	    {
	        // TODO
	    }
    }
}