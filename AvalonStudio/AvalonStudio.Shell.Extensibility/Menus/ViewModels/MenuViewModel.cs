using AvalonStudio.Menus.Models;
using ReactiveUI;
using System.Collections.Immutable;

namespace AvalonStudio.Menus.ViewModels
{
    public class MenuViewModel : ReactiveObject
    {
        public IImmutableList<MenuItemModel> Items { get; }

        public MenuViewModel(IImmutableList<MenuItemModel> items)
        {
            Items = items;
        }
    }
}