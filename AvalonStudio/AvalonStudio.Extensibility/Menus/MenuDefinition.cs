namespace AvalonStudio.Extensibility.Menus
{
    using Commands;
    using Avalonia.Input;
    using System;

    public class MenuDefinition : MenuDefinitionBase
    {
        private readonly MenuBarDefinition _menuBar;
        private readonly int _sortOrder;
        private readonly string _text;

        public MenuBarDefinition MenuBar
        {
            get { return _menuBar; }
        }

        public override int SortOrder
        {
            get { return _sortOrder; }
        }

        public override string Text
        {
            get { return _text; }
        }

        public override Uri IconSource
        {
            get { return null; }
        }

        public override KeyGesture KeyGesture
        {
            get { return null; }
        }

        public override CommandDefinitionBase CommandDefinition
        {
            get { return null; }
        }

        public MenuDefinition(MenuBarDefinition menuBar, int sortOrder, string text)
        {
            _menuBar = menuBar;
            _sortOrder = sortOrder;
            _text = text;
        }
    }
}