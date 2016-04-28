namespace AvalonStudio.Extensibility.MainMenu.Models
{
    using System;
    using System.Windows.Input;

    public abstract class StandardMenuItem : MenuItemBase
    {
        public abstract string Text { get; }
        public abstract Uri IconSource { get; }
        public abstract string InputGestureText { get; }
        public abstract ICommand Command { get; }
        public abstract bool IsChecked { get; }
        public abstract bool IsVisible { get; }
    }
}