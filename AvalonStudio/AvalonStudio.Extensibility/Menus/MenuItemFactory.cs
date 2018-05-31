using Avalonia.Media;
using AvalonStudio.Commands;
using System.Composition;

namespace AvalonStudio.Menus
{
    [Export(typeof(IMenuItemFactory))]
    [Shared]
    internal class MenuItemFactory : IMenuItemFactory
    {
        private CommandService _commandsService;

        [ImportingConstructor]
        public MenuItemFactory(CommandService commandsService)
        {
            _commandsService = commandsService;
        }

        public IMenuItem CreateCommandMenuItem(string commandName) => new CommandMenuItem(_commandsService, commandName);
        public IMenuItem CreateHeaderMenuItem(string label, DrawingGroup icon) => new HeaderMenuItem(label, icon);
    }
}
