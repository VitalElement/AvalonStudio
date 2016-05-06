namespace AvalonStudio.Debugging.Commands
{
    using AvalonStudio.Extensibility.Commands;
    using Debugging;
    using Extensibility;
    using Perspex.Controls;
    using Perspex.Input;
    using Projects;
    using ReactiveUI;
    using Shell;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Windows.Input;
    using Utils;

    [CommandDefinition]
    class StepIntoCommandDefinition : CommandDefinition
    {
        public StepIntoCommandDefinition()
        {
            command = ReactiveCommand.Create();
            command.Subscribe(_ =>
            {
                var manager = IoC.Get<IDebugManager>();

                manager.StepInto();
            });
        }

        private ReactiveCommand<object> command;

        public override ICommand Command
        {
            get
            {
                return command;
            }
        }

        public override string Text
        {
            get
            {
                return "Step Into";
            }
        }

        public override string ToolTip
        {
            get
            {
                return "Steps into the current line.";
            }
        }

        [Export]
        public static CommandKeyboardShortcut KeyGesture = new CommandKeyboardShortcut<StepIntoCommandDefinition>(new KeyGesture() { Key = Key.F11 });
    }
}
