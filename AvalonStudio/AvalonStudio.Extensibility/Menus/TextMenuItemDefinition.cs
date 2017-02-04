using System;
using Avalonia.Input;
using AvalonStudio.Extensibility.Commands;
using System.Composition;

namespace AvalonStudio.Extensibility.Menus
{
    [PartNotDiscoverable]
    public class TextMenuItemDefinition : MenuItemDefinition
    {
        public TextMenuItemDefinition(Func<MenuItemGroupDefinition> group, string text, int sortOrder, Func<CommandDefinition> command) : base(group, text, sortOrder, command)
        {
        }
    }
}