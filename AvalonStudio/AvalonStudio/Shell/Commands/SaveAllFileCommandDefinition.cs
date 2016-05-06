namespace AvalonStudio.Shell.Commands
{
    using AvalonStudio.Extensibility.Commands;
    using Extensibility;
    using ReactiveUI;
    using System;

    [CommandDefinition]
    public class SaveAllFileCommandDefinition : CommandDefinition
    {
        public SaveAllFileCommandDefinition()
        {
            command = ReactiveCommand.Create();

            command.Subscribe(_ =>
            {
                var shell = IoC.Get<IShell>();

                shell?.SaveAll();
            });
        }

        public override string Text
        {
            get { return "Save All"; }
        }

        public override string ToolTip
        {
            get { return "Save All Tool Tip"; }
        }

        ReactiveCommand<object> command;
        public override System.Windows.Input.ICommand Command
        {
            get
            {
                return command;
            }
        }
    }
}