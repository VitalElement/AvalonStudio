using Avalonia.Controls.Shapes;
using Avalonia.Media;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Commands;
using ReactiveUI;
using System;
using System.Windows.Input;

namespace AvalonStudio.Shell.Commands
{
    public class SaveAllFileCommandDefinition : CommandDefinition
    {
        private readonly ReactiveCommand _command;

        public SaveAllFileCommandDefinition()
        {
            _command = ReactiveCommand.Create(() =>
            {
                var shell = IoC.Get<IShell>();

                shell?.SaveAll();
            });
        }

        public override string Text => "Save All";

        public override string ToolTip => "Saves all modified documents.";

        public override DrawingGroup Icon => this.GetCommandIcon("SaveAll");

        public override ICommand Command => _command;
    }
}