namespace AvalonStudio.Extensibility.Menus
{
    using Avalonia.Input;
    using AvalonStudio.Extensibility.Commands;
    using AvalonStudio.Extensibility.Plugin;
    using ReactiveUI;
    using System;
    using System.Composition;
    using System.Windows.Input;

    [PartNotDiscoverable]
    public abstract class MenuItemDefinition :  IExtension
	{
        private Func<MenuItemGroupDefinition> _getGroup;
        private Func<CommandDefinition> _getCommand;        

        protected MenuItemDefinition(Func<MenuItemGroupDefinition> group, string text, int sortOrder, Func<CommandDefinition> command)
		{
            _getCommand = command;
			_getGroup = group;
			SortOrder = sortOrder;
            Text = text;
		}

		public MenuItemGroupDefinition Group { get; private set; }

        public CommandDefinition CommandDefinition { get; private set; }


        public string Text { get; }
        public int SortOrder { get; }
        

        public void Activation()
        {
            Group = _getGroup();
            CommandDefinition = _getCommand();
        }

        public void BeforeActivation()
        {
            
        }
    }
}