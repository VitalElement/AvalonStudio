using Avalonia.Media;
using System;
using System.Windows.Input;

namespace AvalonStudio.Menus.Models
{
    public class MenuItemSeparatorModel : MenuItemModel
    {
        private static readonly Lazy<IMenuItem> EmptyItem = new Lazy<IMenuItem>(() => new EmptyMenuItem());

        public MenuItemSeparatorModel()
            : base(EmptyItem, null)
        {
        }

        private class EmptyMenuItem : IMenuItem
        {
            public string Label => null;
            public DrawingGroup Icon => null;

            public ICommand Command => null;
        }
    }
}
