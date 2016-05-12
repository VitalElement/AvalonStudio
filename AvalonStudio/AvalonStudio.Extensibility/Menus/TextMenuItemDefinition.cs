namespace AvalonStudio.Extensibility.Menus
{
    using System;
    using Avalonia.Input;
    using Commands;

    public class TextMenuItemDefinition : MenuItemDefinition
    {
        private readonly string _text;
        private readonly Uri _iconSource;

        public override string Text
        {
            get { return _text; }
        }

        public override Uri IconSource
        {
            get { return _iconSource; }
        }

        public override KeyGesture KeyGesture
        {
            get { return null; }
        }

        public override CommandDefinitionBase CommandDefinition
        {
            get { return null; }
        }

        public TextMenuItemDefinition(MenuItemGroupDefinition group, int sortOrder, string text, Uri iconSource = null)
            : base(group, sortOrder)
        {
            _text = text;
            _iconSource = iconSource;
        }
    }
}