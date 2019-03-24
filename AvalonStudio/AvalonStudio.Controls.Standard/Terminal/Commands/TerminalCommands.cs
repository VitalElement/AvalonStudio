using AvalonStudio.Commands;
using AvalonStudio.Extensibility;
using AvalonStudio.MainMenu;
using AvalonStudio.Menus;
using AvalonStudio.Shell;
using AvalonStudio.Utils;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Text;

namespace AvalonStudio.Controls.Standard.Terminal.Commands
{
    internal class TerminalCommands
    {
        [ExportCommandDefinition("View.Terminal")]
        public CommandDefinition ViewTerminalCommand { get; }

        [ExportCommandDefinition("View.NewTerminal")]
        public CommandDefinition NewTerminalCommand { get; }

        private readonly IShell _shell;
        private readonly TerminalViewModel _terminal;

        [ImportingConstructor]
        public TerminalCommands(TerminalViewModel terminalViewModel)
        {
            _shell = IoC.Get<IShell>();
            _terminal = terminalViewModel;

            ViewTerminalCommand = new CommandDefinition("Terminal", null,
                ReactiveCommand.Create(() =>
                {
                    _shell.CurrentPerspective.AddOrSelectTool(_terminal);
                }));

            NewTerminalCommand = new CommandDefinition("New Terminal", null, ReactiveCommand.Create(() =>
            {
                _shell.CurrentPerspective.AddOrSelectTool(new TerminalViewModel());
            }));
        }
    }

    internal class TeminalMenuItems
    {
        [ExportMainMenuItem("View", "Terminal")]
        [DefaultGroup("DefaultTools")]
        [DefaultOrder(0)]
        public IMenuItem ViewTerminal => _menuItemFactory.CreateCommandMenuItem("View.Terminal");

        [ExportMainMenuItem("View", "New Terminal")]
        [DefaultGroup("DefaultTools")]
        [DefaultOrder(0)]
        public IMenuItem ViewWelcomeScreen => _menuItemFactory.CreateCommandMenuItem("View.NewTerminal");

        private readonly IMenuItemFactory _menuItemFactory;

        [ImportingConstructor]
        public TeminalMenuItems(IMenuItemFactory menuItemFactory)
        {
            _menuItemFactory = menuItemFactory;
        }
    }
}
