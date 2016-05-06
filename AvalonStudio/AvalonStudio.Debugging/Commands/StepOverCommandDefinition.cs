namespace AvalonStudio.Controls.Standard.SolutionExplorer.Commands
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
    class StepOverCommandDefinition : CommandDefinition
    {
        public StepOverCommandDefinition()
        {
            command = ReactiveCommand.Create();
            command.Subscribe(_ =>
            {
                var manager = IoC.Get<IDebugManager>();

                manager.StepOver();
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
                return "Step Over";
            }
        }

        public override string ToolTip
        {
            get
            {
                return "Steps over the current line.";
            }
        }

        [Export]
        public static CommandKeyboardShortcut KeyGesture = new CommandKeyboardShortcut<StepOverCommandDefinition>(new KeyGesture() { Key = Key.F10 });
    }
}
