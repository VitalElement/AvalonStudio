using AvalonStudio.Menus.Models;
using AvalonStudio.Menus.ViewModels;
using System;
using System.Collections.Immutable;

namespace AvalonStudio.Toolbars.ViewModels
{
    public class ToolbarViewModel : MenuViewModel
    {
        public string Name => _toolbar.Value.Name;

        private readonly Lazy<Toolbar> _toolbar;

        public ToolbarViewModel(Lazy<Toolbar> toolbar, IImmutableList<MenuItemModel> items)
            : base(items)
        {
            _toolbar = toolbar;
        }
    }
}
