using AvalonStudio.Extensibility.Commands;
using ReactiveUI;
using System.Windows.Input;

namespace AvalonStudio.Controls.Standard.CodeEditor.Commands
{
    internal class GotoDefinitionCommand : CommandDefinition
    {
        private readonly ReactiveCommand command;

        public GotoDefinitionCommand()
        {
            command = ReactiveCommand.Create(() =>
            {
                System.Console.WriteLine("Goto definition clicked");
            });
        }

        public override ICommand Command
        {
            get { return command; }
        }

        public override string Text
        {
            get { return "Goto Definition"; }
        }

        public override string ToolTip
        {
            get { return ""; }
        }
    }
}
