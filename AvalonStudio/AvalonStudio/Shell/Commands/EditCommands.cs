using AvalonStudio.Commands;
using AvalonStudio.Documents;
using AvalonStudio.Extensibility;
using ReactiveUI;
using System.Composition;

namespace AvalonStudio.Shell.Commands
{
    internal class EditCommands
    {
        //[ExportCommandDefinition("Edit.ShowQuickCommander")]
        //[DefaultKeyGestures("CTRL+P")]
        //public CommandDefinition ShowQuickCommanderCommand { get; }

        [ExportCommandDefinition("Edit.Undo")]
        [DefaultKeyGestures("CTRL+Z")]
        public CommandDefinition UndoCommand { get; }

        [ExportCommandDefinition("Edit.Redo")]
        [DefaultKeyGestures("CTRL+Y")]
        public CommandDefinition RedoCommand { get; }

        [ExportCommandDefinition("Edit.Comment")]
        public CommandDefinition CommentCommand { get; }

        [ExportCommandDefinition("Edit.Uncomment")]
        public CommandDefinition UncommentCommand { get; }

        private readonly IShell _shell;

        [ImportingConstructor]
        public EditCommands(CommandIconService commandIconService)
        {
            _shell = IoC.Get<IShell>();

            //ShowQuickCommanderCommand = new CommandDefinition(
              //  "Show Quick Commander", null, ReactiveCommand.Create(ShowQuickCommander));

            UndoCommand = new CommandDefinition(
                "Undo",
                commandIconService.GetCompletionKindImage("Undo"),
                ReactiveCommand.Create(Undo));

            RedoCommand = new CommandDefinition(
                "Redo",
                commandIconService.GetCompletionKindImage("Redo"),
                ReactiveCommand.Create(Redo));

            CommentCommand = new CommandDefinition(
                "Comment",
                commandIconService.GetCompletionKindImage("CommentCode"),
                ReactiveCommand.Create(Comment));

            UncommentCommand = new CommandDefinition(
                "Uncomment",
                commandIconService.GetCompletionKindImage("UncommentCode"),
                ReactiveCommand.Create(Uncomment));
        }

        //private void ShowQuickCommander() => _shell.ShowQuickCommander();

        private void Undo() => GetSelectedDocumentEditor()?.Undo();
        private void Redo() => GetSelectedDocumentEditor()?.Redo();

        private void Comment() => GetSelectedDocumentEditor()?.Comment();
        private void Uncomment() => GetSelectedDocumentEditor()?.Uncomment();

        private IEditor GetSelectedDocumentEditor() => null;
    }
}
